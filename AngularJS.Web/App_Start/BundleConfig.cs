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
                        "~/Scripts/jquery/jquery-2.1.0.js"));

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
                    "~/Scripts/angular/angular.min.js",
                    "~/Scripts/angular/angular-route.min.js",
                    "~/Scripts/angular/angular-animate.min.js",
                    "~/Scripts/angular/angular-sanitize.min.js",
                    "~/Scripts/kendo/2015.1.408/kendo.angular.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/misc").Include(
                    "~/Scripts/misc/loading-bar.min.js",
                    "~/Scripts/misc/angular-local-storage.min.js",
                    "~/Scripts/misc/ui-bootstrap-tpls-0.13.0.min.js",
                    "~/Scripts/misc/ngToast.min.js",
                    "~/Scripts/misc/dialogs.min.js",
                    "~/Scripts/misc/moment.js",
                    //"~/Scripts/misc/select.min.js",
                    //"~/Scripts/misc/angular-strap.min.js",
                    //"~/Scripts/misc/angular-strap.tpl.min.js",
                    "~/Scripts/misc/xeditable.min.js",
                    "~/Scripts/misc/angular-filter.min.js",
                    "~/Scripts/datatables/js/jquery.dataTables.min.js",
                    "~/Scripts/datatables/angular-datatables.min.js",
                    "~/Scripts/datatables/plugins/bootstrap/angular-datatables.bootstrap.min.js",
                    "~/Scripts/datatables/extensions/Scroller/js/dataTables.scroller.min.js",
                    "~/Scripts/datatables/plugins/scroller/angular-datatables.scroller.min.js",
                    "~/Scripts/misc/angular-file-upload-shim.min.js",
					"~/Scripts/misc/angular-file-upload.min.js"
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
                      "~/Content/misc/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/misc/loading-bar.css",
                      "~/Content/misc/ngToast.min.css",
                      "~/Content/misc/dialogs.min.css",
                      // "~/Content/misc/ngDialog-theme-default.min.css",
                      //"~/Content/misc/select.min.css",
                      // "~/Content/misc/angular-strap.min.css",
                      "~/Content/misc/xeditable.css",
                      "~/Scripts/datatables/css/jquery.dataTables.min.css",
                      "~/Scripts/datatables/extensions/Scroller/css/dataTables.scroller.min.css",
                      "~/Scripts/datatables/plugins/bootstrap/datatables.bootstrap.min.css",
                      "~/Content/kendo/2015.1.408/kendo.common.min.css",
                      "~/Content/kendo/2015.1.408/kendo.silver.min.css"
                ));
        }
    }
}
