using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BpeProducts.Common.Ioc.Extensions;
using BpeProducts.Common.WebApi.NHibernate;
using BpeProducts.Common.WebApi.Validation;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain;

namespace BpeProducts.Services.Course.Host.Controllers
{
    [Authorize]
    public class CourseLearningActivityController : ApiController
	{
        private readonly ICourseLearningActivityService _courseLearningActivityService;
        private readonly string _regExPattern = ConfigurationManager.AppSettings["Regex.Course"];
        private readonly string _rewriteUrl = ConfigurationManager.AppSettings["RewriteUrl.Course"];

        public CourseLearningActivityController(ICourseLearningActivityService courseLearningActivityService)
        {
            _courseLearningActivityService = courseLearningActivityService;
        }

        [Route("course/{courseId:guid}/segment/{segmentId:guid}/learningactivity/{id:guid}", Name = "GetCourseLearningActivity")]
		public CourseLearningActivityResponse Get(Guid courseId, Guid segmentId, Guid id)
		{
            var learningActivity = _courseLearningActivityService.Get(courseId, segmentId, id);
            return learningActivity;
		}

        [Route("course/{courseId:guid}/segment/{segmentId:guid}/learningactivity/")]
        public IEnumerable<CourseLearningActivityResponse> Get(Guid courseId, Guid segmentId)
        {
            return _courseLearningActivityService.Get(courseId, segmentId);
        }

		[Transaction]
        [ArgumentsNotNull]
		[ValidateModelState]
        [Route("course/{courseId:guid}/segment/{segmentId:guid}/learningactivity")]
        public HttpResponseMessage Post(Guid courseId, Guid segmentId, SaveCourseLearningActivityRequest request)
		{
            var learningActivityResponse = _courseLearningActivityService.Create(courseId, segmentId, request);
            var response = Request.CreateResponse(HttpStatusCode.Created, learningActivityResponse);

            var uri = Url.Link("GetCourseLearningActivity", new { id = learningActivityResponse.Id });
            uri = uri.UrlRewrite(_regExPattern, _rewriteUrl);

            if (uri != null)
            {
                response.Headers.Location = new Uri(uri);
            }
            return response;
		}

		[Transaction]
		[ArgumentsNotNull]
		[ValidateModelState]
        [Route("course/{courseId:guid}/segment/{segmentId:guid}/learningactivity/{id:guid}")]
        public HttpResponseMessage Put(Guid courseId, Guid segmentId, Guid id, SaveCourseLearningActivityRequest request)
		{
            _courseLearningActivityService.Update(courseId, segmentId, id, request);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
		}

		[Transaction]
        [Route("course/{courseId:guid}/segment/{segmentId:guid}/learningactivity/{id:guid}")]
        public void Delete(Guid courseId, Guid segmentId, Guid id)
		{
            _courseLearningActivityService.Delete(courseId, segmentId, id);
		}
	}
}

