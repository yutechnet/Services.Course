using System.Collections.Generic;
using System.Linq;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Domain.Handlers
{
    public class CourseUpdatedHandler : IHandle<CourseUpdated>
    {
        private readonly IDomainEvents _domainEvents;

        public CourseUpdatedHandler(IDomainEvents domainEvents)
        {
            _domainEvents = domainEvents;
        }

        public void Handle(IDomainEvent domainEvent)
        {
            var @event = domainEvent as CourseUpdated;

            var events = new List<IDomainEvent>();
            foreach (var program in @event.New.Programs)
            {
                if (@event.Old.Programs.All(x => x.Id != program.Id))
                {
                    _domainEvents.Raise<CourseAssociatedWithProgram>(new CourseAssociatedWithProgram { ProgramId = program.Id });
                    //events.Add(new CourseAssociatedWithProgram { ProgramId = program.Id });
                }
            }

            foreach (var program in @event.Old.Programs)
            {
                if (@event.New.Programs.All(x => x.Id != program.Id))
                {
                    _domainEvents.Raise<CourseAssociatedWithProgram>(new CourseDisassociatedWithProgram { ProgramId = program.Id });
                    //events.Add(new CourseDisassociatedWithProgram { ProgramId = program.Id });
                }
            }

            //bus.Publish<CourseCreatedEvent>(new CourseCreatedEvent {});
            if (@event.New.Name != @event.Old.Name ||
                @event.New.Code != @event.Old.Code ||
                @event.New.Description != @event.Old.Description)
            {
                //events.Add(new CourseInfoUpdated
                //    {
                //        Code = @event.New.Code,
                //        Description = @event.New.Description,
                //        Name = @event.New.Name
                //    });
                _domainEvents.Raise<CourseInfoUpdated>(new CourseInfoUpdated
                    {
                        Code = @event.New.Code,
                        Description = @event.New.Description,
                        Name = @event.New.Name
                    });
            }

            //var store = new CourseEventStore();
            //store.Store(@event.AggregateId, events);
        }
    }
}