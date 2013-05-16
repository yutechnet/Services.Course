using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Handlers;
using BpeProducts.Services.Course.Domain.Repositories;
using BpeProducts.Services.Course.Host.App_Start;
using Moq;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Domain.Unit.Handlers
{
    [TestFixture]
    public class UpdateModelOnAddingCourseSegmentTests
    {
        private Mock<ICourseRepository> _mockCourseRepository;
        private UpdateModelOnAddingCourseSegment _updateModelOnAddingCourseSegment;

        [TestFixtureSetUp]
        public static void TestFixtureSetup()
        {
            MapperConfig.ConfigureMappers();
        }

        [SetUp]
        public void SetUp()
        {
            _mockCourseRepository = new Mock<ICourseRepository>();
            _updateModelOnAddingCourseSegment = new UpdateModelOnAddingCourseSegment(_mockCourseRepository.Object);
        }

        [Test]
        public void Throws_Exception_When_DomainEvent_Is_Not_CourseSegmentAdded()
        {
            var exception = Assert.Throws<InvalidOperationException>(
                () => _updateModelOnAddingCourseSegment.Handle(new CourseInfoUpdated()));
            Assert.That(exception.Message, Is.EqualTo("Invalid domain event."));
        }

        [Test]
        public void Add_Root_Segment_If_ParentSegmentId_Not_Specified()
        {
            // Setup
            var courseId = Guid.NewGuid();
            var course = new Entities.Course
                {
                    Id = courseId,
                    Code = "Code",
                    Name = "Name",
                    Description = "Description"
                };

            _mockCourseRepository.Setup(c => c.GetById(It.IsAny<Guid>())).Returns(course);

            var courseSegementId = Guid.NewGuid();
            var courseSegmentAddedEvent = new CourseSegmentAdded
                {
                    AggregateId = courseId,
                    Description = "Description",
                    Id = courseSegementId,
                    Name = "Name",
                    Type = "someType"
                };

            _updateModelOnAddingCourseSegment.Handle(courseSegmentAddedEvent);

            _mockCourseRepository.Verify(c => c.GetById(courseId), Times.Once());
            _mockCourseRepository.Verify(c => c.Update(It.Is<Entities.Course>(d => d.Segments.Count == 1 && d.Segments[0].Id == courseSegementId)), 
                Times.Once());
        }

        [Test]
        public void Add_Child_Segment_If_ParentSegentId_Specified()
        {
            // Setup
            var courseId = Guid.NewGuid();
            var parentSegmentId = Guid.NewGuid();
            var course = new Entities.Course
            {
                Id = courseId,
                Code = "Code",
                Name = "Name",
                Description = "Description",
                Segments = new List<CourseSegment>
                    {
                        new CourseSegment
                            {
                                Id = parentSegmentId
                            }
                    }
            };

            _mockCourseRepository.Setup(c => c.GetById(It.IsAny<Guid>())).Returns(course);

            var courseSegmentId = Guid.NewGuid();
            var courseSegmentAddedEvent = new CourseSegmentAdded
            {
                AggregateId = courseId,
                Description = "Description",
                Id = courseSegmentId,
                Name = "Name",
                Type = "someType",
                ParentSegmentId = parentSegmentId
            };

            _updateModelOnAddingCourseSegment.Handle(courseSegmentAddedEvent);

            _mockCourseRepository.Verify(c => c.GetById(courseId), Times.Once());
            _mockCourseRepository.Verify(c => c.Update(It.Is<Entities.Course>(d => d.Segments.Count == 1 
                && d.Segments[0].ChildrenSegments.Count == 1
                && d.Segments[0].ChildrenSegments[0].Id == courseSegmentId)),
                Times.Once());
        }

    }
}
