using System;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Domain.Handlers
{
	public class UpdateModelOnAddingCoursePrerequisite : IHandle<CoursePrerequisiteAdded>
	{
		private readonly IRepository _repository;

		public UpdateModelOnAddingCoursePrerequisite(IRepository repository)
		{
			_repository = repository;
		}

		public void Handle(IDomainEvent domainEvent)
		{
			var e = domainEvent as CoursePrerequisiteAdded;
			if (e == null)
			{
				throw new InvalidOperationException("Invalid domain event.");
			}

			var course = _repository.Get<Courses.Course>(e.AggregateId);
			var prerequisiteCourse = _repository.Get<Courses.Course>(e.PrerequisiteCourseId);

			course.AddPrerequisite(prerequisiteCourse);

			_repository.Save(course);
		}
	}
}