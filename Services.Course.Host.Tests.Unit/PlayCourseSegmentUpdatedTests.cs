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
    public class PlayCourseSegmentUpdatedTests
    {
        private PlayCourseSegmentUpdated _handler;

        [SetUp]
        public void SetUp()
        {
            _handler = new PlayCourseSegmentUpdated();    
        }

        [Test]
        public void Can_update_cours_segment_upon_event()
        {
            var course = new Course.Domain.Courses.Course();
            var segmentId = Guid.NewGuid();
            course.AddSegment(segmentId, Guid.Empty, new SaveCourseSegmentRequest
                {
                    Name = string.Empty,
                    Description = string.Empty,
                    DisplayOrder = 2
                });


            var msg = new CourseSegmentUpdated
                {
                    AggregateId = Guid.NewGuid(),
                    SegmentId = segmentId,
                    Request = new SaveCourseSegmentRequest
                        {
                            Description = "description",
                            Name = "name",
                            DisplayOrder = 1
                        }
                };

            var updatedCourse = _handler.Apply(msg, course);

            Assert.That(updatedCourse.Segments.Count, Is.EqualTo(1));
            Assert.That(updatedCourse.Segments[0].Description, Is.EqualTo("description"));
            Assert.That(updatedCourse.Segments[0].Name, Is.EqualTo("name"));
            Assert.That(updatedCourse.Segments[0].DisplayOrder, Is.EqualTo(1));
        }
    }
}
