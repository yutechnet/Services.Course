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
    public class PlayCourseDeletedTests
    {
        private PlayCourseDeleted _handler;

        [SetUp]
        public void SetUp()
        {
            _handler = new PlayCourseDeleted();
        }

        [Test]
        public void Can_delete_course_upon_course_deleted_event()
        {
            var @event = new CourseDeleted
                {
                    AggregateId = Guid.NewGuid()
                };


            var course = _handler.Apply(@event, new Domain.Courses.Course
                {
                    ActiveFlag = true
                });

            Assert.That(course.ActiveFlag, Is.False);
        }
    }
}
