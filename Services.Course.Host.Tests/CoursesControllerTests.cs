using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
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
            var saveCourseRequest = new SaveCourseRequest
                {
                    Code = "PSY101",
                    Description = "Psych!",
                    Name = "Psychology 101"
                };

            var expected = Guid.NewGuid();

            _mockCourseRepository.Setup(c => c.Add(It.IsAny<Domain.Entities.Course>())).Returns(expected);

            var actual = _coursesController.Post(saveCourseRequest);

            //_mockCourseRepository.Verify(c => c.Add(It.IsAny<Domain.Entities.Course>()), Times.Once());
            _mockCourseRepository.Verify(c => c.Add(
                It.Is<Domain.Entities.Course>(p => p.Code.Equals("PSY101") &&
                                                   p.Description.Equals("Psych!") &&
                                                   p.Name.Equals("Psychology 101"))), Times.Once());
            Assert.That(actual.Equals(expected));

        }

        [Test]
        public void should_prevent_a_course_creation_if_existing_course_with_same_name_or_code_exist()
        {
            var courses = SetUpExistingCourse();

            var saveCourseRequest = new SaveCourseRequest
            {
                Code = "PSY101",
                Description = "Psych!",
                Name = "Test 101"
            };

            var response =
                Assert.Throws<HttpResponseException>(() => _coursesController.Post(saveCourseRequest)).Response;
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));

            saveCourseRequest = new SaveCourseRequest
            {
                Code = "ENG101",
                Description = "Psych!",
                Name = "Psychology 101"
            };

            response =
                Assert.Throws<HttpResponseException>(() => _coursesController.Post(saveCourseRequest)).Response;
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
        }

        [Test]
        public void can_update_an_existing_course()
        {
            Mapper.CreateMap<Domain.Entities.Course, SaveCourseRequest>();

            var courses = SetUpExistingCourse();

            var saveCourseRequest = Mapper.Map<SaveCourseRequest>(courses.First());

            _coursesController.Put(saveCourseRequest.Id, saveCourseRequest);

            _mockCourseRepository.Verify(c => c.Update(
                It.Is<Domain.Entities.Course>(p => p.Id.Equals(saveCourseRequest.Id) &&
                                                   p.Code.Equals(saveCourseRequest.Code) &&
                                                   p.Description.Equals(saveCourseRequest.Description) &&
                                                   p.Name.Equals(saveCourseRequest.Name))), Times.Once());
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
            var courses = SetUpExistingCourse();

            var id = courses.FirstOrDefault().Id;
            _coursesController.Delete(id);

            _mockCourseRepository.Verify(c => c.DeleteById(id), Times.Once());
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

        private IEnumerable<Domain.Entities.Course> SetUpExistingCourse()
        {
            var courses = new List<Domain.Entities.Course>
                {
                    new Domain.Entities.Course
                        {
                            Id = Guid.NewGuid(),
                            Code = "PSY101",
                            Description = "Psych!",
                            Name = "Psychology 101"
                        },
                    new Domain.Entities.Course
                        {
                            Id = Guid.NewGuid(),
                            Code = "PSY102",
                            Description = "Psych!",
                            Name = "Psychology 102"
                        },
                    new Domain.Entities.Course
                        {
                            Id = Guid.NewGuid(),
                            Code = "PSY103",
                            Description = "Psych!",
                            Name = "Psychology 103"
                        }
                };
            _mockCourseRepository.Setup(t => t.GetAll()).Returns(courses.AsQueryable());

            return courses;
        }
    }
}
