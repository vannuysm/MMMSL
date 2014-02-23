using System.Web.Optimization;

namespace Mmmsl.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterScripts(bundles);
            RegisterStylesheets(bundles);
        }

        private static void RegisterStylesheets(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/assets/js/modernizr").Include("~/assets/js/modernizr-*"));
            bundles.Add(new ScriptBundle("~/assets/js/vendor").Include(
                "~/assets/js/jquery-{version}.js",
                "~/assets/js/bootstrap.js",
                "~/assets/js/respond.js",
                "~/assets/js/retina-*"
            ));
        }

        private static void RegisterScripts(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/assets/css/vendor").Include("~/assets/css/bootstrap.css"));
            bundles.Add(new StyleBundle("~/assets/css/app").Include("~/assets/css/app-*"));
        }
    }
}
