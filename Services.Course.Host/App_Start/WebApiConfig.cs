﻿using System.Net.Http;
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

            // Had to send to a separate controller because I couldn't figure out how to allow two PUT methods
            // with a similar function signature
            // http://stackoverflow.com/a/13128870
            config.Routes.MapHttpRoute(
                "CoursePublish",
                "{entityType}/{id}/publish",
                new { controller = "Version", action = "PublishVersion" });

		    config.Routes.MapHttpRoute(
		        "CourseVersion",
		        "{entityType}/version",
                new { controller = "Version", action = "CreateVersion" });

            config.Routes.MapHttpRoute("CourseSegmentOutcome", "course/{courseId}/segments/{segmentId}/outcome/{id}",
                                       new { controller = "outcome", id = RouteParameter.Optional, action = "CourseSegmentOutcome" },
                                       constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Post) });

            // a program, course, segment supports? satisfies? fullfills? meets?... Go to doamin expert and get proper term in the ubiquitous language
            config.Routes.MapHttpRoute("OutcomeApi", "{entityType}/{entityId}/ToBeDetermined/{outcomeId}",
                                       new { controller = "outcome", outcomeId = RouteParameter.Optional });

            config.Routes.MapHttpRoute("OutcomeSupportsOutcome1", "outcome/{supportingOutcomeId}/supports/{supportedOutcomeId}",
                                       new { controller = "outcome", action = "AddSupportingOutcome" },
                                       constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Put) });

            config.Routes.MapHttpRoute("OutcomeSupportsOutcome2", "outcome/{supportingOutcomeId}/supports",
                                       new { controller = "outcome", action = "GetSupportingOutcomes" },
                                       constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) });

            config.Routes.MapHttpRoute("OutcomeSupportsOutcome3", "outcome/{supportingOutcomeId}/supports/{supportedOutcomeId}",
                                       new { controller = "outcome", action = "RemoveSupportingOutcome" },
                                       constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Delete) });

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