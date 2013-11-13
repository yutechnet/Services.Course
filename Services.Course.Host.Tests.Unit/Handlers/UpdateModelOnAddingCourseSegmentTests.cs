using System;
using System.Collections.Generic;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Handlers;
using BpeProducts.Services.Course.Domain.Repositories;
using BpeProducts.Services.Course.Host.App_Start;
using Moq;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit.Handlers
{
    [TestFixture]
    public class UpdateModelOnAddingCourseSegmentTests
    {
        private Mock<ICourseRepository> _mockRepository;
        private UpdateModelOnAddingCourseSegment _updateModelOnAddingCourseSegment;

        [TestFixtureSetUp]
        public static void TestFixtureSetup()
        {
            MapperConfiguration.Configure();
        }

        [SetUp]
        public void SetUp()
        {
            _mockRepository = new Mock<ICourseRepository>();
            _updateModelOnAddingCourseSegment = new UpdateModelOnAddingCourseSegment(_mockRepository.Object);
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
            var course = new Domain.Courses.Course
                {
                    Id = courseId,
                    Code = "Code",
                    Name = "Name",
                    Description = "Description"
                };

            _mockRepository.Setup(c => c.GetOrThrow(It.IsAny<Guid>())).Returns(course);

            var courseSegementId = Guid.NewGuid();
            var courseSegmentAddedEvent = new CourseSegmentAdded
                {
                    AggregateId = courseId,
                    ParentSegmentId = Guid.Empty,
                    SegmentId = courseSegementId,
                    Request = new SaveCourseSegmentRequest
                        {
                            Description = "Description",
                            Name = "Name",
                            Type = "Type"
                        }
                };

            _updateModelOnAddingCourseSegment.Handle(courseSegmentAddedEvent);

            _mockRepository.Verify(c => c.GetOrThrow(courseId), Times.Once());
            _mockRepository.Verify(c => c.Save(It.Is<Domain.Courses.Course>(d => d.Segments.Count == 1 && d.Segments[0].Id == courseSegementId)), 
                Times.Once());
        }

        [Test]
        public void Add_Child_Segment_If_ParentSegentId_Specified()
        {
            // Setup
            var courseId = Guid.NewGuid();
            var parentSegmentId = Guid.NewGuid();
            var course = new Domain.Courses.Course
            {
                Id = courseId,
                Code = "Code",
                Name = "Name",
                Description = "Description"
            };

            course.AddSegment(parentSegmentId, Guid.Empty, new SaveCourseSegmentRequest());

            _mockRepository.Setup(c => c.GetOrThrow(It.IsAny<Guid>())).Returns(course);

            var courseSegmentId = Guid.NewGuid();
            var courseSegmentAddedEvent = new CourseSegmentAdded
            {
                AggregateId = courseId,
                ParentSegmentId = parentSegmentId,
                SegmentId = courseSegmentId,
                Request = new SaveCourseSegmentRequest
                {
                    Description = "Description",
                    Name = "Name",
                    Type = "Type"
                }
            };

            _updateModelOnAddingCourseSegment.Handle(courseSegmentAddedEvent);

            _mockRepository.Verify(c => c.GetOrThrow(courseId), Times.Once());
            _mockRepository.Verify(c => c.Save(It.Is<Domain.Courses.Course>(d => d.Segments.Count == 2 
                && d.Segments[0].ChildSegments.Count == 1
                && d.Segments[0].ChildSegments[0].Id == courseSegmentId)),
                Times.Once());
        }

    }
}
