using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData.Query;
using BpeProducts.Common.Exceptions;
using BpeProducts.Common.WebApi.Attributes;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain;
using BpeProducts.Services.Course.Domain.Entities;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.OData;

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

        // GET course/{courseId}/segment/{segmentId}/learningactivity/{id}
		public CourseLearningActivityResponse Get(Guid courseId, Guid segmentId, Guid id)
		{
            return _courseLearningActivityService.Get(courseId, segmentId, id);
		}

		[Transaction]
		[CheckModelForNull]
		[ValidateModelState]
        // POST course/{courseId}/segment/{segmentId}/learningactivity
        public HttpResponseMessage Post(Guid courseId, Guid segmentId, SaveCourseLearningActivityRequest request)
		{
            var learningActivityResponse = _courseLearningActivityService.Create(courseId, segmentId, request);
            var response = Request.CreateResponse(HttpStatusCode.Created, learningActivityResponse);

            var uri = Url.Link("CourseLearningActivity", new { id = learningActivityResponse.Id });
            if (uri != null)
            {
                response.Headers.Location = new Uri(uri);
            }
            return response;
		}

		[Transaction]
		[CheckModelForNull]
		[ValidateModelState]
		// PUT course/{courseId}/segment/{segmentId}/learningactivity/{id}
        public HttpResponseMessage Put(Guid courseId, Guid segmentId, Guid id, SaveCourseLearningActivityRequest request)
		{
            _courseLearningActivityService.Update(courseId, segmentId, id, request);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
		}

		[Transaction]
		// DELETE course/{courseId}/segment/{segmentId}/learningactivity/id
        public void Delete(Guid courseId, Guid segmentId, Guid id)
		{
            _courseLearningActivityService.Delete(courseId, segmentId, id);
		}
	}
}

