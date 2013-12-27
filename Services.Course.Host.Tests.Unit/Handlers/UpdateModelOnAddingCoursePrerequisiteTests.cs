using System;
using Autofac.Extras.Moq;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Course.Domain.Courses;
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
		private AutoMock _autoMock;
		private Mock<ICoursePublisher> _coursePublisher;

		[SetUp]
		public void SetUp()
		{
			_repositoryMock = new Mock<IRepository>();
			_updatePrereqHandler = new UpdateModelOnAddingCoursePrerequisite(_repositoryMock.Object);
			_autoMock = AutoMock.GetLoose();
			_coursePublisher = _autoMock.Mock<ICoursePublisher>();
		}

		Course.Domain.Courses.Course GetCourse()
		{
			return _autoMock.Create<Course.Domain.Courses.Course>();

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

			var aggregateCourse = GetCourse();
			var courseToAddAsPrerequisite = GetCourse();
			courseToAddAsPrerequisite.Id = Guid.NewGuid() ;
			courseToAddAsPrerequisite.Publish("", _coursePublisher.Object);
			aggregateCourse.AddPrerequisite(courseToAddAsPrerequisite);
			var prerequisiteCourse =GetCourse();
			prerequisiteCourse.Publish("",_coursePublisher.Object);

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

			var aggregateCourse = GetCourse();
			var courseToAddAsPrerequisite = GetCourse();
			courseToAddAsPrerequisite.Id = prerequisiteCourseGuid;
			courseToAddAsPrerequisite.Publish("",_coursePublisher.Object);
			aggregateCourse.AddPrerequisite(courseToAddAsPrerequisite);

			var prerequisiteCourse = GetCourse();
			prerequisiteCourse.Id = prerequisiteCourseGuid ;
			prerequisiteCourse.Publish("", _coursePublisher.Object);

			_repositoryMock.Setup(r => r.Get<Domain.Courses.Course>(aggregateCourseGuid)).Returns(aggregateCourse);
			_repositoryMock.Setup(r => r.Get<Domain.Courses.Course>(prerequisiteCourseGuid)).Returns(prerequisiteCourse);

			_updatePrereqHandler.Handle(new CoursePrerequisiteAdded { AggregateId = aggregateCourseGuid, PrerequisiteCourseId = prerequisiteCourseGuid });

			_repositoryMock.Verify(c => c.Save(aggregateCourse), Times.Once());
			Assert.That(aggregateCourse.Prerequisites.Count, Is.EqualTo(1));
		}
	}
}