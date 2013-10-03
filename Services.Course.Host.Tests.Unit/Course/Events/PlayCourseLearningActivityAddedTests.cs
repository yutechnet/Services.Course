using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Courses;
using BpeProducts.Services.Course.Domain.Courses.Events;
using BpeProducts.Services.Course.Domain.Events;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit.Course.Events
{
    [TestFixture]
    public class PlayCourseLearningActivityAddedTests
    {
        private PlayCourseLearningActivityAdded _handler;
        private Domain.Courses.Course _course;
        private Guid _segmentId;

        [SetUp]
        public void SetUp()
        {
            _handler = new PlayCourseLearningActivityAdded();

            _course = new Domain.Courses.Course {Id = Guid.NewGuid()};
            _segmentId = Guid.NewGuid();

            _course.AddSegment(_segmentId, Guid.Empty, new SaveCourseSegmentRequest
                {
                    Name = "segment name",
                    Description = "segment description",
                    DisplayOrder = 1
                });

        }

        [Test]
        public void Can_handle_course_learning_activity_added_event()
        {
            var @event = new CourseLearningActivityAdded
                {
                    AggregateId = Guid.NewGuid(),
                    LearningActivityId = Guid.NewGuid(),
                    Request = new SaveCourseLearningActivityRequest
                        {
                            Name = "LearningActivity",
                            IsGradeable = true,
                            IsExtraCredit = false
                        },
                    SegmentId = _segmentId
                };

            Assert.That(_course.Segments.Count, Is.EqualTo(1));
            Assert.That(_course.Segments[0].CourseLearningActivities.Count, Is.EqualTo(0));

            _handler.Apply(@event, _course);

            Assert.That(_course.Segments.Count, Is.EqualTo(1));
            Assert.That(_course.Segments[0].CourseLearningActivities.Count, Is.EqualTo(1));
            Assert.That(_course.Segments[0].CourseLearningActivities[0].Name, Is.EqualTo("LearningActivity"));
            Assert.That(_course.Segments[0].CourseLearningActivities[0].IsGradeable, Is.True);
            Assert.That(_course.Segments[0].CourseLearningActivities[0].IsExtraCredit, Is.False);
        }
    }
}
