using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(eToolsWebsite.Startup))]
namespace eToolsWebsite
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
