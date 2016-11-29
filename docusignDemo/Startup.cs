using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(docusignDemo.Startup))]
namespace docusignDemo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
