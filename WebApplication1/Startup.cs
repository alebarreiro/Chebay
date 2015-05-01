using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Frontoffice.Startup))]
namespace Frontoffice
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
