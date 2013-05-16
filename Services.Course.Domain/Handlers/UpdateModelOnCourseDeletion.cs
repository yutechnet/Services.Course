﻿using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Domain.Handlers
{
	public class UpdateModelOnCourseDeletion : IHandle<CourseDeleted>
	{
		private readonly ICourseRepository _courseRepository;
		public UpdateModelOnCourseDeletion(ICourseRepository courseRepository)
		{
			_courseRepository = courseRepository;
		}
		public void Handle(IDomainEvent domainEvent)
		{
			var e = (CourseDeleted) domainEvent;
			var course = _courseRepository.GetById(e.AggregateId);
			course.ActiveFlag = false;
			_courseRepository.Update(course);

		}
	}
}