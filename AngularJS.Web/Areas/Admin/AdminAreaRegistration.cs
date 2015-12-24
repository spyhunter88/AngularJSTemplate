﻿using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using AngularJS.Web.Extensions;
using System.Web.Http.OData.Builder;
using System.Web.Http.Dispatcher;
using AngularJS.Web.Helpers;
using System.Web.Http.OData.Routing.Conventions;
using System.Web.Http.OData.Routing;
using System.Collections.Generic;

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

            // Add WebAPI Route for Areas
            //context.Routes.MapHttpRoute(
            //    "AdminApi_default",
            //    "Admin/api/{controller}/{id}",
            //    defaults: new { area = "admin", id = RouteParameter.Optional }
            //);

            context.MapHttpRoute(
                "Admin_defaultApi",
                "api/admin/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //context.Routes.MapHttpRoute(
            //    "AdminApi_defaultGet",
            //    "Admin/api/{controller}/{action}/{id}",
            //    defaults: new { action = "Get" },
            //    constraints: new { httpMethod = new HttpMethodConstraint("GET") }
            //);

            // OData Register
            HttpConfiguration config = GlobalConfiguration.Configuration;
            
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Entities.Models.Category>(typeof(Entities.Models.Category).Name);
            var model = builder.GetEdmModel();

            IList<IODataRoutingConvention> routingConventions = ODataRoutingConventions.CreateDefault();
            routingConventions.Insert(0, new CustomControllerRoutingConvention("Admin"));
            config.Routes.MapODataRoute("Admin_odata", "odata/Admin", model);
                // new DefaultODataPathHandler(), routingConventions);

            config.EnableQuerySupport();

            // Add Admin Bundle
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}