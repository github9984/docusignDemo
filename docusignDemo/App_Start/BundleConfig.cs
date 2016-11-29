using System.Web.Optimization;

namespace docusignDemo
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Javascripts
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.validate*","~/Scripts/jquery.unobtrusive*"));
            bundles.Add(new ScriptBundle("~/bundles/knockout").Include("~/Scripts/knockout*"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap-switch").Include("~/Scripts/bootstrap-switch.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/datatables").Include("~/Scripts/datatables.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.js","~/Scripts/respond.js"));
            bundles.Add(new ScriptBundle("~/bundles/swal").Include("~/Scripts/Sweetalert/sweetalert.min.js"));


            // CSS 
            bundles.Add(new StyleBundle("~/Content/site").Include("~/Content/Styles/site.min.css"));
            bundles.Add(new StyleBundle("~/Content/bootstrap-switch").Include("~/Content/bootstrap-switch.min.css"));
            bundles.Add(new StyleBundle("~/Content/datatables").Include("~/Content/datatables.min.css"));
            bundles.Add(new StyleBundle("~/Content/fa").Include("~/Content/font-awesome.min.css"));
            bundles.Add(new StyleBundle("~/Content/loginscreen").Include("~/Content/Styles/login.css"));
            bundles.Add(new StyleBundle("~/Content/swal").Include("~/Content/Sweetalert/sweetalert.css"));

        }
    }
}