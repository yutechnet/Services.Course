using System;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Handlers;
using BpeProducts.Services.Course.Domain.Repositories;
using Moq;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit.Handlers
{
	[TestFixture]
	public class UpdateModelOnAddingCoursePrerequisiteTests
	{
		private Mock<IRepository> _repositoryMock;
		private UpdateModelOnAddingCoursePrerequisite _updatePrereqHandler;

		[SetUp]
		public void SetUp()
		{
			_repositoryMock = new Mock<IRepository>();
			_updatePrereqHandler = new UpdateModelOnAddingCoursePrerequisite(_repositoryMock.Object);
		}

		[Test]
		public void Throw_Exception_When_Domain_Event_Is_Not_PrerequisteAdded()
		{
			var exception =
				Assert.Throws<InvalidOperationException>(
					() => _updatePrereqHandler.Handle(new FakeDomainEvent()));
			Assert.That(exception.Message, Is.EqualTo("Invalid domain event."));
		}

		[Test]
		public void Can_add_prerequiste_for_course()
		{
			var aggregateCourseGuid = Guid.NewGuid();
			var prerequisiteCourseGuid = Guid.NewGuid();

			var aggregateCourse = new Domain.Courses.Course();
			var courseToAddAsPrerequisite = new Domain.Courses.Course { Id = Guid.NewGuid() };
			courseToAddAsPrerequisite.Publish("");
			aggregateCourse.AddPrerequisite(courseToAddAsPrerequisite);
			var prerequisiteCourse = new Domain.Courses.Course();
			prerequisiteCourse.Publish("");

			_repositoryMock.Setup(r => r.Get<Domain.Courses.Course>(aggregateCourseGuid)).Returns(aggregateCourse);
			_repositoryMock.Setup(r => r.Get<Domain.Courses.Course>(prerequisiteCourseGuid)).Returns(prerequisiteCourse);

			_updatePrereqHandler.Handle(new CoursePrerequisiteAdded { AggregateId = aggregateCourseGuid, PrerequisiteCourseId = prerequisiteCourseGuid });

			_repositoryMock.Verify(c => c.Save(aggregateCourse), Times.Once());
			Assert.That(aggregateCourse.Prerequisites.Count, Is.EqualTo(2));
		}

		[Test]
		public void Can_add_prerequiste_for_course_does_not_throw_if_prereq_already_exists()
		{
			var aggregateCourseGuid = Guid.NewGuid();
			var prerequisiteCourseGuid = Guid.NewGuid();

			var aggregateCourse = new Domain.Courses.Course();
			var courseToAddAsPrerequisite = new Domain.Courses.Course {Id = prerequisiteCourseGuid};
			courseToAddAsPrerequisite.Publish("");
			aggregateCourse.AddPrerequisite(courseToAddAsPrerequisite);
			var prerequisiteCourse = new Domain.Courses.Course { Id = prerequisiteCourseGuid };
			prerequisiteCourse.Publish("");

			_repositoryMock.Setup(r => r.Get<Domain.Courses.Course>(aggregateCourseGuid)).Returns(aggregateCourse);
			_repositoryMock.Setup(r => r.Get<Domain.Courses.Course>(prerequisiteCourseGuid)).Returns(prerequisiteCourse);

			_updatePrereqHandler.Handle(new CoursePrerequisiteAdded { AggregateId = aggregateCourseGuid, PrerequisiteCourseId = prerequisiteCourseGuid });

			_repositoryMock.Verify(c => c.Save(aggregateCourse), Times.Once());
			Assert.That(aggregateCourse.Prerequisites.Count, Is.EqualTo(1));
		}
	}
}