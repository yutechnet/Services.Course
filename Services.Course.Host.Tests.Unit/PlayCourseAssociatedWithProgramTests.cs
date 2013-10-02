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
    public class PlayCourseAssociatedWithProgramTests
    {
        private PlayCourseAssociatedWithProgram _handler;

        [SetUp]
        public void SetUp()
        {
            _handler = new PlayCourseAssociatedWithProgram();
        }

        [Test]
        public void Can_add_program_upon_course_associated_program_event()
        {
            var @event = new CourseAssociatedWithProgram
                {
                    AggregateId = Guid.NewGuid(),
                    Name = "program name",
                    ProgramId = Guid.NewGuid(),
                    ProgramType = "program type"
                };
            var course = _handler.Apply(@event, new Domain.Courses.Course());

            Assert.That(course.Programs.Count, Is.EqualTo(1));
            Assert.That(course.Programs[0].Name, Is.EqualTo(@event.Name));
            Assert.That(course.Programs[0].ProgramType, Is.EqualTo(@event.ProgramType));
            Assert.That(course.Programs[0].Id, Is.EqualTo(@event.ProgramId));
        }
    }
}
