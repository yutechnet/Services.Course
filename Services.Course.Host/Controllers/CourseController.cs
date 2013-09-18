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
		//[ClaimsAuth("ViewCourses")]
        public IEnumerable<CourseInfoResponse> Get(ODataQueryOptions options)
        {
			// get orgs/objs for which user has viewcourse capability (maybe a matrix later)

			// if user does not have capabilty viewcourse on any orgs or objs return unauthorized/ or return empty collection

			// add orgids and objectids filter criteria
   
            return _courseService.Search(Request.RequestUri.Query);
        }

        // GET api/courses/5
        public CourseInfoResponse Get(Guid id)
        {
	        SetSamlTokenInBootstrapContext();
            return _courseService.Get(id);
        }

        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
		// POST api/courses
        public HttpResponseMessage Post(SaveCourseRequest request)
        {
			SetSamlTokenInBootstrapContext();
	       
	        var courseInfoResponse = _courseService.Create(request);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, courseInfoResponse);

            string uri = Url.Link("DefaultApi", new {id = courseInfoResponse.Id});
            if (uri != null)
            {
                response.Headers.Location = new Uri(uri);
            }
            return response;
        }

	    private void SetSamlTokenInBootstrapContext()
	    {
			//TODO: move this to common.webapi
		    var user = User as ClaimsPrincipal;
		    var identity = user.Identities.First();
		    var authenticationHeadervalue = Request.Headers.Authorization;
		    identity.BootstrapContext = new BootstrapContext(authenticationHeadervalue.Parameter);
	    }

	    [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
        // PUT api/courses/5
        public void Put(Guid id, UpdateCourseRequest request)
        {
            _courseService.Update(id, request);
        }

        [Transaction]
        // DELETE api/courses/5
        public void Delete(Guid id)
        {
            _courseService.Delete(id);
        }
    }
}