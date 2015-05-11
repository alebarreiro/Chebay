using System.Web;
using System.Web.Optimization;

namespace Frontoffice
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui-chat").Include(
                       "~/Scripts/jquery-ui.js",
                       "~/Scripts/jquery.ui.chatbox.js"
                       ));

            bundles.Add(new ScriptBundle("~/bundles/signalR").Include(
                       "~/Scripts/jquery.signalR-2.1.2.js"
                       ));

            bundles.Add(new ScriptBundle("~/bundles/Chat").Include(
                       "~/Scripts/Chat.js"
                       ));

            bundles.Add(new ScriptBundle("~/bundles/customScripts").Include(
                        "~/Scripts/moment.js",
                        "~/Scripts/moment.with-locales.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/Subasta").Include(
                       "~/Scripts/Subasta.js"
                       ));

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
                      "~/Content/bootstrap.css",
                      "~/Content/sections/main/main.css",
                      "~/Content/normalize.css",
                      "~/Content/normalize.min.css",
                      "~/Content/partials/product_item.css",
                      "~/Content/jquery-ui.css",
                      "~/Content/jquery.ui.chatbox.css",
                      "~/Content/animate.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/personalizacion/EstiloUno/css").Include(
                "~/Content/personalizacion/EstiloUno/orangeStyle.css"
            ));

            bundles.Add(new StyleBundle("~/Content/personalizacion/EstiloDos/css").Include(
                "~/Content/personalizacion/EstiloDos/violetStyle.css"
            ));


 
        }
    }
}
