using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AngularJS.Services;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AngularJS.Web.Startup))]
namespace AngularJS.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            AutoMapperConfiguration.Configure();

            Debug.AutoFlush = true;
        }
    }
}
