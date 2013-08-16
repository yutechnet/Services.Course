using System;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Domain.Handlers
{
    public class UpdateModelOnDeletingCourseSegment : IHandle<CourseSegmentDeleted>
    {
        private readonly ICourseRepository _repository;

        public UpdateModelOnDeletingCourseSegment(ICourseRepository repository)
        {
            _repository = repository;
        }

        public void Handle(IDomainEvent domainEvent)
        {
            var e = domainEvent as CourseSegmentDeleted;
            if (e == null)
            {
                throw new InvalidOperationException("Invalid domain event.");
            }

            var courseInDb = _repository.Get(e.AggregateId);
            courseInDb.DeleteSegment(e.SegmentId);

            _repository.Save(courseInDb);
        }
    }
}