using System;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Handlers;
using BpeProducts.Services.Course.Domain.Repositories;
using Moq;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit.Handlers
{
    [TestFixture]
    public class UpdateModelOnCourseDeletionTests
    {
        private Mock<ICourseRepository> _mockCourseRepository;
        private UpdateModelOnCourseDeletion _updateModelOnCourseDeletion;

        [SetUp]
        public void SetUp()
        {
            _mockCourseRepository = new Mock<ICourseRepository>();
            _updateModelOnCourseDeletion = new UpdateModelOnCourseDeletion(_mockCourseRepository.Object);
        }

        [Test]
        public void Throws_Exception_When_DomainEvent_Is_Not_CourseDeleted()
        {
            var exception =
                Assert.Throws<InvalidOperationException>(
                    () => _updateModelOnCourseDeletion.Handle(new FakeDomainEvent()));

            Assert.That(exception.Message, Is.EqualTo("Invalid domain event."));
        }

        [Test]
        public void Soft_Deletes_Course_When_CourseDeleted()
        {
            var course = new Domain.Entities.Course
                {
                    ActiveFlag = true
                };

            _mockCourseRepository.Setup(c => c.Load(It.IsAny<Guid>())).Returns(course);

            var courseId = Guid.NewGuid();
            _updateModelOnCourseDeletion.Handle(new CourseDeleted
                {
                    AggregateId = courseId
                });

            _mockCourseRepository.Verify(c => c.Load(courseId), Times.Once());
            _mockCourseRepository.Verify(c => c.Delete(course));
        }
    }
}
