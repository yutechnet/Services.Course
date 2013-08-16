using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Routing;
using Newtonsoft.Json;
using ServiceStack.Common.Web;

namespace BpeProducts.Services.Course.Host
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			config.EnableQuerySupport();

		    config.Routes.MapHttpRoute("CoursePrerequisteEdits", "course/{id}/prerequisites",
		                               defaults: new {controller = "CoursePrerequisite"});

            // Had to send to a separate controller because I couldn't figure out how to allow two PUT methods
            // with a similar function signature
            // http://stackoverflow.com/a/13128870
		    config.Routes.MapHttpRoute("CoursePublish", "{entityType}/{id}/publish",
		                               new {controller = "Version", action = "PublishVersion"});

		    config.Routes.MapHttpRoute("CourseVersion", "{entityType}/version",
		                               new {controller = "Version", action = "CreateVersion"});

            config.Routes.MapHttpRoute("CourseLearningActivity", "course/{courseId}/segments/{segmentId}/learningactivity/{id}",
                                      new { controller = "courselearningactivity", id = RouteParameter.Optional});

            config.Routes.MapHttpRoute("CourseSegmentOutcome", "course/{courseId}/segments/{segmentId}/supports/{Id}",
                                       new { controller = "outcome", id = RouteParameter.Optional, action = "CourseSegmentOutcome" });

		    config.Routes.MapHttpRoute("CourseSegmentOutcome2", "outcome/entityoutcomes",
		                               new {controller = "outcome", action = "GetEntityOutcomes"},
                                       constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) });

            //TODO: Combine the outcome routes into 1
            config.Routes.MapHttpRoute("OutcomeSupportsOutcome1", "outcome/{supportingOutcomeId}/supports/{supportedOutcomeId}",
                                       new { controller = "outcome", action = "AddSupportingOutcome" },
                                       constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Put) });

            config.Routes.MapHttpRoute("OutcomeSupportsOutcome2", "outcome/{supportingOutcomeId}/supports",
                                       new { controller = "outcome", action = "GetSupportingOutcomes" },
                                       constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) });

            config.Routes.MapHttpRoute("OutcomeSupportsOutcome3", "outcome/{supportingOutcomeId}/supports/{supportedOutcomeId}",
                                       new { controller = "outcome", action = "RemoveSupportingOutcome" },
                                       constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Delete) });

            config.Routes.MapHttpRoute("OutcomeApi", "{entityType}/{entityId}/supports/{outcomeId}",
                                       new { controller = "outcome", outcomeId = RouteParameter.Optional });

			config.Routes.MapHttpRoute("CourseSegments", "course/{courseId}/segments/{segmentId}",
                                       new { controller = "segment", segmentId = RouteParameter.Optional });

		    config.Routes.MapHttpRoute("DefaultApi", "{controller}/{id}",
		                               defaults: new {id = RouteParameter.Optional});

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
