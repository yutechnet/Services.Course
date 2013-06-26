﻿using System;
using BpeProducts.Services.Course.Domain.Events;
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
			var e = domainEvent as CourseDeleted;
            if (e == null)
            {
                throw new InvalidOperationException("Invalid domain event.");
            }

			var course = _courseRepository.Load(e.AggregateId);
			_courseRepository.Delete(course);
		}
	}
}