using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using Microsoft.AspNet.Identity;

namespace SessionFixation
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        void Application_PreRequestHandlerExecute(Object sender, EventArgs e)
        {
            if (Context.Handler is IRequiresSessionState || Context.Handler is IReadOnlySessionState)
            {
                if (Context.User.Identity.IsAuthenticated)
                {
                    if (Session["SessionOwner"] == null)
                    {
                        Context.GetOwinContext().Authentication.SignOut();
                        Response.Redirect("/");
                    }
                    else
                    {
                        var sessionUserID = Session["SessionOwner"].ToString();
                        var authUserID = ((ClaimsIdentity)Context.User.Identity).GetUserId();

                        if (sessionUserID != authUserID)
                        {
                            Context.Session.Clear();
                            Context.Session.Abandon();
                            throw new Exception("You are an evil doer! No soup for you!");
                        }
                    }
                }
            }
        }

    }
}