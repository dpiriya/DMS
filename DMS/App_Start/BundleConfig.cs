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

            bundles.Add(new StyleBundle("~/bundles/datatablecss").Include(
                "~/Assets/Datatables/jquery.dataTables.min.css"));

            bundles.Add(new StyleBundle("~/bundles/sweetcss").Include(
                "~/css/sweetalert.css"));
            BundleTable.EnableOptimizations = true;

        }
    }
}