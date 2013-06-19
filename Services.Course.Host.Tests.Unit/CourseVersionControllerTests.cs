using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
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
        [Ignore("Url.Link returns NullReferenceExeption. Don't know why...")]
        public void can_create_new_version()
        {
            SetUpApiController();

            var originalCourseId = Guid.NewGuid();
            var parentVersionId = Guid.NewGuid();
            var courses = new List<Domain.Entities.Course>
                {
                    new Domain.Entities.Course
                        {
                            Id = originalCourseId,
                            OriginalEntityId = originalCourseId,
                            ActiveFlag = true,
                            VersionNumber = "1.0"
                        },
                    new Domain.Entities.Course
                        {
                            Id = parentVersionId,
                            OriginalEntityId = originalCourseId,
                            ParentEntityId = originalCourseId,
                            ActiveFlag = true,
                            VersionNumber = "1.1"
                        },
                };
            var courseVersionRequest = new CourseVersionRequest
            {
                ParentVersionId = parentVersionId,
                VersionNumber = "1.2"
            };

            _mockRepository.Setup(c => c.Query<Domain.Entities.Course>()).Returns(courses.AsQueryable);

            _courseVersionController.CreateVersion(courseVersionRequest);
            var response = _courseVersionController.CreateVersion(courseVersionRequest);
            var actual = response.Headers.Location;

            Assert.That(actual, Is.Not.Null);

            _mockDomainEvents.Verify(c => c.Raise<CourseVersionCreated>(
                It.Is<CourseVersionCreated>(p => p.IsPublished == false &&
                    p.OriginalCourseId == originalCourseId &&
                    p.ParentCourseId == parentVersionId &&
                    p.VersionNumber == courseVersionRequest.VersionNumber
                    )), Times.Once());

        }

        [Test]
        public void can_check_for_non_existing_parent_version()
        {
            var courses = new List<Domain.Entities.Course>();
            var parentVersionId = Guid.NewGuid();
            var courseVersionRequest = new CourseVersionRequest
                {
                    ParentVersionId = parentVersionId,
                    VersionNumber = "1.0"
                };

            _mockRepository.Setup(c => c.Query<Domain.Entities.Course>()).Returns(courses.AsQueryable);

            var response =
                Assert.Throws<HttpResponseException>(
                    () => _courseVersionController.CreateVersion(courseVersionRequest)).Response;
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public void can_check_for_duplicate_version()
        {
            var originalCourseId = Guid.NewGuid();
            var parentVersionId = Guid.NewGuid();
            var courses = new List<Domain.Entities.Course>
                {
                    new Domain.Entities.Course
                        {
                            Id = originalCourseId,
                            OriginalEntityId = originalCourseId,
                            ActiveFlag = true,
                            VersionNumber = "1.0"
                        },
                    new Domain.Entities.Course
                        {
                            Id = parentVersionId,
                            OriginalEntityId = originalCourseId,
                            ParentEntityId = originalCourseId,
                            ActiveFlag = true,
                            VersionNumber = "1.1"
                        },
                };
            var courseVersionRequest = new CourseVersionRequest
            {
                ParentVersionId = parentVersionId,
                VersionNumber = "1.1"
            };

            _mockRepository.Setup(c => c.Query<Domain.Entities.Course>()).Returns(courses.AsQueryable);

            var response =
                Assert.Throws<HttpResponseException>(
                    () => _courseVersionController.CreateVersion(courseVersionRequest)).Response;
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
        }

        private void SetUpApiController()
        {
            var configuration = new HttpConfiguration();
            // Register the route
            WebApiConfig.Register(configuration);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://localhost/Courses"),
                Content = new HttpMessageContent(new HttpRequestMessage(HttpMethod.Post, "http://localhost/courses"))
            };

            _courseVersionController.Request = request;
            _courseVersionController.Request.Properties["MS_HttpConfiguration"] = configuration;
            _courseVersionController.Url = new UrlHelper(_courseVersionController.Request);
        }


    }
}
