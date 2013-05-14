using System.Web.Http;
using BpeProducts.Services.Course.Host.Controllers;
using Newtonsoft.Json;

namespace BpeProducts.Services.Course.Host
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
			config.EnableQuerySupport();

            config.Routes.MapHttpRoute("CourseSegmentsApi2", "{controller}/{courseId}/{action}");

            config.Routes.MapHttpRoute("CourseSegmentsApi", "{controller}/{courseId}/{action}/{segmentId}", 
                new
                {
                   segmentId = RouteParameter.Optional
                }
            );

            config.Routes.MapHttpRoute("CourseSegmentsApi3", "{controller}/{courseId}/segments/{segmentId}/segments",
                new { action = "SubSegments" }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
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
