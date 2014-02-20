using System;
using Autofac.Extras.Moq;
using BpeProducts.Common.Exceptions;
using BpeProducts.Services.Asset.Contracts;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain;
using BpeProducts.Services.Course.Domain.Courses;
using BpeProducts.Services.Course.Domain.Validation;
using BpeProducts.Services.Course.Host.App_Start;
using Moq;
using NUnit.Framework;
using Services.Assessment.Contract;

namespace BpeProducts.Services.Course.Host.Tests.Unit.Entities
{
    [TestFixture]
    public class CoursePublisherTests
    {
        private Domain.Courses.Course _course;
        private CoursePublisher _coursePublisher;
        private Guid _segmentId;
        private Guid _assessmentId;
        private Guid _assetId;

        private AutoMock _autoMock;
        private Mock<IAssessmentClient> _assessmentClient;
        private Mock<IAssetServiceClient> _assetClient;

        [SetUp]
        public void SetUp()
        {
            MapperConfiguration.Configure();

            _autoMock = AutoMock.GetLoose();
            _autoMock.Provide<IValidator<Domain.Courses.Course>>(new CoursePublishValidator(new LearningActivityPublishValidator()));
            _assessmentClient = _autoMock.Mock<IAssessmentClient>();
            _assetClient = _autoMock.Mock<IAssetServiceClient>();

            _course = new Domain.Courses.Course
                {
                    OrganizationId = Guid.NewGuid(), 
                    TenantId = 999999
                };

            _coursePublisher = _autoMock.Create<CoursePublisher>();

            _segmentId = Guid.NewGuid();
            _assessmentId = Guid.NewGuid();
            _assetId = Guid.NewGuid();

            _course.AddSegment(_segmentId, new SaveCourseSegmentRequest());
            _course.AddLearningActivity(_segmentId, new SaveCourseLearningActivityRequest
                {
                    AssessmentId = _assessmentId,
                    AssessmentType = "Essay"
                });
        }

        [Test]
        public void Cannot_publish_course_when_invalid()
        {
            var seg1Id = Guid.NewGuid();
            var seg2Id = Guid.NewGuid();

            _course.AddSegment(seg1Id, new SaveCourseSegmentRequest {Name = "S1"});
            _course.AddSegment(seg2Id, seg1Id, new SaveCourseSegmentRequest {Name = "S2"});
            // Next two are "invalid" from publishing perspective
            // because they are non-Custom type and do not have AssessmentIds.
            _course.AddLearningActivity(seg1Id, new SaveCourseLearningActivityRequest { Name = "LA1", AssessmentType = "Essay" });
            _course.AddLearningActivity(seg2Id, new SaveCourseLearningActivityRequest {Name = "LA2", AssessmentType = "Essay"});

            Assert.Throws<BadRequestException>(() => _coursePublisher.Publish(_course, "should not publish"));
        }

        [Test]
        public void Should_publish_assessments_in_learning_activities_when_course_is_published()
        {
            _assessmentClient.Setup(a => a.GetAssessment(It.IsAny<Guid>()))
                             .Returns(new AssessmentInfo {IsPublished = false});
            var publishNote = "hello";
            _coursePublisher.Publish(_course, publishNote);

            _assessmentClient.Verify(a => a.PublishAssessment(_assessmentId, publishNote));
        }

        [Test]
        public void Should_publish_assets_in_learning_materials_when_course_is_published()
        {
            _assessmentClient.Setup(a => a.GetAssessment(It.IsAny<Guid>())).Returns(new AssessmentInfo {IsPublished = false});
            _course.AddLearningMaterial(_segmentId, new LearningMaterialRequest
                {
                    AssetId = _assetId,
                    Instruction = "Hello",
                    IsRequired = true
                });

            _assetClient.Setup(a => a.GetAsset(It.IsAny<Guid>())).Returns(new AssetInfo
                {
                    IsPublished = false
                });

            var publishNote = "hello";
            _coursePublisher.Publish(_course, publishNote);

            _assetClient.Verify(a => a.PublishAsset(_assetId, publishNote));
        }

        [Test]
        public void Should_not_publish_assesstments_that_are_published()
        {
            var publishNote = "blah";
            _assessmentClient.Setup(a => a.GetAssessment(It.IsAny<Guid>()))
                             .Returns(new AssessmentInfo {IsPublished = true});

            _course.AddLearningActivity(_segmentId, new SaveCourseLearningActivityRequest
                {
                    AssessmentType = "Essay",
                    AssessmentId = Guid.NewGuid()
                });

            _coursePublisher.Publish(_course, publishNote);
            // should make any calls since all assessments are stubbed to be publsihed 
            _assessmentClient.Verify(a => a.PublishAssessment(It.IsAny<Guid>(), publishNote), Times.Never());
        }
    }
}