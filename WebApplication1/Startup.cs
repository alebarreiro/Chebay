using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Frontoffice.Startup))]
namespace Frontoffice
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //mapeando los hubs a redis
            GlobalHost.DependencyResolver.UseRedis("chebaylinux.cloudapp.net", 6379, string.Empty, "chebay");
            app.MapSignalR();
            ConfigureAuth(app);
        }
    }
}
