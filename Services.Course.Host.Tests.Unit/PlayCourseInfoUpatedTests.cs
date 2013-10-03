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
    public class PlayCourseInfoUpatedTests
    {
        private PlayCourseInfoUpated _handler;

        [SetUp]
        public void SetUp()
        {
            _handler = new PlayCourseInfoUpated();    
        }

        [Test]
        public void Can_update_course_info()
        {
            var @event = new CourseInfoUpdated
                {
                    AggregateId = Guid.NewGuid(),
                    Code = "course code",
                    CourseType = ECourseType.Traditional,
                    Description = "course description",
                    IsTemplate = true,
                    Name = "course name"
                };

            var course = new Domain.Courses.Course
                {
                    Code = string.Empty,
                    CourseType = ECourseType.Competency,
                    Description = string.Empty,
                    Name = string.Empty
                };

            var updatedCourse = _handler.Apply(@event, course);

            Assert.That(updatedCourse.Code, Is.EqualTo(@event.Code));
            Assert.That(updatedCourse.CourseType, Is.EqualTo(@event.CourseType));
            Assert.That(updatedCourse.Description, Is.EqualTo(@event.Description));
            Assert.That(updatedCourse.Name, Is.EqualTo(@event.Name));
            Assert.That(updatedCourse.IsTemplate, Is.EqualTo(@event.IsTemplate));
        }
    }
}
