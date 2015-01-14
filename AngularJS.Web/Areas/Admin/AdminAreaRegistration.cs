using System.Web.Mvc;

namespace AngularJS.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "AngularJS.Web.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "AdminApi_default",
                "Admin/api/{controller}/{action}/{id}",
                new {  id = UrlParameter.Optional },
                namespaces: new[] { "AngularJS.Web.Areas.Admin.Controllers" }
            );
        }
    }
}