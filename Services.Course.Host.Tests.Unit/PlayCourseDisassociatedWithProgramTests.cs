using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Services.Course.Domain;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Events;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit
{
    [TestFixture]
    public class PlayCourseDisassociatedWithProgramTests
    {
        private PlayCourseDisassociatedWithProgram _handler;

        [SetUp]
        public void SetUp()
        {
            _handler = new PlayCourseDisassociatedWithProgram();
        }

        [Test]
        public void Can_disassociate_program_from_course_upon_event()
        {
            var course = new Course.Domain.Courses.Course();

            var program1 = new Program {Id = Guid.NewGuid()};
            course.AddProgram(program1);
            var program2 = new Program { Id = Guid.NewGuid() };
            course.AddProgram(program2);
            var program3 = new Program { Id = Guid.NewGuid() };
            course.AddProgram(program3);

            var @event = new CourseDisassociatedWithProgram
                {
                    AggregateId = Guid.NewGuid(),
                    ProgramId = program3.Id
                };

            var updatedCourse = _handler.Apply(@event, course);

            Assert.That(updatedCourse.Programs.Count, Is.EqualTo(2));
            Assert.That(updatedCourse.Programs.FirstOrDefault(x => x.Id == program3.Id), Is.Null);
        }
    }
}
