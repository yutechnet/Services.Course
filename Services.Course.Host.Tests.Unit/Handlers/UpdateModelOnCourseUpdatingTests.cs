using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
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
        private Mock<IProgramRepository> _mockProgramRepository;
        private UpdateModelOnCourseUpdating _updateModelOnCourseUpdating;

        [TestFixtureSetUp]
        public static void SetUpFixture()
        {
            MapperConfig.ConfigureMappers();
        }

        [SetUp]
        public void SetUp()
        {
            var autoMock = AutoMock.GetLoose();
            _mockCourseRepository = autoMock.Mock<ICourseRepository>();
            _mockProgramRepository = autoMock.Mock<IProgramRepository>();
            _updateModelOnCourseUpdating = autoMock.Create<UpdateModelOnCourseUpdating>();
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

            _mockCourseRepository.Setup(c => c.Load(It.IsAny<Guid>())).Returns(courseUpdatedEvent.Old);
            _mockProgramRepository.Setup(c => c.Get(It.IsAny<Guid>())).Returns(new Program());
            _mockProgramRepository.Setup(p => p.Get(It.IsAny<List<Guid>>())).Returns(new List<Program>
                {
                    new Program { Id = courseUpdatedEvent.Request.ProgramIds[0] },
                    new Program { Id = courseUpdatedEvent.Request.ProgramIds[1] },
                    new Program { Id = courseUpdatedEvent.Request.ProgramIds[2] }
                });

            _updateModelOnCourseUpdating.Handle(courseUpdatedEvent);

            _mockCourseRepository.Verify(c => c.Load(courseUpdatedEvent.AggregateId), Times.Once());
            _mockCourseRepository.Verify(
                c => c.Save(It.Is<Domain.Entities.Course>(x => x.Name == courseUpdatedEvent.Request.Name
                                                               &&
                                                               x.Description == courseUpdatedEvent.Request.Description
                                                               && x.Code == courseUpdatedEvent.Request.Code
                                                               &&
                                                               x.Programs.Count ==
                                                               courseUpdatedEvent.Request.ProgramIds.Count)),
                Times.Once());
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
                    ProgramIds = new List<Guid> {Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()}
                };

            var course = new Domain.Entities.Course
                {
                    Code = "OldCode1",
                    Description = "OldDescription1",
                    Id = courseId,
                    Name = "OldName1",
                    Programs = new List<Program>
                        {
                            new Program {Id = Guid.NewGuid()},
                            new Program {Id = Guid.NewGuid()},
                            new Program {Id = Guid.NewGuid()},
                            new Program {Id = Guid.NewGuid()},
                            new Program {Id = Guid.NewGuid()}
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
