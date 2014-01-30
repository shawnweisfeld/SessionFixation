using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SessionFixation.Startup))]
namespace SessionFixation
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
