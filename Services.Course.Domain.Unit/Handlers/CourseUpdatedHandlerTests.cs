using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Handlers;
using Moq;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Domain.Unit.Handlers
{
    [TestFixture]
    public class CourseUpdatedHandlerTests
    {
        private Mock<IDomainEvents> _mockDomainEvents;
        private CourseUpdatedHandler _courseUpdatedHandler;

        [SetUp]
        public void SetUp()
        {
            _mockDomainEvents = new Mock<IDomainEvents>();
            _courseUpdatedHandler = new CourseUpdatedHandler(_mockDomainEvents.Object);
        }

        [Test]
        public void Raise_CourseAssociatedWithProgram_Event_When_New_Programs_Are_Added()
        {
            var courseUpdatedEvent = CreateCourseUpdatedEventWithNewAndOldPrograms();
            _courseUpdatedHandler.Handle(courseUpdatedEvent);

            _mockDomainEvents.Verify(d => d.Raise<CourseAssociatedWithProgram>(It.IsAny<CourseAssociatedWithProgram>()),
                                     Times.Exactly(3));
            _mockDomainEvents.Verify(d => d.Raise<CourseDisassociatedWithProgram>(It.IsAny<CourseDisassociatedWithProgram>()), Times.Exactly(3));
        }

        [Test]
        public void Does_Not_Raise_CourseAssociatedWithProgram_Event_When_No_New_Programs_Are_Added()
        {
            var courseUpdatedEvent = CreateCourseUpdatedEventWithNoNewPrograms();
            _courseUpdatedHandler.Handle(courseUpdatedEvent);

            _mockDomainEvents.Verify(d => d.Raise<CourseAssociatedWithProgram>(It.IsAny<CourseAssociatedWithProgram>()),
                                     Times.Never());
            _mockDomainEvents.Verify(d => d.Raise<CourseDisassociatedWithProgram>(It.IsAny<CourseDisassociatedWithProgram>()), Times.Never());
            _mockDomainEvents.Verify(d => d.Raise<CourseInfoUpdated>(It.IsAny<CourseInfoUpdated>()), Times.Once());
        }

        [Test]
        public void Does_Not_Raise_CourseInfoUpdatedEvent_When_No_Course_Info_Is_Updated()
        {
            var courseUpdatedEvent = CreateCourseUpdatedEventWithNoCourseInfoUpdate();
            _courseUpdatedHandler.Handle(courseUpdatedEvent);

            _mockDomainEvents.Verify(d => d.Raise<CourseAssociatedWithProgram>(It.IsAny<CourseAssociatedWithProgram>()),
                                     Times.Never());
            _mockDomainEvents.Verify(d => d.Raise<CourseDisassociatedWithProgram>(It.IsAny<CourseDisassociatedWithProgram>()), Times.Never());
            _mockDomainEvents.Verify(d => d.Raise<CourseInfoUpdated>(It.IsAny<CourseInfoUpdated>()), Times.Never());
        }

        private CourseUpdated CreateCourseUpdatedEventWithNoCourseInfoUpdate()
        {
            var courseId = Guid.NewGuid();
            var programId = Guid.NewGuid();

            var saveCourseRequest = new SaveCourseRequest
                {
                    Code = "NewCode1",
                    Description = "NewDescription1",
                    Id = courseId,
                    Name = "NewName1",
                    ProgramIds = new List<Guid> { programId }
                };

            var course = new Entities.Course
            {
                Code = "NewCode1",
                Description = "NewDescription1",
                Id = courseId,
                Name = "NewName1",
                Programs = new List<Program>
                        {
                            new Program { Id = programId }
                        }
            };

            return new CourseUpdated
            {
                AggregateId = courseId,
                Request = saveCourseRequest,
                Old = course
            };

        }

        private CourseUpdated CreateCourseUpdatedEventWithNoNewPrograms()
        {
            var courseId = Guid.NewGuid();
            var programId = Guid.NewGuid();

            var saveCourseRequest = new SaveCourseRequest
            {
                Code = "NewCode1",
                Description = "NewDescription",
                Id = courseId,
                Name = "NewName1",
                ProgramIds = new List<Guid> { programId }
            };

            var course = new Entities.Course
            {
                Code = "OldCode1",
                Description = "OldDescription1",
                Id = courseId,
                Name = "OldName1",
                Programs = new List<Program>
                        {
                            new Program { Id = programId }
                        }
            };

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

            var saveCourseRequest = new SaveCourseRequest
            {
                Code = "NewCode1",
                Description = "NewDescription",
                Id = courseId,
                Name = "NewName1",
                ProgramIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() }
            };

            var course = new Entities.Course
            {
                Code = "OldCode1",
                Description = "OldDescription1",
                Id = courseId,
                Name = "OldName1",
                Programs = new List<Program>
                        {
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
