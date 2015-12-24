using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Routing;
using AngularJS.Web.Helpers;
using Newtonsoft.Json.Serialization;
using NamespaceControllerSelectorSample;

namespace AngularJS.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Services.Replace(typeof(IHttpControllerSelector),
                new AreaHttpControllerSelector(config));
                // new NamespaceHttpControllerSelector(config));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApiGet",
                routeTemplate: "api/{controller}/{action}/{regionId}",
                defaults: new { action = "Get" },
                constraints: new { httpMethod = new HttpMethodConstraint("GET") }
            );

            //// For erea api
            //config.Routes.MapHttpRoute(
            //    name: "AreaApi",
            //    routeTemplate: "api/{area}/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            // Auto convert JSON format
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
