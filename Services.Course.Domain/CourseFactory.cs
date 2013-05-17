using System;
using System.Collections.Generic;
using AutoMapper;
using Autofac.Features.Indexed;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Events;
using EventStore;

namespace BpeProducts.Services.Course.Domain
{
	public class CourseFactory : ICourseFactory
	{
		private readonly IStoreEvents _store;
		public readonly IIndex<string, IPlayEvent> _index;

		public CourseFactory(IStoreEvents store, IIndex<string, IPlayEvent> index )
		{
			_store = store;
			_index = index;
		}
		
		public Entities.Course Create(SaveCourseRequest request)
		{
			
			//TODO: get tenant id
			var course = new Entities.Course {Id = Guid.NewGuid(), ActiveFlag = true};
			Mapper.Map(request, course);
			return course;
		}

		public Entities.Course Reconstitute(Guid aggregateId)
		{
			Entities.Course course;

			// Get the latest snapshot
			Snapshot latestSnapshot = _store.Advanced.GetSnapshot(aggregateId, int.MaxValue);
			if (latestSnapshot == null)
			{
				using (IEventStream stream = _store.OpenStream(aggregateId, 0, int.MaxValue))
				{
					//throw if it is a new
					course = Reconstitute(stream.CommittedEvents);
				}
			}
			else
			{
				using (IEventStream stream = _store.OpenStream(latestSnapshot, int.MaxValue))
				{
					//throw if it is a new
					course = Reconstitute(stream.CommittedEvents, latestSnapshot.Payload as Entities.Course);
				}
			}
			return course;
		}

		public Entities.Course Reconstitute(ICollection<EventMessage> events, Entities.Course course = null)
		{
			course = course ?? new Entities.Course();

			foreach (EventMessage eventMessage in events)
			{
				object eventBody = eventMessage.Body;
				var applier = _index[eventBody.GetType().Name];
				applier.Apply(eventBody as IDomainEvent, course);
			}

			return course;
		}
	}

	
}