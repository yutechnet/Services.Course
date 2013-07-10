using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Handlers;
using BpeProducts.Services.Course.Domain.Repositories;
using Moq;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit.Handlers
{
    [TestFixture]
    public class UpdateModelOnCourseVersionPublishTests
    {
        private Mock<ICourseRepository> _mockCourseRepository;
        private UpdateModelOnCourseVersionPublish _updateModelOnCourseVersionPublish;

        [SetUp]
        public void SetUp()
        {
            _mockCourseRepository = new Mock<ICourseRepository>();
            _updateModelOnCourseVersionPublish = new UpdateModelOnCourseVersionPublish(_mockCourseRepository.Object);
        }

        [Test]
        public void Throw_Exception_When_DomainEvent_Is_Not_CourseVersionPublished()
        {
            var exception =
                Assert.Throws<InvalidOperationException>(
                    () => _updateModelOnCourseVersionPublish.Handle(new FakeDomainEvent()));
            Assert.That(exception.Message, Is.EqualTo("Invalid domain event."));
        }

        [Test]
        public void Create_published_version_in_repository()
        {
            var courseVersionId = Guid.NewGuid();
            var courseVersionPublished = new CourseVersionPublished
                {
                    AggregateId = courseVersionId,
                    PublishNote = "Blah blah"
                };

            _mockCourseRepository.Setup(c => c.Get(courseVersionId)).Returns(new Domain.Entities.Course
                {
                    Id = courseVersionId,
                    IsPublished = false,
                    PublishNote = null
                });

            _updateModelOnCourseVersionPublish.Handle(courseVersionPublished);

            _mockCourseRepository.Verify(c => c.Get(courseVersionId), Times.Once());
            _mockCourseRepository.Verify(c => c.Save(It.Is<Domain.Entities.Course>(x =>
                x.IsPublished &&
                x.PublishNote.Equals(courseVersionPublished.PublishNote))), Times.Once());
        }
    }
}
