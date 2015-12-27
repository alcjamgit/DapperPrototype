using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DapperMapper.Startup))]
namespace DapperMapper
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
