using System;
using AutoMapper;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Domain.Handlers
{
    public class UpdateModelOnAddingCourseSegment : IHandle<CourseSegmentAdded>
    {
        private readonly IRepository _repository;

        public UpdateModelOnAddingCourseSegment(IRepository repository)
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

            var courseInDb = _repository.Get<Entities.Course>(e.AggregateId);
            var newSegment = Mapper.Map<CourseSegment>(e);
            if (newSegment.ParentSegmentId == Guid.Empty)
            {
                courseInDb.Segments.Add(newSegment);
            }
            else
            {
                var parentSegment = courseInDb.SegmentIndex[e.ParentSegmentId];
                parentSegment.AddSubSegment(newSegment);
            }
            _repository.Save(courseInDb);
        }
    }
}