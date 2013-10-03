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
    public class PlayCourseLearningActivityDeletedTests
    {
        private PlayCourseLearningActivityDeleted _handler;
        private Domain.Courses.Course _course;
        private Guid _segmentId;
        private Guid _learningActivityId;

        [SetUp]
        public void SetUp()
        {
            _handler = new PlayCourseLearningActivityDeleted();

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
        public void Can_delete_learning_activity_upon_event()
        {
            Assert.That(_course.Segments.Count, Is.EqualTo(1));
            Assert.That(_course.Segments[0].CourseLearningActivities.Count, Is.EqualTo(1));

            _handler.Apply(new CourseLearningActivityDeleted
                {
                    AggregateId = Guid.NewGuid(),
                    LearningActivityId = _learningActivityId,
                    SegmentId = _segmentId
                }, _course);

            Assert.That(_course.Segments.Count, Is.EqualTo(1));
            Assert.That(_course.Segments[0].CourseLearningActivities.Count(x => x.ActiveFlag == true), Is.EqualTo(0));
        }
    }
}
