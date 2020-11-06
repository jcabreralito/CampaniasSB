using System.Web;
using System.Web.Optimization;

namespace CampaniasLito
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                      "~/Scripts/moment.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));
                      //"~/Scripts/bootstrap-datetimepicker.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css"));

            bundles.Add(new StyleBundle("~/bundles/css").Include(
                      "~/Content/vendor/bootstrap/css/bootstrap.min.css",
                      //"~/Content/DataTables/datatables.min.css",
                      "~/Content/vendor/metisMenu/metisMenu.min.css",
                      "~/Content/dist/css/sb-admin-2.css"
                      //"~/Content/vendor/morrisjs/morris.css",
                      //"~/Content/vendor/font-awesome/css/all.min.css"
                      //"~/Content/bootstrap-datetimepicker.css"
                      //"~/Content/site.css"
                      ));

            bundles.Add(new StyleBundle("~/bundles/js").Include(
                      "~/Scripts/jquery-3.5.1.min.js",
                      //"~/Content/vendor/font-awesome/js/fontawesome.min.js",
                      "~/Content/vendor/bootstrap/js/bootstrap.min.js",
                      "~/Content/vendor/metisMenu/metisMenu.min.js",
                      "~/Content/vendor/raphael/raphael.min.js",
                      "~/Content/vendor/morrisjs/morris.min.js",
                      "~/Content/data/morris-data.js",
                      "~/Content/dist/js/sb-admin-2.js"
                      ));
            //"~/Scripts/gskpos.js",
            //"~/Scripts/respond.js",
            //"~/Scripts/moment.js",
            //"~/Scripts/bootstrap-datetimepicker.js",
            //"~/Scripts/fileupload.js"
        }
    }
}
