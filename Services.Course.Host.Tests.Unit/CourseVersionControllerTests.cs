using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Net.Http;
using System.Web.Http.Routing;
using Autofac.Extras.Moq;
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
        private Mock<ICourseRepository> _mockRepository;
        private Mock<IDomainEvents> _mockDomainEvents;
        private Mock<ICourseFactory> _mockCourseFactory;

        private CourseVersionController _courseVersionController;

        [SetUp]
        public void SetUp()
        {
            MapperConfig.ConfigureMappers();

            var autoMock = AutoMock.GetLoose();

            _mockRepository = autoMock.Mock<ICourseRepository>();
            _mockDomainEvents = autoMock.Mock<IDomainEvents>();
            _mockCourseFactory = autoMock.Mock<ICourseFactory>();

            _courseVersionController = autoMock.Create<CourseVersionController>();
        }

        [Test]
        public void Should_throw_not_found_exception_if_aggreate_root_is_not_found()
        {
            _mockCourseFactory.Setup(c => c.Reconstitute(It.IsAny<Guid>())).Returns((Domain.Entities.Course)null);

            var response =
                Assert.Throws<HttpResponseException>(
                    () => _courseVersionController.PublishVersion(Guid.NewGuid(), new PublishRequest())).Response;
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public void Can_publish_a_version()
        {
            var courseVersionId = Guid.NewGuid();
            var coursePublishRequest = new PublishRequest
                {
                    PublishNote = "blah blah"
                };

            _mockCourseFactory.Setup(c => c.Reconstitute(courseVersionId)).Returns(new Domain.Entities.Course());

            _courseVersionController.PublishVersion(courseVersionId, coursePublishRequest);

            _mockCourseFactory.Verify(c => c.Reconstitute(courseVersionId), Times.Once());
            _mockDomainEvents.Verify(d => d.Raise<CourseVersionPublished>(It.Is<CourseVersionPublished>(e => e.PublishNote.Equals(coursePublishRequest.PublishNote))));
        }

        [Test]
        [Ignore("Url.Link returns NullReferenceExeption. Don't know why...")]
        public void can_create_new_version()
        {
            SetUpApiController();

            var baseVersion = new Domain.Entities.Course();
            var newVersion = new Domain.Entities.Course();
            var courseVersionRequest = new VersionRequest
            {
                ParentVersionId = Guid.NewGuid(),
                VersionNumber = "1.2"
            };

            _mockRepository.Setup(r => r.Load(It.IsAny<Guid>())).Returns(baseVersion);
            _mockCourseFactory.Setup(c => c.BuildNewVersion(It.IsAny<Domain.Entities.Course>(), It.IsAny<string>()))
                              .Returns(newVersion);

            // _courseVersionController.CreateVersion("courses", courseVersionRequest);
            var response = _courseVersionController.CreateVersion(courseVersionRequest);
            var actual = response.Headers.Location;

            Assert.That(actual, Is.Not.Null);

            _mockRepository.Verify(r => r.Load(courseVersionRequest.ParentVersionId));
            _mockCourseFactory.Verify(c => c.BuildNewVersion(baseVersion, courseVersionRequest.VersionNumber));
            _mockDomainEvents.Verify(c => c.Raise<CourseVersionCreated>(
                It.Is<CourseVersionCreated>(p => p.AggregateId == newVersion.Id && p.NewVersion == newVersion)));
        }

        [Test]
        public void can_check_for_non_existing_parent_version()
        {
            var courses = new List<Domain.Entities.Course>();
            var parentVersionId = Guid.NewGuid();
            var courseVersionRequest = new VersionRequest
                {
                    ParentVersionId = parentVersionId,
                    VersionNumber = "1.0"
                };

            _mockRepository.Setup(r => r.Load(It.IsAny<Guid>())).Returns((Domain.Entities.Course)null);

            var response =
                Assert.Throws<HttpResponseException>(
                    () => _courseVersionController.CreateVersion(courseVersionRequest)).Response;
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
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
