using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting.Web.Http;
using BpeProducts.Common.WebApi.Attributes;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain;

namespace BpeProducts.Services.Course.Host.Controllers
{
    [Authorize]
    public class CourseLearningActivityController : ApiController
	{
        private readonly ICourseLearningActivityService _courseLearningActivityService;

        public CourseLearningActivityController(ICourseLearningActivityService courseLearningActivityService)
        {
            _courseLearningActivityService = courseLearningActivityService;
        }

        [HttpGet]
        [GET("course/{courseId:guid}/segments/{segmentId:guid}/learningactivity/{id:guid}", RouteName = "GetCourseLearningActivity")]
		public CourseLearningActivityResponse Get(Guid courseId, Guid segmentId, Guid id)
		{
            return _courseLearningActivityService.Get(courseId, segmentId, id);
		}

        [HttpGet]
        [GET("course/{courseId:guid}/segments/{segmentId:guid}/learningactivity/")]
        public IEnumerable<CourseLearningActivityResponse> Get(Guid courseId, Guid segmentId)
        {
            return _courseLearningActivityService.Get(courseId, segmentId);
        }

		[Transaction]
		[CheckModelForNull]
		[ValidateModelState]
        [HttpPost]
        [POST("course/{courseId:guid}/segments/{segmentId:guid}/learningactivity")]
        public HttpResponseMessage Post(Guid courseId, Guid segmentId, SaveCourseLearningActivityRequest request)
		{
            var learningActivityResponse = _courseLearningActivityService.Create(courseId, segmentId, request);
            var response = Request.CreateResponse(HttpStatusCode.Created, learningActivityResponse);

            var uri = Url.Link("GetCourseLearningActivity", new { id = learningActivityResponse.Id });
            if (uri != null)
            {
                response.Headers.Location = new Uri(uri);
            }
            return response;
		}

		[Transaction]
		[CheckModelForNull]
		[ValidateModelState]
        [HttpPut]
        [PUT("course/{courseId:guid}/segments/{segmentId:guid}/learningactivity/{id:guid}")]
        public HttpResponseMessage Put(Guid courseId, Guid segmentId, Guid id, SaveCourseLearningActivityRequest request)
		{
            _courseLearningActivityService.Update(courseId, segmentId, id, request);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
		}

		[Transaction]
        [HttpDelete]
        [DELETE("course/{courseId:guid}/segments/{segmentId:guid}/learningactivity/{id:guid}")]
        public void Delete(Guid courseId, Guid segmentId, Guid id)
		{
            _courseLearningActivityService.Delete(courseId, segmentId, id);
		}
	}
}

