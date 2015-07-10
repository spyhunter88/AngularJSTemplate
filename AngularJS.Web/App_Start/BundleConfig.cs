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
                        "~/Scripts/jquery/jquery-2.1.0.js",
                        "~/Scripts/jquery/jquery.plugin.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/misc/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      // "~/Scripts/misc/bootstrap.js",
                      "~/Scripts/misc/respond.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                    "~/Scripts/angular/angular.min.js",
                    "~/Scripts/angular/angular-route.min.js",
                    "~/Scripts/angular/angular-animate.min.js",
                    "~/Scripts/angular/angular-sanitize.min.js",
                    "~/Scripts/kendo/2015.1.408/kendo.angular.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/misc").Include(
                    "~/Scripts/misc/moment.js",
                    "~/Scripts/ng-misc/loading-bar/loading-bar.min.js",
                    "~/Scripts/ng-misc/angular-local-storage/angular-local-storage.min.js",
                    "~/Scripts/ng-misc/ui-bootstrap/ui-bootstrap-tpls-0.13.0.min.js",
                    "~/Scripts/ng-misc/ng-toast/ngToast.min.js",
                    "~/Scripts/ng-misc/angular-dialog-service/dialogs.min.js",
                    "~/Scripts/ng-misc/angular-filter/angular-filter.min.js",
                    "~/Scripts/ng-misc/ng-file-upload/ng-file-upload-shim.min.js",
                    "~/Scripts/ng-misc/ng-file-upload/ng-file-upload.min.js",
                    "~/Scripts/ng-misc/ui-select/select.min.js",
                    "~/Scripts/ng-misc/my-menu/my-menu.js"
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

            //bundles.Add(new ScriptBundle("~bundles/Admin/app").Include(
            //        "~/Areas/Admin/Scripts/app/app.js",
            //        "~/Areas/Admin/Scripts/app/services/*.js",
            //        "~/Areas/Admin/Scripts/app/services/models/*.js",
            //        "~/Areas/Admin/Scripts/app/controllers/*.js"
            //    ));

            bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
                    "~/Scripts/kendo/2015.1.408/kendo.web.min.js",
                    "~/Scripts/kendo/2015.1.408/kendo.data.odata.min.js"
                ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/misc/bootstrap/css/bootstrap.min.css",
                      "~/Content/site.css",
                      "~/Scripts/ng-misc/loading-bar/loading-bar.min.css",
                      "~/Scripts/ng-misc/ng-toast/ngToast.min.css",
                      "~/Scripts/ng-misc/angular-dialog-service/dialogs.min.css",
                      "~/Scripts/ng-misc/ui-select/select.min.css",
                      "~/Scripts/ng-misc/my-menu/*.css",
                      "~/Content/kendo/2015.1.408/kendo.common.min.css",
                      "~/Content/kendo/2015.1.408/kendo.silver.min.css"
                ));
        }
    }
}
