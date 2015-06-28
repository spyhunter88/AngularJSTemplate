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
                    "~/Scripts/misc/loading-bar.min.js",
                    "~/Scripts/misc/angular-local-storage.min.js",
                    "~/Scripts/misc/ui-bootstrap-tpls-0.13.0.min.js",
                    "~/Scripts/misc/ngToast.min.js",
                    "~/Scripts/misc/dialogs.min.js",
                    "~/Scripts/misc/adapt-strap-2.3.0/adapt-strap.min.js",
                    "~/Scripts/misc/adapt-strap-2.3.0/adapt-strap.tpl.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Admin/angular").Include(
                    "~/Scripts/angular/angular.min.js",
                    "~/Scripts/angular/angular-route.min.js",
                    "~/Scripts/angular/angular-animate.min.js",
                    "~/Scripts/angular/angular-sanitize.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Admin/app").Include(
                    "~/Areas/Admin/Scripts/app/app.js",
                    "~/Areas/Admin/Scripts/app/services/*.js",
                    "~/Areas/Admin/Scripts/app/services/models/*.js",
                    "~/Areas/Admin/Scripts/app/controllers/*.js"
                ));

            bundles.Add(new StyleBundle("~/Content/Admin/css").Include(
                      "~/Content/misc/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/misc/loading-bar.css",
                      "~/Content/misc/ngToast.min.css",
                      "~/Content/misc/dialogs.min.css",
                      "~/Scripts/misc/adapt-strap-2.3.0/adapt-strap.min.css"
                ));
        }
    }
}