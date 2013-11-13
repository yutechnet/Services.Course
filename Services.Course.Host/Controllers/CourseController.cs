using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web.Http;
using System.Web.Http.OData.Query;
using AttributeRouting.Web.Http;
using BpeProducts.Common.Capabilities;
using BpeProducts.Common.WebApi.Attributes;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain;

namespace BpeProducts.Services.Course.Host.Controllers
{
    //    [DefaultHttpRouteConvention]
    [Authorize]
    public class CourseController : ApiController
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        // GET api/programs
        [HttpGet]
        [GET("course?{$filter}")]
        public IEnumerable<CourseInfoResponse> Get(ODataQueryOptions options)
        {
			// get orgs/objs for which user has viewcourse capability (maybe a matrix later)

			// if user does not have capabilty viewcourse on any orgs or objs return unauthorized/ or return empty collection

			// add orgids and objectids filter criteria
   
            return _courseService.Search(Request.RequestUri.Query);
        }

        [SetSamlTokenInBootstrapContext]
        [HttpGet]
        [GET("course/{courseId:guid}", RouteName = "GetCourse")]
        public CourseInfoResponse Get(Guid courseId)
        {
            return _courseService.Get(courseId);
        }

        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
        [SetSamlTokenInBootstrapContext]
        [HttpPost]
        [POST("course")]
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
        [CheckModelForNull]
        [ValidateModelState]
        [HttpPut]
        [PUT("course/{courseId:guid}")]
        // PUT api/courses/5
        public void Put(Guid courseId, UpdateCourseRequest request)
        {
            _courseService.Update(courseId, request);
        }

        [Transaction]
        [HttpDelete]
        [DELETE("course/{courseId:guid}")]
        public void Delete(Guid courseId)
        {
            _courseService.Delete(courseId);
        }
    }
}