using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BpeProducts.Common.WebApi.Attributes;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Repositories;
using Services.Section.Contracts;

namespace BpeProducts.Services.Course.Host.Controllers
{
    [Authorize]
    public class SectionController : ApiController
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ISectionClient _sectionClient;

        public SectionController(ICourseRepository courseRepository, ISectionClient sectionClient)
        {
            _courseRepository = courseRepository;
            _sectionClient = sectionClient;
        }

        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
        [SetSamlTokenInBootstrapContext]
		// POST api/courses
        public HttpResponseMessage Post(Guid id, CourseSectionRequest request)
        {
            var course = _courseRepository.GetOrThrow(id);
            var sectionRequest = course.GetSectionRequest(request);

            var response = _sectionClient.CreateSection(request.SectionServiceUri, sectionRequest);
            return response;
        }
    }
}