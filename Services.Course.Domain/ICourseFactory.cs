using System;
using BpeProducts.Services.Course.Contract;
using EventStore;

namespace BpeProducts.Services.Course.Domain
{
	public interface ICourseFactory
	{
		Entities.Course Create(SaveCourseRequest request);
		Entities.Course Reconstitute(Guid aggregateId);
		Entities.Course Reconstitute(IEventStream stream, Entities.Course course = null);
	}
}