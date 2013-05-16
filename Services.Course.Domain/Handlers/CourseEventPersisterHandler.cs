using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;
using EventStore;

namespace BpeProducts.Services.Course.Domain.Handlers
{
    public class CourseEventPersisterHandler : IHandle<CourseAssociatedWithProgram>,
                                               IHandle<CourseDisassociatedWithProgram>,
                                               IHandle<CourseCreated>, IHandle<CourseInfoUpdated>,
                                               IHandle<CourseDeleted>, IHandle<CourseSegmentAdded>,
                                               IHandle<CourseSegmentUpdated>
    {
        private readonly IStoreCourseEvents _eventStore;

        public CourseEventPersisterHandler(IStoreCourseEvents eventStore)
        {
            _eventStore = eventStore;
        }

        public void Handle(IDomainEvent domainEvent)
        {
            _eventStore.Store(domainEvent);
        }
    }
}