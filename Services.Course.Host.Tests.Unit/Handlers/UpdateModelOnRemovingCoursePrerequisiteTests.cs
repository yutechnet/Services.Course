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
	public class UpdateModelOnRemovingCoursePrerequisiteTests
	{
		private Mock<IRepository> _repositoryMock;
		private UpdateModelOnRemovingCoursePrerequisite _removePrereqHandler;
		private AutoMock _autoMock;
		private Mock<ICoursePublisher> _coursePublisher;

		Course.Domain.Courses.Course GetCourse()
		{
		
			return _autoMock.Create<Course.Domain.Courses.Course>();
			
		}

		[SetUp]
		public void SetUp()
		{
			_autoMock = AutoMock.GetLoose();
			_repositoryMock = new Mock<IRepository>();
			_coursePublisher = _autoMock.Mock<ICoursePublisher>();
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

			var courseToAddAsPrerequisite = GetCourse();
			courseToAddAsPrerequisite.Id = prerequisiteCourseGuid ;
			courseToAddAsPrerequisite.Publish("", _coursePublisher.Object);

			var courseToAddAsPrerequisite2 = GetCourse();
			courseToAddAsPrerequisite2.Id = Guid.NewGuid();
			courseToAddAsPrerequisite2.Publish("", _coursePublisher.Object);

			var aggregateCourse =GetCourse();
			aggregateCourse.AddPrerequisite(courseToAddAsPrerequisite);
			aggregateCourse.AddPrerequisite(courseToAddAsPrerequisite2);
			var prerequisiteCourse = GetCourse();
			prerequisiteCourse.Id = prerequisiteCourseGuid ;

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

			var courseToAddAsPrerequisite = GetCourse();
			courseToAddAsPrerequisite.Id = prerequisiteCourseGuid ;
			courseToAddAsPrerequisite.Publish("", _coursePublisher.Object);

			var courseToAddAsPrerequisite2 = GetCourse();
			courseToAddAsPrerequisite2.Id = Guid.NewGuid() ;
			courseToAddAsPrerequisite2.Publish("", _coursePublisher.Object);

			var aggregateCourse = GetCourse();
			aggregateCourse.AddPrerequisite(courseToAddAsPrerequisite);
			aggregateCourse.AddPrerequisite(courseToAddAsPrerequisite2);
			var prerequisiteCourse = GetCourse();
			prerequisiteCourse.Id = prerequisiteCourseGuid ;

			_repositoryMock.Setup(r => r.Get<Domain.Courses.Course>(aggregateCourseGuid)).Returns(aggregateCourse);
			_repositoryMock.Setup(r => r.Get<Domain.Courses.Course>(prerequisiteCourseGuid)).Returns(prerequisiteCourse);

			_removePrereqHandler.Handle(new CoursePrerequisiteRemoved { AggregateId = aggregateCourseGuid, PrerequisiteCourseId = Guid.NewGuid() });

			_repositoryMock.Verify(c => c.Save(aggregateCourse), Times.Once());
			Assert.That(aggregateCourse.Prerequisites.Count, Is.EqualTo(2));
		}
	}
}