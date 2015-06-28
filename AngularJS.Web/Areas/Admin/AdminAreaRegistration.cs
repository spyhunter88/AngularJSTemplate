﻿using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

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
            // Add MVC Route for Areas
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "AngularJS.Web.Areas.Admin.Controllers" }
            );

            // context.Routes.MapHttpAttributeRoutes();
            // Add WebAPI Route for Areas
            context.Routes.MapHttpRoute(
                "AdminApi_default",
                "api/Admin/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            //context.Routes.MapHttpRoute(
            //    "AdminApi_defaultGet",
            //    "Admin/api/{controller}/{action}/{id}",
            //    defaults: new { action = "Get" },
            //    constraints: new { httpMethod = new HttpMethodConstraint("GET") }
            //);

            // Add Admin Bundle
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}