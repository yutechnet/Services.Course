using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using BpeProducts.Common.NHibernate;
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
	    private AutoMock _autoMock;

	    [TestFixtureSetUp]
        public static void SetUpFixture()
        {
            MapperConfiguration.Configure();
        }
		[SetUp]
		public void SetUp()
		{
			_autoMock = AutoMock.GetLoose();
		}
        [Test]
        public void Throw_Exception_When_DomainEvent_Is_Not_CourseVersionCreated()
        {
            

            var updateModelOnCourseVersionCreation = _autoMock.Create<UpdateModelOnCourseVersionCreation>();
            var exception =
                Assert.Throws<InvalidOperationException>(
                    () => updateModelOnCourseVersionCreation.Handle(new FakeDomainEvent()));
            Assert.That(exception.Message, Is.EqualTo("Invalid domain event."));
        }

        [Test]
        public void Create_course_version_in_repository()
        {
            var autoMock = AutoMock.GetLoose();
            var mockRepository = autoMock.Mock<IRepository>();

            var courseId = Guid.NewGuid();
	        var course = _autoMock.Create<Course.Domain.Courses.Course>();
	        var newVersion=course.CreateVersion("2.0a");

            var courseVersionCreated = new VersionCreated
                {
                    AggregateId = courseId,
                    NewVersion = newVersion
                };

            var updateModelOnCourseVersionCreation = autoMock.Create<UpdateModelOnCourseVersionCreation>();
            updateModelOnCourseVersionCreation.Handle(courseVersionCreated);

            mockRepository.Verify(c => c.Save(courseVersionCreated.NewVersion), Times.Once());

        }

    }
}
