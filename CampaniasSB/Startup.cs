using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CampaniasSB.Startup))]
namespace CampaniasSB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
