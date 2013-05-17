using System;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Domain.Handlers
{
	public class UpdateModelOnCourseCreation : IHandle<CourseCreated>
	{
		private readonly ICourseRepository _courseRepository;
		public UpdateModelOnCourseCreation(ICourseRepository courseRepository)
		{
			_courseRepository = courseRepository;
		}
		public void Handle(IDomainEvent domainEvent)
		{
			var e = domainEvent as CourseCreated;
            if (e == null)
            {
                throw new InvalidOperationException("Invalid domain event.");
            }

			_courseRepository.Add(e.Course);
		}
	}
}