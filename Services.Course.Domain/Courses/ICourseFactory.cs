using System;
using System.Collections.Generic;
using BpeProducts.Services.Course.Contract;
using EventStore;

namespace BpeProducts.Services.Course.Domain
{
	public interface ICourseFactory
	{
		Entities.Course Create(SaveCourseRequest request);
		Entities.Course Reconstitute(Guid aggregateId);
		Entities.Course Reconstitute(ICollection<EventMessage> stream, Entities.Course course = null);
	}
}