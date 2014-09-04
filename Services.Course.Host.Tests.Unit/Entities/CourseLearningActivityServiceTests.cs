using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Autofac.Extras.Moq;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain;
using BpeProducts.Services.Course.Domain.CourseAggregates;
using BpeProducts.Services.Course.Domain.Repositories;
using Moq;
using NUnit.Framework;
using BpeProducts.Services.Assessment.Contract;

namespace BpeProducts.Services.Course.Host.Tests.Unit.Entities
{
    [TestFixture]
    public class CourseLearningActivityServiceTests
    {
        private Guid _courseId;
        private Guid _segmentId;
        private Guid _assessmentId;
        private Guid _learningActivityId;
        private Mock<IAssessmentClient> _assessmentClient;
        private Mock<ICourseRepository> _courseRepository;
        private CourseLearningActivityService _courseLearningActivityService;

        private Domain.CourseAggregates.Course _course;
	    private AutoMock _autoMock;

	    [SetUp]
        public void SetUp()
        {
            _assessmentClient = new Mock<IAssessmentClient>();
            _courseRepository = new Mock<ICourseRepository>();

            _courseId = Guid.NewGuid();
            _assessmentId = Guid.NewGuid();
	        _autoMock = AutoMock.GetLoose();
		    _course = _autoMock.Create<Domain.CourseAggregates.Course>();
			_course.Id = _courseId;

            _segmentId = _course.AddSegment(new SaveCourseSegmentRequest()).Id;
            _learningActivityId = _course.AddLearningActivity(_segmentId, new SaveCourseLearningActivityRequest {AssessmentType = "Custom"}).Id;

            _courseLearningActivityService = new CourseLearningActivityService(_courseRepository.Object, _assessmentClient.Object);

            _courseRepository.Setup(c => c.Get(_courseId)).Returns(_course);

            Mapper.CreateMap<CourseLearningActivity, CourseLearningActivityResponse>();
        }

        [Test]
        public void Should_check_for_valid_assessment_when_new_learning_activity_is_created()
        {
            _assessmentClient.Setup(a => a.GetAssessment(_assessmentId))
                             .Returns(new AssessmentInfo
                                 {
                                     Id = _assessmentId,
                                     AssessmentType = AssessmentType.Essay
                                 });

            var courseLearningActivityRequest = new SaveCourseLearningActivityRequest { AssessmentId = _assessmentId };

            var learningActivity = _courseLearningActivityService.Create(_courseId, _segmentId, courseLearningActivityRequest);

            _assessmentClient.Verify(a => a.GetAssessment(_assessmentId));
            Assert.That(learningActivity.AssessmentType, Is.EqualTo("Essay"));
        }

        [Test]
        public void Should_check_for_valid_assessment_when_added_to_existing_learning_activity()
        {
            _assessmentClient.Setup(a => a.GetAssessment(_assessmentId))
                             .Returns(new AssessmentInfo
                             {
                                 Id = _assessmentId,
                                 AssessmentType = AssessmentType.Essay
                             });

            var courseLearningActivityRequest = new SaveCourseLearningActivityRequest { AssessmentId = _assessmentId };
            _courseLearningActivityService.Update(_courseId, _segmentId, _learningActivityId, courseLearningActivityRequest);

            _assessmentClient.Verify(a => a.GetAssessment(_assessmentId));
        }
    }
}
