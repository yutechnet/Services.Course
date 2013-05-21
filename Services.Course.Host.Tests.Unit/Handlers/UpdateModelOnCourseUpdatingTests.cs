using System;
using System.Collections.Generic;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Handlers;
using BpeProducts.Services.Course.Domain.Repositories;
using BpeProducts.Services.Course.Host.App_Start;
using Moq;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit.Handlers
{
    [TestFixture]
    public class UpdateModelOnCourseUpdatingTests
    {
        private Mock<ICourseRepository> _mockCourseRepository;
        private UpdateModelOnCourseUpdating _updateModelOnCourseUpdating;

        [TestFixtureSetUp]
        public static void SetUpFixture()
        {
            MapperConfig.ConfigureMappers();
        }

        [SetUp]
        public void SetUp()
        {
            _mockCourseRepository = new Mock<ICourseRepository>();
            _updateModelOnCourseUpdating = new UpdateModelOnCourseUpdating(_mockCourseRepository.Object);
        }

        [Test]
        public void Throw_Exception_When_DomainEvent_Is_Not_CourseUpdated()
        {
            var exception =
                Assert.Throws<InvalidOperationException>(
                    () => _updateModelOnCourseUpdating.Handle(new FakeDomainEvent()));
            Assert.That(exception.Message, Is.EqualTo("Invalid domain event."));
        }

        [Test]
        public void Update_Course_In_Repository()
        {
            var courseUpdatedEvent = CreateCourseUpdatedEventWithNewAndOldPrograms();

            _mockCourseRepository.Setup(c => c.GetById(It.IsAny<Guid>())).Returns(courseUpdatedEvent.Old);
            _mockCourseRepository.Setup(c => c.Load<Program>(It.IsAny<Guid>())).Returns(new Program());

            _updateModelOnCourseUpdating.Handle(courseUpdatedEvent);

            _mockCourseRepository.Verify(c => c.GetById(courseUpdatedEvent.AggregateId), Times.Once());
            _mockCourseRepository.Verify(c => c.Update(It.Is<Domain.Entities.Course>(x => x.Name == courseUpdatedEvent.Request.Name 
                && x.Description == courseUpdatedEvent.Request.Description
                && x.Code == courseUpdatedEvent.Request.Code
                && x.Programs.Count == courseUpdatedEvent.Request.ProgramIds.Count)), Times.Once());
        }

        private CourseUpdated CreateCourseUpdatedEventWithNewAndOldPrograms()
        {
            var courseId = Guid.NewGuid();

            var saveCourseRequest = new SaveCourseRequest
            {
                Code = "NewCode1",
                Description = "NewDescription",
                Id = courseId,
                Name = "NewName1",
                ProgramIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() }
            };

            var course = new Domain.Entities.Course
            {
                Code = "OldCode1",
                Description = "OldDescription1",
                Id = courseId,
                Name = "OldName1",
                Programs = new List<Program>
                        {
                            new Program { Id = Guid.NewGuid() },
                            new Program { Id = Guid.NewGuid() },
                            new Program { Id = Guid.NewGuid() },
                            new Program { Id = Guid.NewGuid() },
                            new Program { Id = Guid.NewGuid() }
                        }
            };

            return new CourseUpdated
            {
                AggregateId = Guid.NewGuid(),
                Request = saveCourseRequest,
                Old = course
            };

        }

    }
}
