using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Quizbee.Startup))]
namespace Quizbee
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
