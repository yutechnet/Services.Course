using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Services.Course.Domain.Courses.Events;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;
using Moq;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit.Course.Events
{
    [TestFixture]
    public class PlayCoursePrerequisiteRemovedTests
    {
        private PlayCoursePrerequisiteRemoved _handler;
        private Domain.Courses.Course _course;
        private Domain.Courses.Course _prereq;

        [SetUp]
        public void SetUp()
        {
            _handler = new PlayCoursePrerequisiteRemoved();

            _course = new Domain.Courses.Course { Id = Guid.NewGuid() };
            _prereq = new Domain.Courses.Course { Id = Guid.NewGuid(), Name = "Prereq 1" };
            _prereq.Publish("publish note");

            _course.AddPrerequisite(_prereq);
        }

        [Test]
        public void Can_remove_prerequisite_upon_event()
        {
            var @event = new CoursePrerequisiteRemoved
                {
                    AggregateId = Guid.NewGuid(),
                    PrerequisiteCourseId = _prereq.Id
                };

            _handler.Apply(@event, _course);

            Assert.That(_course.Prerequisites.Count, Is.EqualTo(0));

        }
    }
}
