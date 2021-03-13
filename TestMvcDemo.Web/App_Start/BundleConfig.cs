using System.Web;
using System.Web.Optimization;

namespace TestMvcDemo.Web
{
    public class BundleConfig
    {
        // 有关捆绑的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/vue").Include(
"~/Content/vue/vue.min.js",
"~/Content/axios/axios.min.js",
"~/Content/elementui/index.js",
"~/Content/utils/utils.js"));

            bundles.Add(new ScriptBundle("~/mobile/vue").Include(
          "~/Content/vue/vue.min.js",
          "~/Content/axios/axios.min.js",
          "~/Content/vantui/vant.min.js",
          "~/Content/utils/utils.js"));

            bundles.Add(new StyleBundle("~/content/css").Include(
                "~/Content/avatar.css",
                "~/Content/list.css",
                "~/Content/elementui/index.css"));

            bundles.Add(new StyleBundle("~/mobile/css").Include(
                "~/Content/main.css",
               "~/Content/vantui/index.css"
               ));

            bundles.Add(new ScriptBundle("~/Scripts/loader").Include(
            "~/Content/http-vue-loader/http-vue-loader.js"));
        }
    }
}
