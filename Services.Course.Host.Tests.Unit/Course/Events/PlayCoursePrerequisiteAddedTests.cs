using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Course.Domain.Courses.Events;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;
using Moq;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit.Course.Events
{
    [TestFixture]
    public class PlayCoursePrerequisiteAddedTests
    {
        private Mock<ICourseRepository> _mockRepository;
        private PlayCoursePrerequisiteAdded _handler;
        private Domain.Courses.Course _course;
        private Domain.Courses.Course _prereq;

        [SetUp]
        public void SetUp()
        {
            _mockRepository = new Mock<ICourseRepository>();
            _handler = new PlayCoursePrerequisiteAdded(_mockRepository.Object);

            _course = new Domain.Courses.Course {Id = Guid.NewGuid()};
            _prereq = new Domain.Courses.Course {Id = Guid.NewGuid(), Name = "Prereq 1"};
            _prereq.Publish("publish note");
        }

        [Test]
        public void Can_add_prerequisite_upon_event()
        {
            _mockRepository.Setup(x => x.Get(It.IsAny<Guid>()))
                           .Returns(_prereq);

            var @event = new CoursePrerequisiteAdded
                {
                    AggregateId = Guid.NewGuid(),
                    PrerequisiteCourseId = Guid.NewGuid()
                };
            _handler.Apply(@event, _course);

            Assert.That(_course.Prerequisites.Count, Is.EqualTo(1));
            Assert.That(_course.Prerequisites[0], Is.EqualTo(_prereq));
        }
    }
}
