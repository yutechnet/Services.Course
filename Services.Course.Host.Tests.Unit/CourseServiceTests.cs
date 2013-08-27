using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Services.Course.Domain;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;
using Moq;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit
{
    [TestFixture]
    public class CourseServiceTests
    {
        private ICourseService _courseService;
        private Mock<ICourseFactory> _courseFactoryMock;
        private Mock<IDomainEvents> _domainEventsMock;
	    private Mock<ICourseRepository> _repoMock;

        [SetUp]
        public void SetUp()
        {
            _courseFactoryMock = new Mock<ICourseFactory>();
            _domainEventsMock = new Mock<IDomainEvents>();
	        _repoMock = new Mock<ICourseRepository>();

			_courseService = new CourseService(_courseFactoryMock.Object, _domainEventsMock.Object, _repoMock.Object);
        }

        [Test]
        public void Can_Add_prerequisites()
        {
            var courseToReturn = new Domain.Courses.Course {Id = Guid.NewGuid(), ActiveFlag = true};
			_courseFactoryMock.Setup(r => r.Reconstitute(It.IsAny<Guid>())).Returns(courseToReturn);

            var courseToBePrerequisite = new Domain.Courses.Course { Id = Guid.NewGuid(), ActiveFlag = true };
            var prereqCoursesInDb = new List<Course.Domain.Courses.Course> {courseToBePrerequisite };

            _repoMock.Setup(c => c.Get(It.IsAny<List<Guid>>())).Returns(prereqCoursesInDb);

            var newPrerequisiteList = new List<Guid> {courseToBePrerequisite.Id};
            _courseService.UpdatePrerequisiteList(courseToReturn.Id, newPrerequisiteList);

            _domainEventsMock.Verify(d => d.Raise<CoursePrerequisiteAdded>(It.IsAny<CoursePrerequisiteAdded>()), Times.Exactly(1));
            _domainEventsMock.Verify(d => d.Raise<CoursePrerequisiteRemoved>(It.IsAny<CoursePrerequisiteRemoved>()), Times.Exactly(0));
        }

        [Test]
        public void Can_Remove_prerequisites()
        {
			var prerequisiteCourse = new Domain.Courses.Course { Id = Guid.NewGuid() };
			prerequisiteCourse.Publish("");

           var prereqCoursesInDb = new List<Course.Domain.Courses.Course> { prerequisiteCourse };
            _repoMock.Setup(c => c.Get(It.IsAny<List<Guid>>())).Returns(prereqCoursesInDb);

            var courseToReturn = new Domain.Courses.Course { Id = Guid.NewGuid(), ActiveFlag = true };
			courseToReturn.AddPrerequisite(prerequisiteCourse);
			_courseFactoryMock.Setup(r => r.Reconstitute(It.IsAny<Guid>())).Returns(courseToReturn);
			//_repoMock.Setup(c => c.Get(It.IsAny<Guid>())).Returns(courseToReturn);

            var newPrerequisiteList = new List<Guid>();
            _courseService.UpdatePrerequisiteList(courseToReturn.Id, newPrerequisiteList);

            _domainEventsMock.Verify(d => d.Raise<CoursePrerequisiteAdded>(It.IsAny<CoursePrerequisiteAdded>()), Times.Exactly(0));
            _domainEventsMock.Verify(d => d.Raise<CoursePrerequisiteRemoved>(It.IsAny<CoursePrerequisiteRemoved>()), Times.Exactly(1));
        }

        [Test]
        public void Can_Add_And_Remove_prerequisites()
        {
            var guid1 = Guid.NewGuid();
            var guid2 = Guid.NewGuid();
            var guid3 = Guid.NewGuid();

            var prerequisiteCourse = new Domain.Courses.Course {Id = guid1};
	        var prerequisiteCourse2 = new Domain.Courses.Course {Id = guid2};
            
            var prerequisiteCourseToBeAdded = new Domain.Courses.Course {Id = guid3};
			prerequisiteCourse.Publish("");
			prerequisiteCourse2.Publish("");
            var prereqCoursesInDb = new List<Course.Domain.Courses.Course> {prerequisiteCourse, prerequisiteCourse2,prerequisiteCourseToBeAdded};

            var courseToReturn = new Domain.Courses.Course { Id = Guid.NewGuid(), ActiveFlag = true };
			courseToReturn.AddPrerequisite(prerequisiteCourse);
			courseToReturn.AddPrerequisite(prerequisiteCourse2);
			_courseFactoryMock.Setup(r => r.Reconstitute(It.IsAny<Guid>())).Returns(courseToReturn);
			//_repoMock.Setup(c => c.Get(It.IsAny<Guid>())).Returns(courseToReturn);
            _repoMock.Setup(c => c.Get(It.IsAny<List<Guid>>())).Returns(prereqCoursesInDb);

            var newPrerequisiteList = new List<Guid> { guid2, prerequisiteCourseToBeAdded.Id };
            _courseService.UpdatePrerequisiteList(courseToReturn.Id, newPrerequisiteList);

            _domainEventsMock.Verify(d => d.Raise<CoursePrerequisiteAdded>(It.IsAny<CoursePrerequisiteAdded>()), Times.Exactly(1));
            _domainEventsMock.Verify(d => d.Raise<CoursePrerequisiteRemoved>(It.IsAny<CoursePrerequisiteRemoved>()), Times.Exactly(1));
        }
    }
}
