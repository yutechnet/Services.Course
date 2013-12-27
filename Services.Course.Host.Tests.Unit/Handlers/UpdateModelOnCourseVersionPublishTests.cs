using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Course.Domain.Courses;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Handlers;
using BpeProducts.Services.Course.Domain.Repositories;
using BpeProducts.Services.Course.Domain.Validation;
using Moq;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit.Handlers
{
    [TestFixture]
    public class UpdateModelOnCourseVersionPublishTests
    {
        private Mock<IRepository> _mcokRepository;
        private UpdateModelOnEntityVersionPublish _updateModelOnCourseVersionPublish;
	    private AutoMock _autoMock;

	    [SetUp]
        public void SetUp()
        {
            _mcokRepository = new Mock<IRepository>();
            _updateModelOnCourseVersionPublish = new UpdateModelOnEntityVersionPublish(_mcokRepository.Object,new Mock<ICoursePublisher>().Object);
	        _autoMock = AutoMock.GetLoose();
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
            var courseVersionPublished = new VersionPublished
                {
                    AggregateId = courseVersionId,
                    PublishNote = "Blah blah"
                };

	        var courseToReturn = _autoMock.Create<Course.Domain.Courses.Course>();
	        courseToReturn.Id = courseVersionId;
            _mcokRepository.Setup(c => c.Get<Domain.Courses.Course>(courseVersionId)).Returns(courseToReturn);

            _updateModelOnCourseVersionPublish.Handle(courseVersionPublished);

            //_mcokRepository.Verify(c => c.Get<Domain.Entities.Course>(courseVersionId), Times.Once());
            //_mcokRepository.Verify(c => c.Save(It.Is<VersionableEntity>(x =>
            //    x.IsPublished &&
            //    x.PublishNote.Equals(courseVersionPublished.PublishNote))), Times.Once());
        }
    }
}
