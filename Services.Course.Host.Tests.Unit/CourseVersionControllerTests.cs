using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net.Http;
using System.Web.Http;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;
using BpeProducts.Services.Course.Host.App_Start;
using BpeProducts.Services.Course.Host.Controllers;
using Moq;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit
{
    [TestFixture]
    public class CourseVersionControllerTests
    {
        private Mock<IRepository> _mockRepository;
        private Mock<IDomainEvents> _mockDomainEvents;
        private Mock<ICourseFactory> _mockCourseFactory;

        private CourseVersionController _courseVersionController;

        [SetUp]
        public void SetUp()
        {
            MapperConfig.ConfigureMappers();

            _mockRepository = new Mock<IRepository>();
            _mockDomainEvents = new Mock<IDomainEvents>();
            _mockCourseFactory = new Mock<ICourseFactory>();

            _courseVersionController = new CourseVersionController(_mockRepository.Object, _mockDomainEvents.Object, _mockCourseFactory.Object);
        }

        [Test]
        public void Should_throw_not_found_exception_if_aggreate_root_is_not_found()
        {
            _mockCourseFactory.Setup(c => c.Reconstitute(It.IsAny<Guid>())).Returns((Domain.Entities.Course)null);

            var response =
                Assert.Throws<HttpResponseException>(
                    () => _courseVersionController.Publish(Guid.NewGuid(), new CoursePublishRequest())).Response;
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public void Can_publish_a_version()
        {
            var courseVersionId = Guid.NewGuid();
            var coursePublishRequest = new CoursePublishRequest
                {
                    PublishNote = "blah blah"
                };

            _mockCourseFactory.Setup(c => c.Reconstitute(courseVersionId)).Returns(new Domain.Entities.Course());

            _courseVersionController.Publish(courseVersionId, coursePublishRequest);

            _mockCourseFactory.Verify(c => c.Reconstitute(courseVersionId), Times.Once());
            _mockDomainEvents.Verify(d => d.Raise<CourseVersionPublished>(It.Is<CourseVersionPublished>(e => e.PublishNote.Equals(coursePublishRequest.PublishNote))));
        }

        [Test]
        public void can_create_new_version()
        {
            
        }

        [Test]
        public void can_check_for_missing_version_number()
        {
            
        }

        [Test]
        public void can_check_for_non_existing_parent_version()
        {
            
        }

        [Test]
        public void can_check_for_duplicate_version()
        {
            
        }
    }
}
