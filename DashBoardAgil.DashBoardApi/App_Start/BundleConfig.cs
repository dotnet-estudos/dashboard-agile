using System.Web;
using System.Web.Optimization;

namespace DashBoardAgil.DashBoardApi
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                     // "~/Content/bootstrap.css",
                      "~/Content/site.css"));


            bundles.Add(new ScriptBundle("~/bundles/bootstrap_template").Include(
                     "~/Scripts/js/jquery.min.js",
                     "~/Scripts/js/bootstrap.min.js",
                     "~/Scripts/js/metisMenu.min.js",
                     "~/Scripts/startmin.js"));

            bundles.Add(new ScriptBundle("~/bundles/Dash").Include(
                   "~/Scripts/sweetalert2.min.js",
                   "~/Scripts/DashBoard.js"));

            bundles.Add(new StyleBundle("~/Content/sweetalert2").Include(
                     "~/Content/sweetalert2.min.css"));

            bundles.Add(new StyleBundle("~/Content/css_template").Include(
                      "~/Content/css/bootstrap.min.css",
                      "~/Content/css/metisMenu.min.css",
                      "~/Content/css/timeline.min.css",
                      "~/Content/css/startmin.min.css",
                      "~/Content/css/font-awesome.min.css",
                      "~/Content/sweetalert2.min.css",
                      "~/Content/site.css"));


        }
    }
}
