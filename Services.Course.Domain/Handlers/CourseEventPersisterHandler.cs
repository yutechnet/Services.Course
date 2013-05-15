using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Domain.Handlers
{
    public class CourseEventPersisterHandler : IHandle<CourseAssociatedWithProgram>,
                                               IHandle<CourseDisassociatedWithProgram>,
                                               IHandle<CourseCreated>, IHandle<CourseInfoUpdated>,
                                               IHandle<CourseDeleted>, IHandle<CourseSegmentAdded>,
                                               IHandle<CourseSegmentUpdated>
    {
        public void Handle(IDomainEvent domainEvent)
        {
            var store = new CourseEventStore();
            store.Store(domainEvent);
        }
    }
}