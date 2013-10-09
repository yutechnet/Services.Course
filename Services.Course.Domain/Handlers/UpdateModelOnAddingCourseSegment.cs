using System;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Domain.Handlers
{
    public class UpdateModelOnAddingCourseSegment : IHandle<CourseSegmentAdded>
    {
        private readonly ICourseRepository _repository;

        public UpdateModelOnAddingCourseSegment(ICourseRepository repository)
        {
            _repository = repository;
        }

        public void Handle(IDomainEvent domainEvent)
        {
            var e = domainEvent as CourseSegmentAdded;
            if (e == null)
            {
                throw new InvalidOperationException("Invalid domain event.");
            }

            var course = _repository.GetOrThrow(e.AggregateId);
            course.AddSegment(e.SegmentId, e.ParentSegmentId, e.Request);

            _repository.Save(course);
        }
    }
}