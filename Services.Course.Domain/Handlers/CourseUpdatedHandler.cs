using System;
using System.Collections.Generic;
using System.Linq;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Domain.Handlers
{
    public class CourseUpdatedHandler : IHandle<CourseUpdated>
    {
        private readonly IDomainEvents _domainEvents;
        private readonly IRepository _repository;

        public CourseUpdatedHandler(IDomainEvents domainEvents, IRepository repository)
        {
            _domainEvents = domainEvents;
            _repository = repository;
        }

        public void Handle(IDomainEvent domainEvent)
        {
            var @event = domainEvent as CourseUpdated;
            if (@event != null)
            {
                var events = new List<IDomainEvent>();
                foreach (var programId in @event.Request.ProgramIds)
                    if (@event.Old.Programs.All(x => x.Id != programId))
                    {
                        var program = _repository.Get<Entities.Program>(programId);
                        _domainEvents.Raise<CourseAssociatedWithProgram>(new CourseAssociatedWithProgram
                            {
                                AggregateId = @event.AggregateId,
                                ProgramId = programId, 
                                Name = program.Name,
                                Description = program.Description,
                                ProgramType = program.ProgramType
                            });
                    }

                foreach (var program in @event.Old.Programs)
                {
                    if (@event.Request.ProgramIds.All(x => x != program.Id))
                    {
                        _domainEvents.Raise<CourseDisassociatedWithProgram>(new CourseDisassociatedWithProgram
                            {
                                AggregateId = @event.AggregateId,
                                ProgramId = program.Id
                            });
                    }
                }

				// Determine prerequisites added
	            foreach (var incomingPrereqId in @event.Request.PrerequisiteCourseIds)
	            {
		            // If all of the current prerequisite courses do not equal this incoming prerequisiteId, it's new
		            if (@event.Old.Prerequisites.All(existingPrereqs => existingPrereqs.Id != incomingPrereqId))
		            {
						_domainEvents.Raise<CoursePrerequisiteAdded>(new CoursePrerequisiteAdded
						{
							AggregateId = @event.AggregateId,
							PrerequisiteCourseId = incomingPrereqId
						});
		            }
	            }

				// Determine prerequisites removed
				foreach (var currentPrereqCourse in @event.Old.Prerequisites)
				{
					// If all of the incoming prerequisite Ids do not equal this existing prerequisiteId, it's been removed
					if (@event.Request.PrerequisiteCourseIds.All(incomingPrereqIds => incomingPrereqIds != currentPrereqCourse.Id))
					{
						_domainEvents.Raise<CoursePrerequisiteRemoved>(new CoursePrerequisiteRemoved
						{
							AggregateId = @event.AggregateId,
							PrerequisiteCourseId = currentPrereqCourse.Id
						});
					}
				}

	            //bus.Publish<CourseCreatedEvent>(new CourseCreatedEvent {});
                if (@event.Request.Name != @event.Old.Name ||
                    @event.Request.Code != @event.Old.Code ||
                    @event.Request.Description != @event.Old.Description ||
                    @event.Request.CourseType != @event.Old.CourseType ||
                    @event.Request.IsTemplate != @event.Old.IsTemplate
					)
                {
                    _domainEvents.Raise<CourseInfoUpdated>(new CourseInfoUpdated
                        {
                            AggregateId = @event.AggregateId,
                            Name = @event.Request.Name,
                            Code = @event.Request.Code,
                            Description = @event.Request.Description,
                            CourseType = @event.Request.CourseType,
                            IsTemplate = @event.Request.IsTemplate,
							PrerequisiteCourseIds = @event.Request.PrerequisiteCourseIds
                        });
                }
            }
        }

		// TODO: Move Guid comparison items into common
	    public bool AreGuidListsTheSame(List<Guid> guidList1, List<Guid> guidList2)
	    {
			return guidList1.All(l1 => guidList2.Any(l2 => l1 == l2));
	    }
    }
}
