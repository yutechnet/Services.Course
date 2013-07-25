using System;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Handlers;
using BpeProducts.Services.Course.Domain.Repositories;
using Moq;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit.Handlers
{
	[TestFixture]
	public class UpdateModelOnRemovingCoursePrerequisiteTests
	{
		private Mock<IRepository> _repositoryMock;
		private UpdateModelOnRemovingCoursePrerequisite _removePrereqHandler;

		[SetUp]
		public void SetUp()
		{
			_repositoryMock = new Mock<IRepository>();
			_removePrereqHandler = new UpdateModelOnRemovingCoursePrerequisite(_repositoryMock.Object);
		}

		[Test]
		public void Throw_Exception_When_Domain_Event_Is_Not_PrerequisteRemoved()
		{
			var exception =
				Assert.Throws<InvalidOperationException>(
					() => _removePrereqHandler.Handle(new FakeDomainEvent()));
			Assert.That(exception.Message, Is.EqualTo("Invalid domain event."));
		}

		[Test]
		public void Can_remove_prerequiste_for_course()
		{
			var aggregateCourseGuid = Guid.NewGuid();
			var prerequisiteCourseGuid = Guid.NewGuid();

			var aggregateCourse = new Domain.Courses.Course();
			aggregateCourse.AddPrerequisite(new Domain.Courses.Course { Id = prerequisiteCourseGuid } );
			aggregateCourse.AddPrerequisite(new Domain.Courses.Course { Id = Guid.NewGuid() });
			var prerequisiteCourse = new Domain.Courses.Course { Id = prerequisiteCourseGuid };

			_repositoryMock.Setup(r => r.Get<Domain.Courses.Course>(aggregateCourseGuid)).Returns(aggregateCourse);
			_repositoryMock.Setup(r => r.Get<Domain.Courses.Course>(prerequisiteCourseGuid)).Returns(prerequisiteCourse);

			_removePrereqHandler.Handle(new CoursePrerequisiteRemoved { AggregateId = aggregateCourseGuid, PrerequisiteCourseId = prerequisiteCourseGuid });

			_repositoryMock.Verify(c => c.Save(aggregateCourse), Times.Once());
			Assert.That(aggregateCourse.Prerequisites.Count, Is.EqualTo(1));
		}

		[Test]
		public void Can_remove_prerequiste_for_course_does_not_throw_if_prereq_is_already_removed()
		{
			var aggregateCourseGuid = Guid.NewGuid();
			var prerequisiteCourseGuid = Guid.NewGuid();

			var aggregateCourse = new Domain.Courses.Course();
			aggregateCourse.AddPrerequisite(new Domain.Courses.Course { Id = prerequisiteCourseGuid });
			aggregateCourse.AddPrerequisite(new Domain.Courses.Course { Id = Guid.NewGuid() });
			var prerequisiteCourse = new Domain.Courses.Course { Id = prerequisiteCourseGuid };

			_repositoryMock.Setup(r => r.Get<Domain.Courses.Course>(aggregateCourseGuid)).Returns(aggregateCourse);
			_repositoryMock.Setup(r => r.Get<Domain.Courses.Course>(prerequisiteCourseGuid)).Returns(prerequisiteCourse);

			_removePrereqHandler.Handle(new CoursePrerequisiteRemoved { AggregateId = aggregateCourseGuid, PrerequisiteCourseId = Guid.NewGuid() });

			_repositoryMock.Verify(c => c.Save(aggregateCourse), Times.Once());
			Assert.That(aggregateCourse.Prerequisites.Count, Is.EqualTo(2));
		}
	}
}