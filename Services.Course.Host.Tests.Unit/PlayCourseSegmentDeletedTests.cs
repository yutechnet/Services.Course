using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain;
using BpeProducts.Services.Course.Domain.Courses;
using BpeProducts.Services.Course.Domain.Events;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit
{
    [TestFixture]
    public class PlayCourseSegmentDeletedTests
    {
        private PlayCourseSegmentDeleted _handler;

        [SetUp]
        public void SetUp()
        {
            _handler = new PlayCourseSegmentDeleted();
        }

        [Test]
        public void Can_delete_segment_upon_event()
        {
            var course = new Course.Domain.Courses.Course();
            var segmentId = Guid.NewGuid();

            course.AddSegment(Guid.NewGuid(), Guid.Empty, new SaveCourseSegmentRequest { Description = "segment 1" });
            course.AddSegment(Guid.NewGuid(), Guid.Empty, new SaveCourseSegmentRequest { Description = "segment 2" });
            course.AddSegment(segmentId, Guid.Empty, new SaveCourseSegmentRequest { Description = "segment 3" });

            var msg = new CourseSegmentDeleted
                {
                    AggregateId = Guid.NewGuid(),
                    SegmentId = segmentId
                };

            var updatedCourse = _handler.Apply(msg, course);

            Assert.That(updatedCourse.Segments.Count(x => x.ActiveFlag == true), Is.EqualTo(2));
            Assert.That(updatedCourse.Segments.FirstOrDefault(x => x.Description == "segment 3" && x.ActiveFlag == true), Is.Null);
        }
    }
}
