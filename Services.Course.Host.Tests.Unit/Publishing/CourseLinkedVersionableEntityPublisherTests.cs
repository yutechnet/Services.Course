using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using BpeProducts.Services.Asset.Contracts;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Courses;
using Moq;
using NUnit.Framework;
using Services.Assessment.Contract;

namespace BpeProducts.Services.Course.Host.Tests.Unit.Publishing
{
	[TestFixture]
	public class CourseLinkedVersionableEntityPublisherTests
	{
		private Course.Domain.Courses.Course _course;
		private Guid _segmentId;
		private Guid _learningActivityId;
		private Guid _assessmentId;

		private Mock<IAssessmentClient> _assessmentClient;
		private Mock<IAssetServiceClient> _assetClient;
		private CourseLinkedVersionableEntityPublisher _courseLinkedVersionableEntityPublisher;

		[SetUp]
		public void Before_each()
		{
			_assessmentClient = new Mock<IAssessmentClient>();
			_assetClient = new Mock<IAssetServiceClient>();


			_segmentId = Guid.NewGuid();
			_learningActivityId = Guid.NewGuid();
			_assessmentId = Guid.NewGuid();
			var autoMock = AutoMock.GetLoose();
			_course = autoMock.Create<Course.Domain.Courses.Course>();
			

			_course.AddSegment(_segmentId, new SaveCourseSegmentRequest());
			_course.AddLearningActivity(_segmentId, new SaveCourseLearningActivityRequest
				{
					AssessmentId = _assessmentId,
					AssessmentType = "Essay"
				}, _learningActivityId);

			_courseLinkedVersionableEntityPublisher = new CourseLinkedVersionableEntityPublisher(_assessmentClient.Object,
			                                                                                     _assetClient.Object);
		}

		[Test]
		public void Should_publish_assessments_in_learning_activities_when_course_is_published()
		{
			var publishNote = "hello";
			_courseLinkedVersionableEntityPublisher.Publish(_course, publishNote);

			_assessmentClient.Verify(a => a.PublishAssessment(_assessmentId, publishNote));
		}

		[Test]
		public void Should_not_publish_custom_assesstment_type()
		{
			var publishNote = "blah";
			_course.AddLearningActivity(_segmentId, new SaveCourseLearningActivityRequest
				{
					AssessmentType = "Custom"
				}, Guid.NewGuid());

			_courseLinkedVersionableEntityPublisher.Publish(_course, publishNote);
			// should expect only one call although the segment has two learning activities. 
			_assessmentClient.Verify(a => a.PublishAssessment(It.IsAny<Guid>(), publishNote), Times.Once());
		}
	}
}
