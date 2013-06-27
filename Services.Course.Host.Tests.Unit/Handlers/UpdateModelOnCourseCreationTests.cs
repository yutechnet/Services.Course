using System;
using System.Collections.Generic;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Handlers;
using BpeProducts.Services.Course.Domain.Repositories;
using Moq;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit.Handlers
{
    [TestFixture]
    public class UpdateModelOnCourseCreationTests
    {
        private Mock<ICourseFactory> _mockCourseFactory;
        private Mock<ICourseRepository> _mockCourseRepository;
        private Mock<IDomainEvents> _mockDomainEvents;
        private UpdateModelOnCourseCreation _updateModelOnCourseCreationTests;

        [SetUp]
        public void SetUp()
        {
            _mockCourseRepository = new Mock<ICourseRepository>();
            _mockDomainEvents = new Mock<IDomainEvents>();
            _mockCourseFactory = new Mock<ICourseFactory>();
            _updateModelOnCourseCreationTests = new UpdateModelOnCourseCreation(_mockCourseFactory.Object, _mockCourseRepository.Object, _mockDomainEvents.Object);
        }

        [Test]
        public void Throw_Exception_When_Domain_Event_Is_Not_CourseCreated()
        {
            var exception =
                Assert.Throws<InvalidOperationException>(
                    () => _updateModelOnCourseCreationTests.Handle(new FakeDomainEvent()));
            Assert.That(exception.Message, Is.EqualTo("Invalid domain event."));
        }

        [Test]
        public void Add_New_Course_To_Repository()
        {
            var course = new Domain.Entities.Course();
            _updateModelOnCourseCreationTests.Handle(new CourseCreated { Course = course });

            _mockCourseRepository.Verify(c => c.Save(course), Times.Once());
        }

        [Test]
        public void Add_New_Course_From_Template_To_Repository()
        {
            var course = new Domain.Entities.Course();
            course.Template = new Domain.Entities.Course()
                {
                    Segments =
                        new List<CourseSegment>()
                            {
                                new CourseSegment()
                                    {
                                        Id = Guid.NewGuid(),
                                        Description = "Parent",
                                        ChildrenSegments =
                                            new List<CourseSegment>()
                                                {
                                                    new CourseSegment()
                                                        {
                                                            Id = Guid.NewGuid(),
                                                            Description = "Child"
                                                        }
                                                }
                                    }
                            }
                };

            _updateModelOnCourseCreationTests.Handle(new CourseCreated { Course = course });

            _mockCourseRepository.Verify(c => c.Save(course), Times.Once());
            _mockDomainEvents.Verify(c => c.Raise<CourseSegmentAdded>(It.IsAny<CourseSegmentAdded>()), Times.Exactly(2));
            
        }
    }
}
