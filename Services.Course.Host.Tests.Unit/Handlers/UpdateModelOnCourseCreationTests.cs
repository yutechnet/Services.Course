using System;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Handlers;
using BpeProducts.Services.Course.Domain.Repositories;
using Moq;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit.Handlers
{
    [TestFixture]
    public class UpdateModelOnCourseCreationTests
    {
        private Mock<ICourseRepository> _mockCourseRepository;
        private UpdateModelOnCourseCreation _updateModelOnCourseCreationTests;

        [SetUp]
        public void SetUp()
        {
            _mockCourseRepository = new Mock<ICourseRepository>();
            _updateModelOnCourseCreationTests = new UpdateModelOnCourseCreation(_mockCourseRepository.Object);
        }

        [Test]
        public void Throw_Exception_When_Domain_Event_Is_Not_CourseCreated()
        {
            var exception =
                Assert.Throws<InvalidOperationException>(
                    () => _updateModelOnCourseCreationTests.Handle(new FakeDomainEvent()));
            Assert.That(exception.Message, Is.EqualTo("Invalid domain event."));
        }

        [Test]
        public void Add_New_Course_To_Repository()
        {
            var course = new Domain.Entities.Course();
            _updateModelOnCourseCreationTests.Handle(new CourseCreated { Course = course });

            _mockCourseRepository.Verify(c => c.Add(course), Times.Once());
        }
    }
}
