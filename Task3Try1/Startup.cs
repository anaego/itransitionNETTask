using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Task3Try1.Startup))]
namespace Task3Try1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
