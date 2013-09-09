using System;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Domain.Handlers
{
    public class UpdateModelOnReorderingCourseSegment : IHandle<CourseSegmentReordered>
    {
        private readonly ICourseRepository _repository;

        public UpdateModelOnReorderingCourseSegment(ICourseRepository repository)
        {
            _repository = repository;
        }

        public void Handle(IDomainEvent domainEvent)
        {
            var e = domainEvent as CourseSegmentReordered;
            if (e == null)
            {
                throw new InvalidOperationException("Invalid domain event.");
            }

            var courseInDb = _repository.Get(e.AggregateId);
            courseInDb.ReorderSegment(e.SegmentId, e.Request, e.DisplayOrder);

            _repository.Save(courseInDb);
        }
    }
}