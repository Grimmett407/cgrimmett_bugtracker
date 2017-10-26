using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(cgrimmett_bugtracker.Startup))]
namespace cgrimmett_bugtracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
