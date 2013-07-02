using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Routing;
using Newtonsoft.Json;

namespace BpeProducts.Services.Course.Host
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			config.EnableQuerySupport();

            // Had to send to a separate controller because I couldn't figure out how to allow two PUT methods
            // with a similar function signature
            // http://stackoverflow.com/a/13128870
            config.Routes.MapHttpRoute(
                "CoursePublish",
                "courses/{id}/publish",
                new { controller = "CourseVersion", action = "PublishVersion" });

		    config.Routes.MapHttpRoute(
		        "CourseVersion",
		        "courses/version",
                new { controller = "CourseVersion", action = "CreateVersion" });

            config.Routes.MapHttpRoute(
                "OutcomePublish",
                "outcome/{id}/publish",
                new { controller = "OutcomeVersion", action = "PublishVersion" });

            config.Routes.MapHttpRoute(
                "OutcomeVersion",
                "outcome/version",
                new { controller = "OutcomeVersion",  action = "CreateVersion" });

            config.Routes.MapHttpRoute("OutcomeApi", "{entityType}/{entityId}/outcome/{outcomeId}",
                                       new { controller = "outcome", outcomeId = RouteParameter.Optional });

            config.Routes.MapHttpRoute("CourseSegmentsApi2", "{controller}/{courseId}/{action}");

			config.Routes.MapHttpRoute("CourseSegmentsApi", "{controller}/{courseId}/{action}/{segmentId}",
			                           new
				                           {
					                           segmentId = RouteParameter.Optional
				                           }
				);

			config.Routes.MapHttpRoute("CourseSegmentsApi3", "{controller}/{courseId}/segments/{segmentId}/segments",
			                           new {action = "SubSegments"}
				);

            config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "{controller}/{id}",
				defaults: new {id = RouteParameter.Optional }
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