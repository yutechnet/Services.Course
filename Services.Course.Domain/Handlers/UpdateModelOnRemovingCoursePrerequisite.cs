using System;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;

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

			//TODO: Ensure that the courses loaded from NHB are valid? This isn't done in 'UpdateModelOnCourseUpdating'..
			// Or do this in the domain?

			course.RemovePrerequisite(e.PrerequisiteCourseId);

			_repository.Save(course);
		}
	}
}