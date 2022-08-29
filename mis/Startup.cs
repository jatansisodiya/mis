using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using mis.Models;
using Owin;

[assembly: OwinStartupAttribute(typeof(mis.Startup))]
namespace mis
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }
    }
}
