﻿using System;
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

        // Consider forcing the entire website to only accept HTTPS
        // see: http://tools.ietf.org/html/rfc6797
        // 
        // This can be done in the web.config with the following XML
        //<system.webServer> 
        //  <httpProtocol> 
        //      <customHeaders> 
        //          <add name="Strict-Transport-Security" value="max-age=10886400"/> 
        //      </customHeaders> 
        //< /httpProtocol> 
        //</system.webServer>

        // Consider only allowing cookies when the user is using SSL
        // <httpCookies httpOnlyCookies="true" requireSSL="true" domain="" />
        //
        // for more information:
        // http://msdn.microsoft.com/en-us/library/ms228262(v=vs.85).aspx


        void Application_PreRequestHandlerExecute(Object sender, EventArgs e)
        {
            //is this a request that has session?
            if (Context.Session != null)
            {
                //is this an authenticated request?
                if (Context.User.Identity.IsAuthenticated)
                {
                    if (Context.Session["SessionOwner"] == null)
                    {
                        //if the session owner is missing from the session 
                        //require the user to log back in
                        IdentityHelper.SignOut();
                    }
                    else
                    {
                        var authUserID = ((ClaimsIdentity)Context.User.Identity).GetUserId();
                        var sessionUserID = Context.Session["SessionOwner"].ToString();

                        if (sessionUserID != authUserID)
                        {
                            //if the session owner is not the person that is logged in
                            //require the user to log back in
                            IdentityHelper.SignOut();
                        }
                    }
                }
                else
                {
                    //Don't store anything in the session for unauthenticated users
                    Context.Session.Clear();
                }
            }
        }
    }
}