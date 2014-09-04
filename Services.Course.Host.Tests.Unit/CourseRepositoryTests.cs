using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using BpeProducts.Common.Exceptions;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Course.Domain.CourseAggregates;
using Moq;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit
{
    [TestFixture]
    public class CourseRepositoryTests
    {

        const string courseCode = "TestCode";
        private ICourseRepository _courseRepository;
        private Mock<IRepository> _repositoryMock;
        private AutoMock _autoMock;


        [SetUp]
        public void SetUp()
        {
            _autoMock = AutoMock.GetLoose();
            _repositoryMock = _autoMock.Mock<IRepository>();
            _courseRepository = _autoMock.Create<CourseRepository>();
        }

        [Test]
        public void Can_Get_Course_By_Course_Code()
        {
            var courseId = Guid.NewGuid();
            var course =  new Domain.CourseAggregates.Course
            {
                Id = courseId,
                Code = courseCode
            };
            var courseList = new List<Domain.CourseAggregates.Course> { course };

            _repositoryMock.Setup(x => x.Query<Domain.CourseAggregates.Course>()).Returns(courseList.AsQueryable);

            var actualCourse = _courseRepository.GetOrThrowByCourseCode(courseCode);
            Assert.That(actualCourse.Id, Is.EqualTo(courseId));
            Assert.That(actualCourse.Code, Is.EqualTo(courseCode));
        }

        [Test]
        public void Throws_NotFound_Exception_When_Course_Code_Not_Exist()
        {
            var courseList = new List<Domain.CourseAggregates.Course>();
            _repositoryMock.Setup(x => x.Query<Domain.CourseAggregates.Course>()).Returns(courseList.AsQueryable);
            Assert.Throws<NotFoundException>(() => _courseRepository.GetOrThrowByCourseCode(courseCode));
        }

        [Test]
        public void Throws_BadRequest_Exception_When_More_Than_One_Courses_Have_Same_Course_Code()
        {
            var course1 = new Domain.CourseAggregates.Course
            {
                Id = Guid.NewGuid(),
                Code = courseCode
            };
            var course2 = new Domain.CourseAggregates.Course
            {
                Id = Guid.NewGuid(),
                Code = courseCode
            };
            var courseList = new List<Domain.CourseAggregates.Course> { course1, course2};

            _repositoryMock.Setup(x => x.Query<Domain.CourseAggregates.Course>()).Returns(courseList.AsQueryable);
            Assert.Throws<BadRequestException>(() => _courseRepository.GetOrThrowByCourseCode(courseCode));
        }
    }
}
