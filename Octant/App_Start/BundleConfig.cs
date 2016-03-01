using System.Web.Optimization;
using System;
using System.Configuration;

namespace IdentitySample
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;
            var version = System.Reflection.Assembly.GetAssembly(typeof(Controllers.HomeController)).GetName().Version.ToString();
            var cdnUrl = "http://az795632.vo.msecnd.net/{0}?v=" + version;

            bundles.Add(new ScriptBundle("~/bundles/jquery", string.Format(cdnUrl, "bundles/jquery")).Include(
                        "~/Scripts/jquery-1.10.2.js",
                        "~/Scripts/jquery-ui-1.10.4.min.js",
                        "~/Scripts/jquery.easing.1.3.min.js",
                        "~/Scripts/jsrender.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval", string.Format(cdnUrl, "bundles/jqueryval")).Include(
                        "~/Scripts/jquery.validate*"/*,
                        "~/Scripts/DateValidator.js"*/));

            bundles.Add(new ScriptBundle("~/bundles/angular", string.Format(cdnUrl, "bundles/angular")).Include(
                        "~/Scripts/angular.min.js",
                        "~/Content/fl/js/ngSlimscroll.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr", string.Format(cdnUrl, "bundles/modernizr")).Include(
                        "~/Scripts/modernizr-2.6.2.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap", string.Format(cdnUrl, "bundles/bootstrap")).Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/bootstrap-multiselect.js"));

            bundles.Add(new StyleBundle("~/Content/css", string.Format(cdnUrl, "Content/css")).Include(
                      "~/Content/bootstrap-fl.min.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/bootstrap-multiselect.css",
                      "~/Content/bootstrap-switch.min.css",
                      "~/Content/goalProgress.fl.css",
                      "~/Content/jquery.circliful.css",
                      "~/Content/jquery-ui-1.10.4.min.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/Scripts/js", string.Format(cdnUrl, "Scripts/js")).Include());

            bundles.Add(new StyleBundle("~/Content/Home", string.Format(cdnUrl, "Content/Home")).Include(
                      "~/Content/Home.css"));

            bundles.Add(new ScriptBundle("~/Scripts/ConductInterview", string.Format(cdnUrl, "Scripts/ConductInterview")).Include(
                        "~/Scripts/bootstrap-switch.min.js",
                        "~/Scripts/angular-bootstrap-switch.min.js",
                        "~/Scripts/ConductInterview.js"));

            bundles.Add(new ScriptBundle("~/Scripts/ConductInterview/Saved", string.Format(cdnUrl, "Scripts/ConductInterview/Saved")).Include(
                        "~/Scripts/goalProgress.fl.js",
                        "~/Scripts/jquery.circliful.fl.js"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css", string.Format(cdnUrl, "Content/themes/base/css")).Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));






            bundles.Add(new StyleBundle("~/FL/css", string.Format(cdnUrl, "FL/css")).Include(
                "~/Content/fl/css/bootstrap.min.css",
                "~/Content/fl/css/bootstrap-multiselect.css",
                "~/Content/fl/css/bootstrap-datepicker.min.css",
                "~/Content/fl/css/font-awesome.min.css",
                "~/Content/fl/css/ionicons.min.css",
                "~/Content/fl/css/select2.min.css",
                "~/Content/fl/css/fullcalendar.min.css",
                "~/Content/fl/css/alte.min.css",
                "~/Content/fl/css/alte-skin-blue.min.css",
                "~/Content/fl/css/main.css",
                "~/Content/ej/web/default-theme/ej.web.all.min.css"));

            bundles.Add(new ScriptBundle("~/FL/js", string.Format(cdnUrl, "FL/js")).Include(
                /*"~/Content/fl/js/jquery.min.js",*/
                "~/Content/fl/js/bootstrap.min.js",
                "~/Content/fl/js/bootstrap-multiselect.js",
                "~/Content/fl/js/bootstrap-datepicker.min.js",
                "~/Content/fl/js/fastclick.min.js",
                "~/Content/fl/js/select2.min.js",
                "~/Content/fl/js/jquery.knob.min.js",
                "~/Content/fl/js/alte.min.js",
                "~/Content/fl/js/main.js"));


            bundles.Add(new StyleBundle("~/FL/pages/conductinterview/saved", string.Format(cdnUrl, "FL/pages/conductinterview/saved")).Include(
                "~/Content/fl/css/pages/conductinterview-saved.css"));

            bundles.Add(new StyleBundle("~/FL/pages/conductinterview/index", string.Format(cdnUrl, "FL/pages/conductinterview/index")).Include(
                "~/Content/fl/css/pages/conductinterview-index.css"));

            bundles.Add(new StyleBundle("~/FL/pages/campaigns/saved", string.Format(cdnUrl, "FL/pages/campaigns/saved")).Include(
                "~/Content/fl/css/pages/campaigns-saved.css"));

            bundles.Add(new StyleBundle("~/FL/pages/campaigns/pdf", string.Format(cdnUrl, "FL/pages/campaigns/pdf")).Include(
                "~/Content/fl/css/pages/campaigns-pdf.css"));


            bundles.Add(new StyleBundle("~/FL/css/calendar", string.Format(cdnUrl, "FL/css/calendar")).Include(
                "~/Content/fl/css/fullcalendar.min.css"));

            bundles.Add(new ScriptBundle("~/FL/js/calendar", string.Format(cdnUrl, "FL/js/calendar")).Include(
                "~/Content/fl/js/moment.min.js",
                "~/Content/fl/js/fullcalendar.min.js"));

            bundles.Add(new ScriptBundle("~/Scripts/ej", string.Format(cdnUrl, "Scripts/ej")).Include(
                "~/Scripts/ej/ej.web.all.min.js",
                "~/Scripts/ej/ej.unobtrusive.min.js",
                "~/Scripts/jquery.globalize.min.js"));

            bundles.IgnoreList.Clear();
        }
    }
}
