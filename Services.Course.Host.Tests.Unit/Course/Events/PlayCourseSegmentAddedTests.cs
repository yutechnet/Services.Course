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
    public class PlayCourseSegmentAddedTests
    {
        private PlayCourseSegmentAdded _handler;
        private Domain.Courses.Course _course;

        [SetUp]
        public void SetUp()
        {
            _course = new Domain.Courses.Course {Id = Guid.NewGuid()};
            _handler = new PlayCourseSegmentAdded();
        }

        [Test]
        public void Can_add_segment_upon_event()
        {
            var parentSegmentId = Guid.NewGuid();

            var @event = new CourseSegmentAdded
                {
                    AggregateId = Guid.NewGuid(),
                    ParentSegmentId = Guid.Empty,
                    Request = new SaveCourseSegmentRequest
                        {
                            Name = "segment name",
                            Description = "segment description"
                        },
                    SegmentId = parentSegmentId
                };

            _handler.Apply(@event, _course);

            Assert.That(_course.Segments.Count, Is.EqualTo(1));
            Assert.That(_course.Segments[0].Id, Is.EqualTo(parentSegmentId));
            Assert.That(_course.Segments[0].Name, Is.EqualTo(@event.Request.Name));

            var segmentId = Guid.NewGuid();
            var @anotherEvent = new CourseSegmentAdded
            {
                AggregateId = Guid.NewGuid(),
                ParentSegmentId = parentSegmentId,
                Request = new SaveCourseSegmentRequest
                {
                    Name = "children segment name",
                    Description = "children segment description"
                },
                SegmentId = segmentId
            };

            _handler.Apply(@anotherEvent, _course);

            Assert.That(_course.Segments.Count, Is.EqualTo(2));
            Assert.That(_course.Segments.First(x => x.Id == parentSegmentId).ChildSegments.Count, Is.EqualTo(1));
            Assert.That(_course.Segments.First(x => x.Id == parentSegmentId).ChildSegments[0].Name, Is.EqualTo(@anotherEvent.Request.Name));
        }
    }
}
