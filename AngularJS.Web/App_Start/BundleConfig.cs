using System.Web;
using System.Web.Optimization;

namespace AngularJS.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery/jquery-1.10.2.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/misc/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/misc/bootstrap.js",
                      "~/Scripts/misc/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                    "~/Scripts/angular/angular.js",
                    "~/Scripts/angular/angular-route.js",
                    "~/Scripts/angular/angular-animate.js",
                    "~/Scripts/kendo/2014.2.716/kendo.angular.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/misc").Include(
                    "~/Scripts/misc/loading-bar.min.js",
                    "~/Scripts/misc/angular-local-storage.min.js",
                    "~/Scripts/misc/ui-bootstrap-tpls-0.12.0.min.js",
                    "~/Scripts/misc/xeditable.min.js",
                    "~/Scripts/misc/angular-datatables.min.js"
                ));

            // This's still bug
            // bundles.Add(new ScriptBundle("~/bundles/app").IncludeDirectory("~/Scripts/app", "*.js", true));
            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                    "~/Scripts/app/app.js",
                    "~/Scripts/app/services/*.js",
                    "~/Scripts/app/services/models/*.js",
                    "~/Scripts/app/controllers/*.js",
                    "~/Scripts/app/directives/*.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
                    "~/Scripts/kendo/2014.2.716/kendo.web.min.js",
                    "~/Scripts/kendo/2014.2.716/kendo.data.odata.min.js"
                ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/misc/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/misc/loading-bar.css",
                      "~/Content/misc/xeditable.css",
                      "~/Content/misc/datatables.bootstrap.min.css",
                      "~/Content/kendo/2014.2.716/kendo.common.min.css",
                      "~/Content/kendo/2014.2.716/kendo.silver.min.css"
                      ));
        }
    }
}
