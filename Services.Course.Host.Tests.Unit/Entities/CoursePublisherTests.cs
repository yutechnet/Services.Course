using System;
using Autofac.Extras.Moq;
using BpeProducts.Common.Exceptions;
using BpeProducts.Services.Asset.Contracts;
using BpeProducts.Services.Course.Contract;
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
		private Course.Domain.Courses.Course _course;
		private CoursePublisher _coursePublisher;
		private Guid _segmentId;
		private Guid _learningActivityId;
		private Guid _assessmentId;
		private Guid _assetId;

		private AutoMock _autoMock;
		private Mock<IAssessmentClient> _assessmentClient;
		private Mock<IAssetServiceClient> _assetClient;

		[SetUp]
		public void SetUp()
		{

			MapperConfiguration.Configure();

			// IValidator<Courses.Course> coursePublishValidator
			var _autoMock = AutoMock.GetLoose();
			_autoMock.Provide<IValidator<Course.Domain.Courses.Course>>(
				new CoursePublishValidator(new LearningActivityPublishValidator()));
			_assessmentClient = _autoMock.Mock<IAssessmentClient>();
			_assetClient = _autoMock.Mock<IAssetServiceClient>();


			_course = _autoMock.Create<Course.Domain.Courses.Course>();
			_course.OrganizationId = Guid.NewGuid();
			_course.TenantId = 999999;

			_coursePublisher = _autoMock.Create<CoursePublisher>();

			_segmentId = Guid.NewGuid();
			_learningActivityId = Guid.NewGuid();
			_assessmentId = Guid.NewGuid();
			_assetId = Guid.NewGuid();

			var autoMock = AutoMock.GetLoose();
			_course = autoMock.Create<Course.Domain.Courses.Course>();


			_course.AddSegment(_segmentId, new SaveCourseSegmentRequest());
			_course.AddLearningActivity(_segmentId, new SaveCourseLearningActivityRequest
			{
				AssessmentId = _assessmentId,
				AssessmentType = "Essay"
			}, _learningActivityId);

			

		}

		[Test]
		public void Cannot_publish_course_when_invalid()
		{
			var seg1Id = Guid.NewGuid();
			var seg2Id = Guid.NewGuid();
			var la1Id = Guid.NewGuid();
			var la2Id = Guid.NewGuid();

			_course.AddSegment(seg1Id, new SaveCourseSegmentRequest { Name = "S1" });
			_course.AddSegment(seg2Id, seg1Id, new SaveCourseSegmentRequest { Name = "S2" });
			// Next two are "invalid" from publishing perspective
			// because they are non-Custom type and do not have AssessmentIds.
			_course.AddLearningActivity(seg1Id, new SaveCourseLearningActivityRequest { Name = "LA1", AssessmentType = "Essay" }, la1Id);
			_course.AddLearningActivity(seg2Id, new SaveCourseLearningActivityRequest { Name = "LA2", AssessmentType = "Essay" }, la2Id);

			Assert.Throws<BadRequestException>(() => _coursePublisher.Publish(_course, "should not publish"));
		}

		[Test]
		public void Should_publish_assessments_in_learning_activities_when_course_is_published()
		{

			_assessmentClient.Setup(a => a.GetAssessment(It.IsAny<Guid>())).Returns(new AssessmentInfo { IsPublished = false });
			var publishNote = "hello";
			_coursePublisher.Publish(_course, publishNote);

			_assessmentClient.Verify(a => a.PublishAssessment(_assessmentId, publishNote));
		}

		[Test]
		public void Should_publish_assets_in_learning_materials_when_course_is_published()
		{
			_assessmentClient.Setup(a => a.GetAssessment(It.IsAny<Guid>())).Returns(new AssessmentInfo { IsPublished = false });
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
		public void Should_not_publish_custom_assesstment_type()
		{
			var publishNote = "blah";
			_assessmentClient.Setup(a => a.GetAssessment(It.IsAny<Guid>())).Returns(new AssessmentInfo {IsPublished = false});
			_course.AddLearningActivity(_segmentId, new SaveCourseLearningActivityRequest
			{
				AssessmentType = "Custom"
			}, Guid.NewGuid());

			_coursePublisher.Publish(_course, publishNote);
			// should expect only one call although the segment has two learning activities. 
			_assessmentClient.Verify(a => a.PublishAssessment(It.IsAny<Guid>(), publishNote), Times.Once());
		}

		[Test]
		public void Should_not_publish_assesstments_that_are_published()
		{
			var publishNote = "blah";
			_assessmentClient.Setup(a => a.GetAssessment(It.IsAny<Guid>())).Returns(new AssessmentInfo { IsPublished = true });
			_course.AddLearningActivity(_segmentId, new SaveCourseLearningActivityRequest
			{
				AssessmentType = "Essay",AssessmentId = Guid.NewGuid()
			}, Guid.NewGuid());

			_coursePublisher.Publish(_course, publishNote);
			// should make any calls since all assessments are stubbed to be publsihed 
			_assessmentClient.Verify(a => a.PublishAssessment(It.IsAny<Guid>(), publishNote), Times.Never());
		}
	}
}