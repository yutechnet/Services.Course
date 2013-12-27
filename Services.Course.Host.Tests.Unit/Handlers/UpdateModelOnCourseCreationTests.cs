using System;
using System.Collections.Generic;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain;
using BpeProducts.Services.Course.Domain.Courses;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Handlers;
using BpeProducts.Services.Course.Domain.Repositories;
using Moq;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit.Handlers
{
    [TestFixture]
    public class UpdateModelOnCourseCreationTests
    {
        private UpdateModelOnCourseCreation _updateModelOnCourseCreation;
        private Mock<IRepository> _mockRepository;
            
        [SetUp]
        public void SetUp()
        {
            _mockRepository = new Mock<IRepository>();
            _updateModelOnCourseCreation = new UpdateModelOnCourseCreation(_mockRepository.Object);
        }

        [Test]
        public void Throw_Exception_When_Domain_Event_Is_Not_CourseCreated()
        {
            var exception =
                Assert.Throws<InvalidOperationException>(
                    () => _updateModelOnCourseCreation.Handle(new FakeDomainEvent()));
            Assert.That(exception.Message, Is.EqualTo("Invalid domain event."));
        }

        [Test]
        public void Add_New_Course_To_Repository()
        {
            var course = new Domain.Courses.Course();
            _updateModelOnCourseCreation.Handle(new CourseCreated { Course = course });

            _mockRepository.Verify(c => c.Save(course), Times.Once());
        }
    }
}
