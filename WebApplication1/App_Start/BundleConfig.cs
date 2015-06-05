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

            bundles.Add(new ScriptBundle("~/bundles/customScripts").Include(
                        "~/Scripts/moment.js",
                        "~/Scripts/moment.with-locales.js",
                        "~/Scripts/sweet-alert.js",
                        "~/Scripts/mustache.js",
                        "~/Scripts/AppScripts/Subasta.js",
                        "~/Scripts/star-rating.min.js"
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

                      "~/Content/sections/producto/InfoProducto.css",
                      "~/Content/sections/usuario/PerfilUsuario.css",
                      "~/Content/bootstrap.css",
                      "~/Content/normalize.css",
                      "~/Content/normalize.min.css",
                      "~/Content/star-rating.min.css",
                      "~/Content/partials/comentarios.css",
                      "~/Content/jquery-ui.css",
                      "~/Content/jquery.ui.chatbox.css",
                      "~/Content/animate.css",
                      "~/Content/font-awesome.css",
                      "~/Content/sweet-alert.css"));

            bundles.Add(new StyleBundle("~/Content/personalizacion/EstiloDos/css").Include(
                "~/Content/personalizacion/EstiloDos/ie8.css",
                "~/Content/personalizacion/EstiloDos/main.css",
                "~/Content/personalizacion/EstiloDos/product_item2.css",
                "~/Content/personalizacion/EstiloDos/fixes.css"
            ));
        }
    }
}
