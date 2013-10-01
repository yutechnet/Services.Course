using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BpeProducts.Common.WebApi.Attributes;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Host.Controllers
{
    [Authorize]
    public class SectionController : ApiController
    {
        private readonly ICourseRepository _courseRepository;

        public SectionController(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
        [SetSamlTokenInBootstrapContext]
		// POST api/courses
        public HttpResponseMessage Post(Guid id, CreateSectionRequest request)
        {
            var courseInfoResponse = _courseRepository.Get(id);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, courseInfoResponse);

            string uri = Url.Link("DefaultApi", new {id = courseInfoResponse.Id});
            if (uri != null)
            {
                response.Headers.Location = new Uri(uri);
            }
            return response;
        }
    }
}