using System;
using System.Collections.Generic;
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
                }) ;
        }

		// TODO: Move Guid comparison items into common
		[Test]
		public void Can_Compare_two_lists_of_guids_returns_false_when_lists_are_different()
		{
			var guid1 = Guid.NewGuid();
			var guidList1 = new List<Guid> { guid1, Guid.NewGuid() };
			var guidList2 = new List<Guid> { guid1, Guid.NewGuid() };

			var result = _courseUpdatedHandler.AreGuidListsTheSame(guidList1, guidList2);

			Assert.That(result, Is.False);
		}

		[Test]
		public void Can_Compare_two_lists_of_guids_returns_true_when_lists_are_same()
		{
			var guid1 = Guid.NewGuid();
			var guid2 = Guid.NewGuid();

			var guidList1 = new List<Guid> { guid1, guid2 };
			var guidList2 = new List<Guid> { guid1, guid2 };

			var result = _courseUpdatedHandler.AreGuidListsTheSame(guidList1, guidList2);

			Assert.That(result, Is.True);
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

		[Test]
		public void Raise_CourseInfoUpdatedEvent_When_Only_Prerequisites_Are_Changed()
		{
			var courseUpdatedEvent = CreateCourseUpdatedWithOnlyPrerequisitesChanged();
			_courseUpdatedHandler.Handle(courseUpdatedEvent);

			_mockDomainEvents.Verify(d => d.Raise<CourseAssociatedWithProgram>(It.IsAny<CourseAssociatedWithProgram>()),
									 Times.Never());
			_mockDomainEvents.Verify(d => d.Raise<CourseDisassociatedWithProgram>(It.IsAny<CourseDisassociatedWithProgram>()), Times.Never());
			_mockDomainEvents.Verify(d => d.Raise<CourseInfoUpdated>(It.IsAny<CourseInfoUpdated>()), Times.Once());
		}

        private CourseUpdated CreateCourseUpdatedEventWithNoCourseInfoUpdate()
        {
            var courseId = Guid.NewGuid();
            var programId = Guid.NewGuid();
            var organizationId = Guid.NewGuid();
	        var guid1 = Guid.NewGuid();
	        var guid2 = Guid.NewGuid();
			var prerequisiteCourseIds = new List<Guid> { guid1, guid2 };

            var saveCourseRequest = new SaveCourseRequest
                {
                    Code = "NewCode1",
                    Description = "NewDescription1",
                    Id = courseId,
                    Name = "NewName1",
                    OrganizationId = organizationId,
                    ProgramIds = new List<Guid> { programId },
					PrerequisiteCourseIds = prerequisiteCourseIds
                };

            var course = new Domain.Entities.Course
            {
                Code = "NewCode1",
                Description = "NewDescription1",
                Id = courseId,
                Name = "NewName1",
                OrganizationId = organizationId,
                Programs = new List<Program>
                        {
                            new Program { Id = programId }
                        },
				PrerequisiteCourses = new List<Domain.Entities.Course> {new Domain.Entities.Course {Id = guid2}, new Domain.Entities.Course {Id = guid1}}
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
            var organizationId = Guid.NewGuid();

            var saveCourseRequest = new SaveCourseRequest
            {
                Code = "NewCode1",
                Description = "NewDescription",
                Id = courseId,
                Name = "NewName1",
                OrganizationId = organizationId,
                ProgramIds = new List<Guid> { programId },
				PrerequisiteCourseIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
            };

            var course = new Domain.Entities.Course
            {
                Code = "OldCode1",
                Description = "OldDescription1",
                Id = courseId,
                Name = "OldName1",
                OrganizationId = organizationId,
                Programs = new List<Program>
                        {
                            new Program { Id = programId }
                        },
				PrerequisiteCourses = new List<Domain.Entities.Course> {new Domain.Entities.Course {Id = Guid.NewGuid()}}
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
            var organizationId = Guid.NewGuid();

            var saveCourseRequest = new SaveCourseRequest
            {
                Code = "NewCode1",
                Description = "NewDescription",
                Id = courseId,
                Name = "NewName1",
                OrganizationId = organizationId,
				ProgramIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() },
				PrerequisiteCourseIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
            };

            var course = new Domain.Entities.Course
            {
                Code = "OldCode1",
                Description = "OldDescription1",
                Id = courseId,
                Name = "OldName1",
                OrganizationId = organizationId,
                Programs = new List<Program>
                        {
                            new Program { Id = Guid.NewGuid() },
                            new Program { Id = Guid.NewGuid() },
                            new Program { Id = Guid.NewGuid() }
                        },
				PrerequisiteCourses = new List<Domain.Entities.Course> {new Domain.Entities.Course {Id = Guid.NewGuid()}}
            };

            return new CourseUpdated
            {
                AggregateId = Guid.NewGuid(),
                Request = saveCourseRequest,
                Old = course
            };

        }

		private CourseUpdated CreateCourseUpdatedWithOnlyPrerequisitesChanged()
		{
			var courseId = Guid.NewGuid();
			var programId = Guid.NewGuid();
			var organizationId = Guid.NewGuid();
			var guid1 = Guid.NewGuid();

			var saveCourseRequest = new SaveCourseRequest
			{
				Code = "NewCode1",
				Description = "NewDescription1",
				Id = courseId,
				Name = "NewName1",
				OrganizationId = organizationId,
				ProgramIds = new List<Guid> { programId },
				PrerequisiteCourseIds = new List<Guid> { guid1, Guid.NewGuid() }
			};

			var course = new Domain.Entities.Course
			{
				Code = "NewCode1",
				Description = "NewDescription1",
				Id = courseId,
				Name = "NewName1",
				OrganizationId = organizationId,
				Programs = new List<Program>
                        {
                            new Program { Id = programId }
                        },
				PrerequisiteCourses  = new List<Domain.Entities.Course> {new Domain.Entities.Course {Id = guid1}}
			};

			return new CourseUpdated
			{
				AggregateId = courseId,
				Request = saveCourseRequest,
				Old = course
			};

		}
    }
}
