using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Services.Course.Domain;
using BpeProducts.Services.Course.Domain.Events;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit
{
    [TestFixture]
    public class PlayCourseCreatedTests
    {
        private PlayCourseCreated _handler;

        [SetUp]
        public void SetUp()
        {
            _handler = new PlayCourseCreated();
        }

        [Test]
        public void Can_add_course_upon_course_created_event()
        {
            var @event = new CourseCreated
                {
                    AggregateId = Guid.NewGuid(),
                    Course = new Domain.Courses.Course
                        {
                            Id = Guid.NewGuid()
                        }
                };

            var course = _handler.Apply(@event, null);

            Assert.That(course, Is.EqualTo(@event.Course));
        }
    }
}
