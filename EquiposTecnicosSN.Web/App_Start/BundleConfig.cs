using EquiposTecnicosSN.Web.Wrappers;
using System.Web.Optimization;

namespace EquiposTecnicosSN.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = false;

            bundles.Add(new ScriptBundle("~/bundles/nqn-js").Include(
                        "~/Scripts/layout.js",
                        "~/Scripts/equipos-tecnicos-nqn.js",
                        "~/Scripts/charts-indicadores.js"));

            bundles.Add(new ScriptBundle("~/bundles/zing-charts-js").Include(
                        "~/Scripts/zingchart.min.js",
                        "~/Scripts/zingmodules/zingchart-pareto.min.js",
                        "~/Scripts/zingmodules/zingchart-grid.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui-js").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-js").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/datetimepicker-js").Include(
                        "~/Scripts/moment*",
                        "~/Scripts/bootstrap-datetimepicker*",
                        "~/Scripts/es.js",
                        "~/Scripts/date-picker-ready.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval-js").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-js").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/bootstrap-switch.js",
                      "~/Scripts/bootstrap-confirmation.min.js",
                      "~/Scripts/bootstrap-filestyle.js"));

            bundles.Add(new ScriptBundle("~/bundles/bower-components-js").Include(
                "~/Content/bower_components/metisMenu/dist/metisMenu.js",
                "~/Content/bower_components/datatables/media/js/jquery.dataTables.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr-js").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/pdfmake-js").Include( 
                "~/Scripts/custom/pdf/pdfmake.min.js", 
                "~/Scripts/custom/pdf/vfs_fonts.js"));

            bundles.Add(new ScriptBundle("~/bundles/qrcode-js").Include("~/Scripts/custom/qrcode/qrcode*"));


            bundles.Add(new StyleBundle("~/bundles/nqn-css").Include(
                "~/Content/timeline.css",
                "~/Content/bootstrap*",
                "~/Content/bootstrap-datetimepicker*",
                "~/Content/site.css",
                "~/Content/themes/base/all.css", 
                "~/Content/bootstrap-switch.css",
                "~/Content/sb-admin-2.css"));

            bundles.Add(new StyleBundle("~/bundles/bower-components-css").Include(
                "~/Content/bower_components/font-awesome/css/font-awesome.css",
                "~/Content/bower_components/metisMenu/dist/metisMenu.css"));


        }
    }
}
