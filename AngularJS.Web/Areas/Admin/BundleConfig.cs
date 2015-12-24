using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace AngularJS.Web.Areas.Admin
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/Admin/misc").Include(
                    "~/Scripts/misc/moment.js",
                    "~/Scripts/ng-misc/loading-bar/loading-bar.min.js",
                    "~/Scripts/ng-misc/angular-local-storage/angular-local-storage.min.js",
                    "~/Scripts/ng-misc/ui-bootstrap/ui-bootstrap-tpls-0.13.0.min.js",
                    "~/Scripts/ng-misc/ng-toast/ngToast.min.js",
                    "~/Scripts/ng-misc/angular-dialog-service/dialogs.min.js",
                    "~/Scripts/ng-misc/adapt-strap-2.3.0/adapt-strap.min.js",
                    "~/Scripts/ng-misc/adapt-strap-2.3.0/adapt-strap.tpl.min.js",
                    "~/Scripts/ng-misc/ui-select/select.min.js",
                    "~/Scripts/ng-misc/ui-select/select2.min.js",
                    "~/Scripts/ng-misc/angular-filter/angular-filter.min.js",
                    "~/Scripts/ng-misc/my-menu/my-menu.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Admin/angular").Include(
                    "~/Scripts/angular/angular.min.js",
                    "~/Scripts/angular/angular-route.min.js",
                    "~/Scripts/angular/angular-animate.min.js",
                    "~/Scripts/angular/angular-sanitize.min.js",
                    "~/Scripts/kendo/2015.1.408/kendo.angular.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Admin/app").Include(
                    "~/Areas/Admin/Scripts/app/app.js",
                    "~/Areas/Admin/Scripts/app/services/*.js",
                    "~/Areas/Admin/Scripts/app/services/models/*.js",
                    "~/Areas/Admin/Scripts/app/directives/*.js",
                    "~/Areas/Admin/Scripts/app/controllers/*.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Admin/kendo").Include(
                    "~/Scripts/kendo/2015.1.408/kendo.web.min.js",
                    "~/Scripts/kendo/2015.1.408/kendo.data.odata.min.js"
                ));

            bundles.Add(new StyleBundle("~/Content/Admin/css").Include(
                    "~/Content/misc/bootstrap/css/bootstrap.min.css",
                    "~/Content/site.css",
                    "~/Scripts/ng-misc/loading-bar/loading-bar.min.css",
                    "~/Scripts/ng-misc/ng-toast/ngToast.min.css",
                    "~/Scripts/ng-misc/angular-dialog-service/dialogs.min.css",
                    "~/Scripts/ng-misc/adapt-strap-2.3.0/adapt-strap.min.css",
                    "~/Scripts/ng-misc/ui-select/select.min.css",
                    "~/Scripts/ng-misc/my-menu/*.css",
                    "~/Content/kendo/2015.1.408/kendo.common.min.css",
                    "~/Content/kendo/2015.1.408/kendo.silver.min.css"
                ));
        }
    }
}