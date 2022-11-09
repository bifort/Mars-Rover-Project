using System.Web.Optimization;

namespace MarsRoverProject.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));
            
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            // need to combine the use of ScriptBundle and Bundle
            // as ScriptBundle is using outdated JsMinifier which doesn't support new JS language features (e.g arrow functions)
            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                      "~/Scripts/bootstrap.js"                      
                      ));
            bundles.Add(new Bundle("~/bundles/js").Include(                      
                      "~/Scripts/mars.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}
