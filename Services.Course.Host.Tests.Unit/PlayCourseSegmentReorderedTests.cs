using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain;
using BpeProducts.Services.Course.Domain.Events;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit
{
    [TestFixture]
    public class PlayCourseSegmentReorderedTests
    {
        private PlayCourseSegmentReordered _handler;

        [SetUp]
        public void SetUp()
        {
            _handler = new PlayCourseSegmentReordered();
        }

        [Test]
        public void Can_reorder_course_segment_upon_event()
        {
            var course = new Course.Domain.Courses.Course();
            var segmentId = Guid.NewGuid();
            var request = new SaveCourseSegmentRequest
                {
                    Name = string.Empty,
                    Description = string.Empty,
                    DisplayOrder = 0
                };
            course.AddSegment(segmentId, Guid.Empty, request);

            var msg = new CourseSegmentReordered
                {
                    AggregateId = Guid.NewGuid(),
                    DisplayOrder = 10,
                    Request = new UpdateCourseSegmentRequest
                        {
                            Name = "name",
                            Description = "description"
                        },
                    SegmentId = segmentId
                };

            var updatedCourse = _handler.Apply(msg, course);

            Assert.That(updatedCourse.Segments.Count, Is.EqualTo(1));
            Assert.That(updatedCourse.Segments[0].Description, Is.EqualTo(msg.Request.Description));
            Assert.That(updatedCourse.Segments[0].Name, Is.EqualTo(msg.Request.Name));
            Assert.That(updatedCourse.Segments[0].DisplayOrder, Is.EqualTo(msg.DisplayOrder));

        }
    }
}
