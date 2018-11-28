using System.Web;
using System.Web.Optimization;

namespace DMS
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            "~/js/jquery-1.12.4.js"));
            bundles.Add(new ScriptBundle("~/bundles/datatable").Include(
               
                "~/Assets/Datatables/jquery.dataTables.js"));
            bundles.Add(new ScriptBundle("~/bundles/sweet").Include(
                "~/Scripts/swal/sweetalert.js",
                "~/Scripts/swal/sweetalert.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/export").Include(
                  "~/Assets/export/js/datatables.min.js",
                  "~/Assets/export/js/dataTables.buttons.min.js",
                "~/Assets/export/js/buttons.flash.min.js",
                "~/Assets/export/js/buttons.html5.min.js",
                "~/Assets/export/js/buttons.print.min.js",
                "~/Assets/export/js/pdfmake.min.js",
                "~/Assets/export/js/vfs_fonts.js"));
            bundles.Add(new StyleBundle("~/bundles/datatablecss").Include(
                "~/Assets/Datatables/jquery.dataTables.min.css"));

            bundles.Add(new StyleBundle("~/bundles/sweetcss").Include(
                "~/css/sweetalert.css"));
            bundles.Add(new StyleBundle("~/bundles/exportcss").Include(
                "~/Assets/export/css/buttons.dataTables.min.css",
                "~/Assets/export/css/datatables.min.css"));

           BundleTable.EnableOptimizations = true;

        }
    }
}