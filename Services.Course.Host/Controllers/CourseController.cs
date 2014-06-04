using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData.Query;
using BpeProducts.Common.Contract;
using BpeProducts.Common.WebApi.NHibernate;
using BpeProducts.Common.WebApi.Validation;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain;
using BpeProducts.Services.Course.Domain.CourseAggregates;

namespace BpeProducts.Services.Course.Host.Controllers
{
    [Authorize]
    public class CourseController : ApiController
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        // GET api/programs
        [Route("course")]
        public IEnumerable<CourseInfoResponse> Get(ODataQueryOptions options)
        {
			// get orgs/objs for which user has viewcourse capability (maybe a matrix later)

			// if user does not have capabilty viewcourse on any orgs or objs return unauthorized/ or return empty collection

			// add orgids and objectids filter criteria
   
            return _courseService.Search(Request.RequestUri.Query);
        }

        [HttpGet]
        [Route("course/published")]
        public IEnumerable<CourseInfoResponse> GetPublishedCourses(Guid organizationId)
        {
            return _courseService.GetPublishedCourses(organizationId);
        }

        [Route("course/{courseId:guid}", Name = "GetCourse")]
        public CourseInfoResponse Get(Guid courseId)
        {
            return _courseService.Get(courseId);
        }

        [Transaction]
        [ArgumentsNotNull]
        [ValidateModelState]
        [Route("course")]
        public HttpResponseMessage Post(SaveCourseRequest request)
        {
	        var courseInfoResponse = _courseService.Create(request);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, courseInfoResponse);

            string uri = Url.Link("GetCourse", new { courseId = courseInfoResponse.Id });
            if (uri != null)
            {
                response.Headers.Location = new Uri(uri);
            }
            return response;
        }

        [Transaction]
        [ArgumentsNotNull]
        [ValidateModelState]
        [Route("coursefromtemplate")]
        public HttpResponseMessage Post(CreateCourseFromTemplateRequest request)
        {
            var courseInfoResponse = _courseService.Create(request);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, courseInfoResponse);

            string uri = Url.Link("GetCourse", new { courseId = courseInfoResponse.Id });
            if (uri != null)
            {
                response.Headers.Location = new Uri(uri);
            }
            return response;
        }

        [Transaction]
        [ArgumentsNotNull]
        [ValidateModelState]
        [Route("course/{courseId:guid}")]
        // PUT api/courses/5
        public void Put(Guid courseId, UpdateCourseRequest request)
        {
            _courseService.Update(courseId, request);
        }

        [Transaction]
        [Route("course/{courseId:guid}")]
        public void Delete(Guid courseId)
        {
            _courseService.Delete(courseId);
        }

		[Transaction]
		[ArgumentsNotNull]
		[ValidateModelState]
		[HttpPost]
		[Route("course/version", Name = "CreateCourseVersion")]
		public HttpResponseMessage CreateVersion(VersionRequest request)
		{
			var courseInfoResponse = _courseService.CreateVersion(request.ParentVersionId, request.VersionNumber);
			HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

			string uri = Url.Link("GetCourse", new { courseId = courseInfoResponse.Id });
			if (uri != null)
			{
				response.Headers.Location = new Uri(uri);
			}
			return response;
		}

		[Transaction]
		[ArgumentsNotNull]
		[ValidateModelState]
		[HttpPut]
		[Route("course/{courseId:guid}/publish")]
		public void PublishVersion(Guid courseId, PublishRequest request)
		{
			_courseService.PublishVersion(courseId, request.PublishNote);
		}

        [Transaction]
		[ArgumentsNotNull]
		[ValidateModelState]
		[HttpPut]
		[Route("course/{courseId:guid}/activate")]
        public void UpdateActiviationStatus(Guid courseId, ActivationRequest request)
        {
            _courseService.UpdateActiviationStatus(courseId, request);
        }
    }
}