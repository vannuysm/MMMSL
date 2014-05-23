using System.Web;
using System.Web.Optimization;

namespace Mmmsl.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles) {
            bundles.Add(new ScriptBundle("~/assets/js/modernizr").Include("~/assets/js/modernizr-*"));
            bundles.Add(new ScriptBundle("~/assets/js/vendor").Include(
                "~/assets/js/jquery-{version}.js",
                "~/assets/js/underscore.js",
                "~/assets/js/moment.js",
                "~/assets/js/bootstrap.js",
                "~/assets/js/bootstrap-datetimepicker.js",
                "~/assets/js/respond.js",
                "~/assets/js/retina-*",
                "~/assets/js/angular.js",
                "~/assets/js/angular-*",
                "~/assets/js/ui-bootstrap-*"
            ));

            bundles.Add(new ScriptBundle("~/app/c").IncludeDirectory("~/app/controllers", "*.js", true));
            bundles.Add(new ScriptBundle("~/app/a").Include("~/app/app.js"));

            bundles.Add(new StyleBundle("~/assets/css/vendor")
                .Include("~/assets/css/bootstrap.css")
                .Include("~/assets/css/bootstrap-datetimepicker.css")
                .Include("~/assets/css/animate.css")
                .Include("~/assets/css/font-awesome.css")
            );
            bundles.Add(new StyleBundle("~/assets/css/app").Include("~/assets/css/app-*"));
        }
    }
}
