using System.Web.Http;

namespace BpeProducts.Services.Course.Host
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
			config.EnableQuerySupport();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new {id = RouteParameter.Optional}
                );

            config.EnableSystemDiagnosticsTracing();
        }
    }
}
