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
            //config.EnableQuerySupport();

            //config.Routes.MapHttpRoute("Course", "course/{id}",
            //                           new {controller = "Course", courseId = RouteParameter.Optional});

            //config.Routes.MapHttpRoute("CoursePrerequisteEdits", "course/{id}/prerequisites",
            //                           new {controller = "CoursePrerequisite"});

            //config.Routes.MapHttpRoute("CoursePublish", "{entityType}/{id}/publish",
            //                           new { controller = "Version", action = "PublishVersion" });

            //config.Routes.MapHttpRoute("CourseVersion", "{entityType}/version",
            //                           new { controller = "Version", action = "CreateVersion" });

            //config.Routes.MapHttpRoute("CourseSegmentOutcome", "course/{courseId}/segments/{segmentId}/supports/{Id}",
            //                           new { controller = "outcome", id = RouteParameter.Optional, action = "CourseSegmentOutcome" });

            //config.Routes.MapHttpRoute("CourseSegmentOutcome2", "outcome/entityoutcomes",
            //                           new { controller = "outcome", action = "GetEntityOutcomes" },
            //                           constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) });

            //config.Routes.MapHttpRoute("OutcomeSupportsOutcome1", "outcome/{supportingOutcomeId}/supports/{supportedOutcomeId}",
            //                           new { controller = "outcome", action = "AddSupportingOutcome" },
            //                           constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Put) });

            //config.Routes.MapHttpRoute("OutcomeSupportsOutcome2", "outcome/{supportingOutcomeId}/supports",
            //                           new { controller = "outcome", action = "GetSupportingOutcomes" },
            //                           constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) });

            //config.Routes.MapHttpRoute("OutcomeSupportsOutcome3", "outcome/{supportingOutcomeId}/supports/{supportedOutcomeId}",
            //                           new { controller = "outcome", action = "RemoveSupportingOutcome" },
            //                           constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Delete) });

            //config.Routes.MapHttpRoute("OutcomeApi", "{entityType}/{entityId}/supports/{outcomeId}",
            //                           new { controller = "outcome", outcomeId = RouteParameter.Optional });

            //config.Routes.MapHttpRoute("CourseSegments", "course/{courseId}/segments/{segmentId}",
            //                           new { controller = "segment", segmentId = RouteParameter.Optional });

            //config.Routes.MapHttpRoute("BulkUpdateCourseSegments", "course/{courseId}/segments",
            //                           new { controller = "segment" });

            //config.Routes.MapHttpRoute("Section", "course/{id}/section",
            //                           new {controller = "Section"});

            //config.Routes.MapHttpRoute("DefaultApi", "{controller}/{id}",
            //                           defaults: new { id = RouteParameter.Optional });

            //config.EnableSystemDiagnosticsTracing();
		}
	}
}
