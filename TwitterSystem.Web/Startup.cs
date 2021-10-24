using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TwitterSystem.Web.Startup))]
namespace TwitterSystem.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
