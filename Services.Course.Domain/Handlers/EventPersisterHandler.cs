using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Outcomes;
using BpeProducts.Services.Course.Domain.Repositories;
using EventStore;

namespace BpeProducts.Services.Course.Domain.Handlers
{
    public class EventPersisterHandler : IHandle<CourseAssociatedWithProgram>,
                                               IHandle<CourseDisassociatedWithProgram>,
                                               IHandle<CourseCreated>, IHandle<CourseInfoUpdated>,
                                               IHandle<CourseDeleted>, IHandle<CourseSegmentAdded>,
                                               IHandle<CourseSegmentUpdated>, IHandle<CourseVersionCreated>,
                                               IHandle<CourseVersionPublished>, IHandle<OutcomeVersionCreated>,
                                               IHandle<OutcomeVersionPublished>, IHandle<OutcomeCreated>
    {
        private readonly IStoreCourseEvents _eventStore;

        public EventPersisterHandler(IStoreCourseEvents eventStore)
        {
            _eventStore = eventStore;
        }

        public void Handle(IDomainEvent domainEvent)
        {
            _eventStore.Store(domainEvent);
        }
    }
}