using System;
using System.Collections.Generic;
using BpeProducts.Services.Course.Contract;
using EventStore;

namespace BpeProducts.Services.Course.Domain
{
	public interface ICourseFactory
	{
		Courses.Course Create(SaveCourseRequest request);
		Courses.Course Reconstitute(Guid aggregateId);
		Courses.Course Reconstitute(ICollection<EventMessage> stream, Courses.Course course = null);
	}
}