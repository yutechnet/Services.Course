﻿using System;
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
            courseToReturn.SetPrerequisites(new List<Domain.Courses.Course>());
			_repoMock.Setup(r => r.Get(It.IsAny<Guid>())).Returns(courseToReturn);

            var newPrerequisiteList = new List<Guid> {Guid.NewGuid()};
            _courseService.UpdatePrerequisiteList(Guid.NewGuid(), newPrerequisiteList);

            _domainEventsMock.Verify(d => d.Raise<CoursePrerequisiteAdded>(It.IsAny<CoursePrerequisiteAdded>()), Times.Exactly(1));
            _domainEventsMock.Verify(d => d.Raise<CoursePrerequisiteRemoved>(It.IsAny<CoursePrerequisiteRemoved>()), Times.Exactly(0));
        }

        [Test]
        public void Can_Remove_prerequisites()
        {
            var courseToReturn = new Domain.Courses.Course { Id = Guid.NewGuid(), ActiveFlag = true };
            courseToReturn.SetPrerequisites(new List<Domain.Courses.Course> { new Domain.Courses.Course { Id = Guid.NewGuid() } });
			_repoMock.Setup(c => c.Get(It.IsAny<Guid>())).Returns(courseToReturn);

            var newPrerequisiteList = new List<Guid>();
            _courseService.UpdatePrerequisiteList(Guid.NewGuid(), newPrerequisiteList);

            _domainEventsMock.Verify(d => d.Raise<CoursePrerequisiteAdded>(It.IsAny<CoursePrerequisiteAdded>()), Times.Exactly(0));
            _domainEventsMock.Verify(d => d.Raise<CoursePrerequisiteRemoved>(It.IsAny<CoursePrerequisiteRemoved>()), Times.Exactly(1));
        }

        [Test]
        public void Can_Add_And_Remove_prerequisites()
        {
            var guid1 = Guid.NewGuid();
            var guid2 = Guid.NewGuid();
            var courseToReturn = new Domain.Courses.Course { Id = Guid.NewGuid(), ActiveFlag = true };
            courseToReturn.SetPrerequisites(new List<Domain.Courses.Course> { new Domain.Courses.Course { Id = guid1 }, new Domain.Courses.Course {Id = guid2}});
			_repoMock.Setup(c => c.Get(It.IsAny<Guid>())).Returns(courseToReturn);

            var newPrerequisiteList = new List<Guid> { guid2, Guid.NewGuid() };
            _courseService.UpdatePrerequisiteList(Guid.NewGuid(), newPrerequisiteList);

            _domainEventsMock.Verify(d => d.Raise<CoursePrerequisiteAdded>(It.IsAny<CoursePrerequisiteAdded>()), Times.Exactly(1));
            _domainEventsMock.Verify(d => d.Raise<CoursePrerequisiteRemoved>(It.IsAny<CoursePrerequisiteRemoved>()), Times.Exactly(1));
        }
    }
}
