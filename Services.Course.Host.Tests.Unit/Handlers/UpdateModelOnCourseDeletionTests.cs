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
        private Mock<IRepository> _mockRepository;
        private UpdateModelOnCourseDeletion _updateModelOnCourseDeletion;

        [SetUp]
        public void SetUp()
        {
            _mockRepository = new Mock<IRepository>();
            _updateModelOnCourseDeletion = new UpdateModelOnCourseDeletion(_mockRepository.Object);
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
            var course = new Domain.Courses.Course
                {
                    ActiveFlag = true
                };

            _mockRepository.Setup(c => c.Get<Domain.Courses.Course>(It.IsAny<Guid>())).Returns(course);

            var courseId = Guid.NewGuid();
            _updateModelOnCourseDeletion.Handle(new CourseDeleted
                {
                    AggregateId = courseId
                });

            _mockRepository.Verify(c => c.Get<Domain.Courses.Course>(courseId), Times.Once());
            _mockRepository.Verify(c => c.Save(It.Is<Domain.Courses.Course>(x => x.ActiveFlag == false)));
        }
    }
}
