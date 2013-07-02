using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using BpeProducts.Services.Course.Domain;
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
        [TestFixtureSetUp]
        public static void SetUpFixture()
        {
            MapperConfig.ConfigureMappers();
        }
        [Test]
        public void Throw_Exception_When_DomainEvent_Is_Not_CourseVersionCreated()
        {
            var autoMock = AutoMock.GetLoose();

            var updateModelOnCourseVersionCreation = autoMock.Create<UpdateModelOnCourseVersionCreation>();
            var exception =
                Assert.Throws<InvalidOperationException>(
                    () => updateModelOnCourseVersionCreation.Handle(new FakeDomainEvent()));
            Assert.That(exception.Message, Is.EqualTo("Invalid domain event."));
        }

        [Test]
        public void Create_course_version_in_repository()
        {
            var autoMock = AutoMock.GetLoose();
            var mockCourseRepository = autoMock.Mock<ICourseRepository>();

            var courseId = Guid.NewGuid();

            var courseVersionCreated = new CourseVersionCreated
                {
                    AggregateId = courseId,
                    NewVersion = new Domain.Entities.Course
                        {
                            Id = courseId,
                            ParentEntity = new Domain.Entities.Course(),
                            OriginalEntity = new Domain.Entities.Course(),
                            VersionNumber = "2.0a",
                            IsPublished = false
                        }
                };

            var updateModelOnCourseVersionCreation = autoMock.Create<UpdateModelOnCourseVersionCreation>();
            updateModelOnCourseVersionCreation.Handle(courseVersionCreated);

            mockCourseRepository.Verify(c => c.Save(courseVersionCreated.NewVersion), Times.Once());

        }

    }
}
