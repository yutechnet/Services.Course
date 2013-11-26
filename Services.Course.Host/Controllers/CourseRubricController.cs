using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting.Web.Http;
using BpeProducts.Common.WebApi.Attributes;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Courses;

namespace BpeProducts.Services.Course.Host.Controllers
{
	[Authorize]
	public class CourseRubricController : ApiController
	{
		private readonly ICourseRubricService _courseRubricService;

		public CourseRubricController(ICourseRubricService courseRubricService)
		{
			_courseRubricService = courseRubricService;
		}

		[Transaction]
		[CheckModelForNull]
		[ValidateModelState]
		[SetSamlTokenInBootstrapContext]
		[HttpPost]
		[POST("course/{courseId:guid}/segments/{segmentId:guid}/learningactivity/{learningActivityId:guid}/courserubric")]
		public HttpResponseMessage Post(Guid courseId, Guid segmentId, Guid learningActivityId, CourseRubricRequest request)
		{
			var courseRubric = _courseRubricService.AddRubric(courseId, segmentId, learningActivityId, request);

			var response = Request.CreateResponse(HttpStatusCode.Created, courseRubric);

			var uri = Url.Link("GetCourseRubric", new { courseId, segmentId, learningActivityId, courseRubricId = courseRubric.Id });
			if (uri != null)
			{
				response.Headers.Location = new Uri(uri);
			}
			return response;
		}

		[HttpGet]
		[GET("course/{courseId:guid}/segments/{segmentId:guid}/learningactivity/{learningActivityId:guid}/courserubric/{courseRubricId:guid}", RouteName = "GetCourseRubric")]
		public CourseRubricInfo Get(Guid courseId, Guid segmentId, Guid learningActivityId, Guid courserubricId)
		{
			return _courseRubricService.Get(courseId, segmentId, learningActivityId, courserubricId);
		}

		[Transaction]
		[HttpDelete]
		[DELETE("course/{courseId:guid}/segments/{segmentId:guid}/learningactivity/{learningActivityId:guid}/courserubric/{rubricId:guid}", RouteName = "DeleteCourseRubric")]
		public void Delete(Guid courseId, Guid segmentId, Guid learningActivityId, Guid rubricId)
		{
			_courseRubricService.DeleteRubric(courseId, segmentId, learningActivityId, rubricId);
		}
	}
}