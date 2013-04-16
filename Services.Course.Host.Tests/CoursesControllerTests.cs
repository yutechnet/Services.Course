using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using AutoMapper;
using BpeProducts.Common.NHibernate;
using BpeProducts.Common.WebApi;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Repositories;
using BpeProducts.Services.Course.Host.App_Start;
using BpeProducts.Services.Course.Host.Controllers;
using Moq;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests
{
    [TestFixture]
    public class CoursesControllerTests
    {
        private Mock<ICourseRepository> _mockCourseRepository;

        private CoursesController _coursesController;

        [SetUp]
        public void SetUp()
        {
            MapperConfig.ConfigureMappers();

            _mockCourseRepository = new Mock<ICourseRepository>();
            _coursesController = new CoursesController(_mockCourseRepository.Object);
        }

        [Test]
        public void can_create_a_new_course()
        {
            SetUpApiController();

            var saveCourseRequest = new SaveCourseRequest
                {
                    Id = Guid.NewGuid(),
                    Code = "PSY101",
                    Description = "Psych!",
                    Name = "Psychology 101"
                };

            var course = new Domain.Entities.Course
                {
                    ActiveFlag = true,
                    Code = saveCourseRequest.Code,
                    Description = saveCourseRequest.Description,
                    Name = saveCourseRequest.Name,
                    Id = saveCourseRequest.Id
                };

            _mockCourseRepository.Setup(c => c.Add(It.IsAny<Domain.Entities.Course>())).Returns(saveCourseRequest.Id);
            _mockCourseRepository.Setup(c => c.GetById(It.IsAny<Guid>())).Returns(course);

            var response = _coursesController.Post(saveCourseRequest);
            var actual = response.Content.ReadAsAsync<CourseInfoResponse>().Result;

            //_mockCourseRepository.Verify(c => c.Add(It.IsAny<Domain.Entities.Course>()), Times.Once());
            _mockCourseRepository.Verify(c => c.Add(
                It.Is<Domain.Entities.Course>(p => p.Code.Equals("PSY101") &&
                                                   p.Description.Equals("Psych!") &&
                                                   p.Name.Equals("Psychology 101") &&
                                                   p.ActiveFlag.Equals(true))), Times.Once());
            Assert.That(actual.Id.Equals(saveCourseRequest.Id));

        }

        [Test]
        public void can_update_an_existing_course()
        {
            Mapper.CreateMap<Domain.Entities.Course, SaveCourseRequest>();

            var courses = SetUpExistingCourse();
            _mockCourseRepository.Setup(c => c.GetById(It.IsAny<Guid>())).Returns(courses.First());

            var saveCourseRequest = Mapper.Map<SaveCourseRequest>(courses.First());

            _coursesController.Put(saveCourseRequest.Id, saveCourseRequest);

            _mockCourseRepository.Verify(c => c.Update(
                It.Is<Domain.Entities.Course>(p => p.Id.Equals(saveCourseRequest.Id) &&
                                                   p.Code.Equals(saveCourseRequest.Code) &&
                                                   p.Description.Equals(saveCourseRequest.Description) &&
                                                   p.Name.Equals(saveCourseRequest.Name))), Times.Once());
        }

        [Test]
        public void cannot_update_non_existing_course()
        {
            Mapper.CreateMap<Domain.Entities.Course, SaveCourseRequest>();

            var saveCourseRequest = new SaveCourseRequest();

            var response =
                Assert.Throws<HttpResponseException>(() => _coursesController.Put(Guid.NewGuid(), saveCourseRequest)).Response;
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

        }

        [Test]
        public void can_get_list_of_course()
        {
            var courses = SetUpExistingCourse();

            var courseInfoList = _coursesController.Get();
            Assert.That(courseInfoList.Count() == courses.Count());
        }

        [Test]
        public void can_delete_a_course()
        {
            _mockCourseRepository.Setup(c => c.GetById(It.IsAny<Guid>())).Returns(new Domain.Entities.Course());
            var courses = SetUpExistingCourse();

            var id = courses.FirstOrDefault().Id;
            _coursesController.Delete(id);

            _mockCourseRepository.Verify(c => c.Update(It.Is<Domain.Entities.Course>(d => d.ActiveFlag == false)), Times.Once());
        }

        [Test]
        public void should_throw_exception_deleting_non_existing_course()
        {
            SetUpExistingCourse();
            var id = Guid.NewGuid();

            var response =
                Assert.Throws<HttpResponseException>(() => _coursesController.Delete(id)).Response;
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public void can_search_course_by_name_name()
        {
            var courses = SetUpExistingCourse();
            var course = courses.FirstOrDefault(c => c.Name.Equals("Psychology 101"));

            var courseInfo = _coursesController.GetByName(course.Name);
            Assert.That(courseInfo.Name.Equals(course.Name));
            Assert.That(courseInfo.Code.Equals(course.Code));
        }

        [Test]
        public void can_search_course_by_code()
        {
            var courses = SetUpExistingCourse();
            var course = courses.FirstOrDefault(c => c.Code.Equals("PSY102"));

            var courseInfo = _coursesController.GetByName(course.Name);
            Assert.That(courseInfo.Name.Equals(course.Name));
            Assert.That(courseInfo.Code.Equals(course.Code));
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

            _coursesController.Request = request;
            _coursesController.Request.Properties["MS_HttpConfiguration"] = configuration;
            _coursesController.Url = new UrlHelper(_coursesController.Request);
        }

        private IEnumerable<Domain.Entities.Course> SetUpExistingCourse()
        {
            var courses = new List<Domain.Entities.Course>
                {
                    new Domain.Entities.Course
                        {
                            Id = Guid.NewGuid(),
                            Code = "PSY101",
                            Description = "Psych!",
                            Name = "Psychology 101",
                            ActiveFlag = true
                        },
                    new Domain.Entities.Course
                        {
                            Id = Guid.NewGuid(),
                            Code = "PSY102",
                            Description = "Psych!",
                            Name = "Psychology 102",
                            ActiveFlag = true
                        },
                    new Domain.Entities.Course
                        {
                            Id = Guid.NewGuid(),
                            Code = "PSY103",
                            Description = "Psych!",
                            Name = "Psychology 103",
                            ActiveFlag = true
                        }
                };
            _mockCourseRepository.Setup(t => t.GetAll()).Returns(courses.AsQueryable());

            return courses;
        }
    }
}
