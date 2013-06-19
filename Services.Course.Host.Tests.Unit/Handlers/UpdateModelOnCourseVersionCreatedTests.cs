using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Handlers;
using BpeProducts.Services.Course.Domain.Repositories;
using BpeProducts.Services.Course.Host.App_Start;
using Moq;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit.Handlers
{
    [TestFixture]
    public class UpdateModelOnCourseVersionCreatedTests
    {
        private Mock<ICourseRepository> _mockCourseRepository;
        private UpdateModelOnCourseVersionCreation _updateModelOnCourseVersionCreation;

        [TestFixtureSetUp]
        public static void SetUpFixture()
        {
            MapperConfig.ConfigureMappers();
        }

        [SetUp]
        public void SetUp()
        {
            _mockCourseRepository = new Mock<ICourseRepository>();
            _updateModelOnCourseVersionCreation = new UpdateModelOnCourseVersionCreation(_mockCourseRepository.Object);
        }

        [Test]
        public void Throw_Exception_When_DomainEvent_Is_Not_CourseVersionCreated()
        {
            var exception =
                Assert.Throws<InvalidOperationException>(
                    () => _updateModelOnCourseVersionCreation.Handle(new FakeDomainEvent()));
            Assert.That(exception.Message, Is.EqualTo("Invalid domain event."));
        }

        [Test]
        public void Create_course_version_in_repository()
        {
            var originalCourseId = Guid.NewGuid();
            var parentVersionId = Guid.NewGuid();
            var newVersionId = Guid.NewGuid();

            var courseVersionCreated = new CourseVersionCreated
                {
                    AggregateId = newVersionId,
                    IsPublished = false,
                    OriginalCourseId = originalCourseId,
                    ParentCourseId = parentVersionId,
                    VersionNumber = "1.0.0.0",
                };

            _mockCourseRepository.Setup(c => c.GetById(parentVersionId))
                                 .Returns(new Domain.Entities.Course
                                     {
                                         OriginalEntityId = originalCourseId,
                                         Id = parentVersionId,
                                         IsPublished = true
                                     });

            _updateModelOnCourseVersionCreation.Handle(courseVersionCreated);

            _mockCourseRepository.Verify(c => c.GetById(parentVersionId), Times.Once());
            _mockCourseRepository.Verify(c => c.Add(It.Is<Domain.Entities.Course>(x => 
                x.OriginalEntityId == originalCourseId &&
                x.ParentEntityId == parentVersionId &&
                x.VersionNumber.Equals(courseVersionCreated.VersionNumber))), Times.Once());

        }

    }
}
