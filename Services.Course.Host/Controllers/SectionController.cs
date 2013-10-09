using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BpeProducts.Common.WebApi.Attributes;
using BpeProducts.Services.Course.Domain.Courses;
using BpeProducts.Services.Course.Domain.Repositories;
using BpeProducts.Services.Course.Host.TempSectionContracts;
using CreateSectionRequest = BpeProducts.Services.Course.Contract.CreateSectionRequest;

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
            var course = _courseRepository.GetOrThrow(id);
            var translatedRequest = new TempSectionContracts.CreateSectionRequest
                {
                    Name = request.Name,
                    Code = request.Code,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    TenantId = course.TenantId,
                    CourseId = course.Id,
                    Segments = BuildSectionSegments(course.Segments)
                };

            var client = new SectionClient(request.SectionServiceUri);

            var response = client.CreateSection(translatedRequest);
            return response;
        }

        private List<SectionSegmentRequest> BuildSectionSegments(IEnumerable<CourseSegment> segments)
        {
            var sectionSegments = segments.Select(courseSegment => new SectionSegmentRequest
                {
                    Name = courseSegment.Name, 
                    Description = courseSegment.Description, 
                    DisplayOrder = courseSegment.DisplayOrder, 
                    Type = courseSegment.Type, 
                    CourseSegmentId = courseSegment.Id, 
                    ChildSegments = BuildSectionSegments(courseSegment.ChildSegments), 
                    LearningActivities = BuildLearningActivities(courseSegment.CourseLearningActivities)
                }).ToList();

            return sectionSegments;
        }

        private IList<SectionLearningActivityRequest> BuildLearningActivities(IEnumerable<CourseLearningActivity> courseLearningActivities)
        {
            var sectionLearningActivities = courseLearningActivities.Select(courseLearningActivity => new SectionLearningActivityRequest
                {
                    Name = courseLearningActivity.Name,
                    Type = (SectionLearningActivityType) courseLearningActivity.Type,
                    ActiveDate = courseLearningActivity.ActiveDate,
                    InactiveDate = courseLearningActivity.InactiveDate,
                    DueDate = courseLearningActivity.DueDate,
                    IsExtraCredit = courseLearningActivity.IsExtraCredit,
                    IsGradeable = courseLearningActivity.IsGradeable,
                    MaxPoint = courseLearningActivity.MaxPoint,
                    Weight = courseLearningActivity.Weight,
                    ObjectId = courseLearningActivity.ObjectId,
                    CustomAttribute = courseLearningActivity.CustomAttribute
                }).ToList();

            return sectionLearningActivities;
        }
    }
}