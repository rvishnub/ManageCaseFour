using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ManageCaseFour.Startup))]
namespace ManageCaseFour
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
