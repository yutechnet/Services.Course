using System;
using System.Collections.Generic;
using System.Linq;
using BpeProducts.Common.NHibernate;
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
                            IsTemplate = @event.Request.IsTemplate
                        });
                }
            }
        }
    }
}
