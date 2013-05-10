using System.Web.Http;
using Newtonsoft.Json;

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

			//id this works move to common.webapi to let serializer handle self referencing objects
			var jsonSerializerSettings = new JsonSerializerSettings
			{
				PreserveReferencesHandling = PreserveReferencesHandling.Objects
			};
	        config.Formatters.JsonFormatter.SerializerSettings = jsonSerializerSettings;
		}
    }
}
