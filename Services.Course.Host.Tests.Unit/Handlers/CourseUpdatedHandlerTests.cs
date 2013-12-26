using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Handlers;
using BpeProducts.Services.Course.Domain.Repositories;
using Moq;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit.Handlers
{
    [TestFixture]
    public class CourseUpdatedHandlerTests
    {
        private Mock<IDomainEvents> _mockDomainEvents;
        private Mock<IRepository> _mockRepository;
        private CourseUpdatedHandler _courseUpdatedHandler;
	    private AutoMock _autoMock;

	    [SetUp]
        public void SetUp()
        {
            _mockDomainEvents = new Mock<IDomainEvents>();
            _mockRepository = new Mock<IRepository>();
            _courseUpdatedHandler = new CourseUpdatedHandler(_mockDomainEvents.Object, _mockRepository.Object);

            _mockRepository.Setup(p => p.Get<Domain.Entities.Program>(It.IsAny<Guid>())).Returns(new Program
                {
                    Id = Guid.NewGuid(),
                    Name = "program name",
                    Description = "program description",
                    ProgramType = "traditional"
                });

	        _autoMock = AutoMock.GetLoose();
        }

        [Test]
        public void Raise_CourseAssociatedWithProgram_Event_When_New_Programs_Are_Added()
        {
            var courseUpdatedEvent = CreateCourseUpdatedEventWithNewAndOldPrograms();
            _courseUpdatedHandler.Handle(courseUpdatedEvent);

            _mockDomainEvents.Verify(d => d.Raise<CourseAssociatedWithProgram>(It.IsAny<CourseAssociatedWithProgram>()),
                                     Times.Exactly(3));
            _mockDomainEvents.Verify(
                d => d.Raise<CourseDisassociatedWithProgram>(It.IsAny<CourseDisassociatedWithProgram>()),
                Times.Exactly(3));
        }

        [Test]
        public void
            Does_Not_Raise_CourseAssociatedWithProgram_Or_CoursePrereq_Events_When_No_Programs_Are_Modified()
        {
            var courseUpdatedEvent = CreateCourseUpdatedEventWithNoNewProgramsOrPrereqs();
            _courseUpdatedHandler.Handle(courseUpdatedEvent);

            _mockDomainEvents.Verify(d => d.Raise<CourseAssociatedWithProgram>(It.IsAny<CourseAssociatedWithProgram>()),
                                     Times.Never());
            _mockDomainEvents.Verify(
                d => d.Raise<CourseDisassociatedWithProgram>(It.IsAny<CourseDisassociatedWithProgram>()), Times.Never());
            _mockDomainEvents.Verify(d => d.Raise<CourseInfoUpdated>(It.IsAny<CourseInfoUpdated>()), Times.Once());
        }

        [Test]
        public void Does_Not_Raise_CourseInfoUpdatedEvent_When_No_Course_Info_Is_Updated()
        {
            var courseUpdatedEvent = CreateCourseUpdatedEventWithNoCourseInfoUpdate();
            _courseUpdatedHandler.Handle(courseUpdatedEvent);

            _mockDomainEvents.Verify(d => d.Raise<CourseAssociatedWithProgram>(It.IsAny<CourseAssociatedWithProgram>()),
                                     Times.Never());
            _mockDomainEvents.Verify(
                d => d.Raise<CourseDisassociatedWithProgram>(It.IsAny<CourseDisassociatedWithProgram>()), Times.Never());
            _mockDomainEvents.Verify(d => d.Raise<CourseInfoUpdated>(It.IsAny<CourseInfoUpdated>()), Times.Never());
        }

        private CourseUpdated CreateCourseUpdatedEventWithNoCourseInfoUpdate()
        {
            var courseId = Guid.NewGuid();
            var programId = Guid.NewGuid();
            var organizationId = Guid.NewGuid();
            var guid1 = Guid.NewGuid();
            var guid2 = Guid.NewGuid();

            var saveCourseRequest = new UpdateCourseRequest
                {
                    Code = "NewCode1",
                    Description = "NewDescription1",
                    Name = "NewName1",
                    ProgramIds = new List<Guid> {programId}
                };

	        var course = _autoMock.Create<Course.Domain.Courses.Course>();
	        course.Code = "NewCode1";
	        course.Description = "NewDescription1";
	        course.Id = courseId;
	        course.Name = "NewName1";
	        course.OrganizationId = organizationId;
                

            course.SetPrograms(new List<Program>
                {
                    new Program {Id = programId}
                });

            return new CourseUpdated
                {
                    AggregateId = courseId,
                    Request = saveCourseRequest,
                    Old = course
                };

        }

        private CourseUpdated CreateCourseUpdatedEventWithNoNewProgramsOrPrereqs()
        {
            var courseId = Guid.NewGuid();
            var programId = Guid.NewGuid();
            var organizationId = Guid.NewGuid();

            var saveCourseRequest = new UpdateCourseRequest
                {
                    Code = "NewCode1",
                    Description = "NewDescription",
                    Name = "NewName1",
                    ProgramIds = new List<Guid> {programId}
                };

         var course = _autoMock.Create<Course.Domain.Courses.Course>();
	        course.Code = "OldCode1";
	        course.Description = "OldDescription1";
	        course.Id = courseId;
	        course.Name = "OldName1";
	        course.OrganizationId = organizationId;
           

            course.SetPrograms(new List<Program>
                {
                    new Program {Id = programId}
                });

            return new CourseUpdated
                {
                    AggregateId = courseId,
                    Request = saveCourseRequest,
                    Old = course
                };

        }

        private CourseUpdated CreateCourseUpdatedEventWithNewAndOldPrograms()
        {
            var courseId = Guid.NewGuid();
            var organizationId = Guid.NewGuid();

            var saveCourseRequest = new UpdateCourseRequest
                {
                    Code = "NewCode1",
                    Description = "NewDescription",
                    Name = "NewName1",
                    ProgramIds = new List<Guid> {Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()}
                };

            var course = _autoMock.Create<Course.Domain.Courses.Course>();
	        course.Code = "OldCode1";
	        course.Description = "OldDescription1";
	        course.Id = courseId;
	        course.Name = "OldName1";
	        course.OrganizationId = organizationId;

	        course.SetPrograms(new List<Program>
                {
                    new Program {Id = Guid.NewGuid()},
                    new Program {Id = Guid.NewGuid()},
                    new Program {Id = Guid.NewGuid()}
                });

            return new CourseUpdated
                {
                    AggregateId = Guid.NewGuid(),
                    Request = saveCourseRequest,
                    Old = course
                };

        }

        private CourseUpdated CreateCourseUpdatedWithThreePrerequisitesAddedAndTwoRemoved()
        {
            var courseId = Guid.NewGuid();
            var programId = Guid.NewGuid();
            var organizationId = Guid.NewGuid();
            var guid1 = Guid.NewGuid();

            var saveCourseRequest = new UpdateCourseRequest
                {
                    Code = "NewCode1",
                    Description = "NewDescription1",
                    Name = "NewName1",
                    ProgramIds = new List<Guid> {programId},
                    PrerequisiteCourseIds = new List<Guid>
                        {
                            Guid.NewGuid(),
                            guid1,
                            Guid.NewGuid(),
                            Guid.NewGuid()
                        }
                };

			var course = _autoMock.Create<Course.Domain.Courses.Course>();
	        course.Code = "NewCode1";
	        course.Description = "NewDescription1";
	        course.Id = courseId;
	        course.Name = "NewName1";
	        course.OrganizationId = organizationId;

	        course.SetPrograms(new List<Program>
                {
                    new Program {Id = programId}
                });

            return new CourseUpdated
                {
                    AggregateId = courseId,
                    Request = saveCourseRequest,
                    Old = course
                };
        }
    }
}
