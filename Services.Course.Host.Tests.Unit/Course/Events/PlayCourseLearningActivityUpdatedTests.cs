using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Courses.Events;
using BpeProducts.Services.Course.Domain.Events;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit.Course.Events
{
    [TestFixture]
    public class PlayCourseLearningActivityUpdatedTests
    {
        private PlayCourseLearningActivityUpdated _handler;
        private Domain.Courses.Course _course;
        private Guid _segmentId;
        private Guid _learningActivityId;

        [SetUp]
        public void SetUp()
        {
            _handler = new PlayCourseLearningActivityUpdated();

            _course = new Domain.Courses.Course { Id = Guid.NewGuid() };
            _segmentId = Guid.NewGuid();
            _learningActivityId = Guid.NewGuid();

            _course.AddSegment(_segmentId, Guid.Empty, new SaveCourseSegmentRequest
            {
                Name = "segment name",
                Description = "segment description",
                DisplayOrder = 1
            });

            _course.AddLearningActivity(_segmentId, new SaveCourseLearningActivityRequest
            {
                Name = "LearningActivity",
                IsGradeable = true,
                IsExtraCredit = false
            }, _learningActivityId);
        }

        [Test]
        public void Can_update_learning_activity_upon_event()
        {
            Assert.That(_course.Segments.Count, Is.EqualTo(1));
            Assert.That(_course.Segments[0].CourseLearningActivities.Count, Is.EqualTo(1));
            Assert.That(_course.Segments[0].CourseLearningActivities[0].Name, Is.EqualTo("LearningActivity"));

            _handler.Apply(new CourseLearningActivityUpdated
                {
                    AggregateId = Guid.NewGuid(),
                    LearningActivityId = _learningActivityId,
                    SegmentId = _segmentId,
                    Request = new SaveCourseLearningActivityRequest
                        {
                            Name = "UpdatedName",
                            IsGradeable = false,
                            IsExtraCredit = true
                        }
                }, _course);

            Assert.That(_course.Segments.Count, Is.EqualTo(1));
            Assert.That(_course.Segments[0].CourseLearningActivities.Count, Is.EqualTo(1));
            Assert.That(_course.Segments[0].CourseLearningActivities[0].Name, Is.EqualTo("UpdatedName"));
            Assert.That(_course.Segments[0].CourseLearningActivities[0].IsGradeable, Is.False);
            Assert.That(_course.Segments[0].CourseLearningActivities[0].IsExtraCredit, Is.True);
        }
    }
}
