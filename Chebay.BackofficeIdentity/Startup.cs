using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Chebay.BackofficeIdentity.Startup))]
namespace Chebay.BackofficeIdentity
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
