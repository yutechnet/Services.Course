using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BpeProducts.Common.WebApi.NHibernate;
using BpeProducts.Common.WebApi.Validation;
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
		[ArgumentsNotNull]
		[ValidateModelState]
        [Route("course/{courseId:guid}/segments/{segmentId:guid}/learningactivity/{learningActivityId:guid}/courserubric")]
		public HttpResponseMessage Post(Guid courseId, Guid segmentId, Guid learningActivityId, CourseRubricRequest request)
		{
			var courseRubric = _courseRubricService.AddRubric(courseId, segmentId, learningActivityId, request);

			var response = Request.CreateResponse(HttpStatusCode.Created, courseRubric);

			var uri = Url.Link("GetCourseRubric", new { courseId, segmentId, learningActivityId, courseRubricId = courseRubric.RubricId });
			if (uri != null)
			{
				response.Headers.Location = new Uri(uri);
			}
			return response;
		}

        [Route("course/{courseId:guid}/segments/{segmentId:guid}/learningactivity/{learningActivityId:guid}/courserubric/{courseRubricId:guid}", Name = "GetCourseRubric")]
		public CourseRubricInfo Get(Guid courseId, Guid segmentId, Guid learningActivityId, Guid courserubricId)
		{
			return _courseRubricService.Get(courseId, segmentId, learningActivityId, courserubricId);
		}

		[Transaction]
        [Route("course/{courseId:guid}/segments/{segmentId:guid}/learningactivity/{learningActivityId:guid}/courserubric/{rubricId:guid}", Name = "DeleteCourseRubric")]
		public void Delete(Guid courseId, Guid segmentId, Guid learningActivityId, Guid rubricId)
		{
			_courseRubricService.DeleteRubric(courseId, segmentId, learningActivityId, rubricId);
		}
	}
}