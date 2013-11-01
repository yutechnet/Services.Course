using System;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain.Handlers
{
	public class UpdateModelOnRemovingCoursePrerequisite : IHandle<CoursePrerequisiteRemoved>
	{
		private readonly IRepository _repository;

		public UpdateModelOnRemovingCoursePrerequisite(IRepository repository)
		{
			_repository = repository;
		}

		public void Handle(IDomainEvent domainEvent)
		{
			var e = domainEvent as CoursePrerequisiteRemoved;
			if (e == null)
			{
				throw new InvalidOperationException("Invalid domain event.");
			}

			var course = _repository.Get<Courses.Course>(e.AggregateId);

			course.RemovePrerequisite(e.PrerequisiteCourseId);

			_repository.Save(course);
		}
	}
}