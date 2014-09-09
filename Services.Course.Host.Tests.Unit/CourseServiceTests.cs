using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using BpeProducts.Services.Course.Domain.CourseAggregates;
using Moq;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit
{
    [TestFixture]
    public class CourseServiceTests
    {
        private ICourseService _courseService;
	    private Mock<ICourseRepository> _courseRepositoryMock;
	    private AutoMock _autoMock;
	    private Mock<ICoursePublisher> _coursePublisher;


	    [SetUp]
        public void SetUp()
        {
            _autoMock = AutoMock.GetLoose();
	        _courseRepositoryMock = _autoMock.Mock<ICourseRepository>();
			
		    _coursePublisher = _autoMock.Mock<ICoursePublisher>();
	        _courseService = _autoMock.Create<CourseService>();
        }


        [Test]
        public void Can_Add_prerequisites()
        {
	        var courseToUpdate = new Domain.CourseAggregates.Course
	            {
	                Id = Guid.NewGuid(),
                    IsDeleted = false
	            };

            _courseRepositoryMock.Setup(r => r.GetOrThrow(courseToUpdate.Id)).Returns(courseToUpdate);

            var prerequisite = new Domain.CourseAggregates.Course
                {
                    Id = Guid.NewGuid(),
                    IsDeleted = false
                };
            prerequisite.Publish("", _coursePublisher.Object);

            _courseRepositoryMock.Setup(c => c.GetOrThrow(prerequisite.Id)).Returns(prerequisite);

            var newPrerequisiteList = new List<Guid> {prerequisite.Id};
            _courseService.UpdatePrerequisiteList(courseToUpdate.Id, newPrerequisiteList);
        }

        [Test]
        public void Can_Remove_prerequisites()
        {
			var prerequisiteCourse =new Domain.CourseAggregates.Course
			    {
			        Id = Guid.NewGuid()
			    };

            prerequisiteCourse.Publish("",_coursePublisher.Object);

            var prereqCoursesInDb = new List<Domain.CourseAggregates.Course> { prerequisiteCourse };
            _courseRepositoryMock.Setup(c => c.Get(It.IsAny<List<Guid>>())).Returns(prereqCoursesInDb);

	        var courseToReturn = new Domain.CourseAggregates.Course
	            {
	                Id = Guid.NewGuid(),
                    IsDeleted = false
	            };

            courseToReturn.AddPrerequisite(prerequisiteCourse);
			_courseRepositoryMock.Setup(r => r.GetOrThrow(It.IsAny<Guid>())).Returns(courseToReturn);
			
            var newPrerequisiteList = new List<Guid>();
            _courseService.UpdatePrerequisiteList(courseToReturn.Id, newPrerequisiteList);
        }

        [Test]
        public void Can_Add_And_Remove_prerequisites()
        {
            var prerequisiteCourse1 = new Domain.CourseAggregates.Course {Id = Guid.NewGuid()};
            var prerequisiteCourse2 = new Domain.CourseAggregates.Course {Id = Guid.NewGuid()};
            var prerequisiteCourse3 = new Domain.CourseAggregates.Course {Id = Guid.NewGuid()};

            prerequisiteCourse1.Publish("", _coursePublisher.Object);
            prerequisiteCourse2.Publish("", _coursePublisher.Object);
            prerequisiteCourse3.Publish("", _coursePublisher.Object);

            var courseToReturn = new Domain.CourseAggregates.Course { Id = Guid.NewGuid(), IsDeleted = false };
            courseToReturn.AddPrerequisite(prerequisiteCourse1);
			courseToReturn.AddPrerequisite(prerequisiteCourse2);

			_courseRepositoryMock.Setup(r => r.GetOrThrow(It.IsAny<Guid>())).Returns(courseToReturn);
			
            _courseRepositoryMock.Setup(c => c.GetOrThrow(prerequisiteCourse1.Id)).Returns(prerequisiteCourse1);
            _courseRepositoryMock.Setup(c => c.GetOrThrow(prerequisiteCourse2.Id)).Returns(prerequisiteCourse2);
            _courseRepositoryMock.Setup(c => c.GetOrThrow(prerequisiteCourse3.Id)).Returns(prerequisiteCourse3);

            var newPrerequisiteList = new List<Guid> { prerequisiteCourse2.Id, prerequisiteCourse3.Id };
            _courseService.UpdatePrerequisiteList(courseToReturn.Id, newPrerequisiteList);

            Assert.That(courseToReturn.Prerequisites.Contains(prerequisiteCourse2));
            Assert.That(courseToReturn.Prerequisites.Contains(prerequisiteCourse3));
        }
    }
}
