using System;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Web.Http;
using BpeProducts.Common.WebApi.NHibernate;
using BpeProducts.Common.WebApi.Validation;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain;
using BpeProducts.Services.Course.Domain.Courses;
using BpeProducts.Services.Course.Domain.Repositories;
using BpeProducts.Services.Section.Contracts;
using Services.Assessment.Contract;

namespace BpeProducts.Services.Course.Host.Controllers
{
    [Authorize]
    public class SectionController : ApiController
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ISectionClient _sectionClient;
        private readonly IAssessmentClient _assessmentClient;
        public SectionController(ICourseRepository courseRepository, ISectionClient sectionClient,IAssessmentClient assessmentClient)
        {
            _courseRepository = courseRepository;
            _sectionClient = sectionClient;
            _assessmentClient = assessmentClient;
        }

        [Transaction]
        [ArgumentsNotNull]
        [ValidateModelState]
        [Route("course/{courseId:guid}/section")]
        public HttpResponseMessage Post(Guid courseId, CourseSectionRequest request)
        {
            var course = _courseRepository.GetOrThrow(courseId);
            var sectionRequest = course.GetSectionRequest(request,_assessmentClient);

            var response = _sectionClient.CreateSection(request.SectionServiceUri, sectionRequest);

            return response;
        }
    }
}