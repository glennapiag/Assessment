using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Assessment.Startup))]
namespace Assessment
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
