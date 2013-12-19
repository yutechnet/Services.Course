using System;
using System.Net.Http;
using System.Web.Http;
using BpeProducts.Common.WebApi.NHibernate;
using BpeProducts.Common.WebApi.Validation;
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
        [ArgumentsNotNull]
        [ValidateModelState]
		// POST api/courses
        [Route("course/{courseId:guid}/section")]
        public HttpResponseMessage Post(Guid courseId, CourseSectionRequest request)
        {
            var course = _courseRepository.GetOrThrow(courseId);
            var sectionRequest = course.GetSectionRequest(request);

            var response = _sectionClient.CreateSection(request.SectionServiceUri, sectionRequest);
            return response;
        }
    }
}