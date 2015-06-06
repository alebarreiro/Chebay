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
            //GlobalHost.DependencyResolver.UseRedis("chebaylinux.cloudapp.net", 6379, string.Empty, "chebay");
            GlobalHost.DependencyResolver.UseRedis("pub-redis-16767.us-east-1-4.3.ec2.garantiadata.com", 16767, "xWoAE6AbeSLe0VOM", "chebay");
            app.MapSignalR();
            ConfigureAuth(app);
        }
    }
}
