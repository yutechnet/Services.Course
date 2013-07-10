using System;
using AutoMapper;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Domain.Handlers
{
    public class UpdateModelOnUpdatingCourseSegment : IHandle<CourseSegmentUpdated>
    {
        private readonly ICourseRepository _courseRepository;

        public UpdateModelOnUpdatingCourseSegment(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public void Handle(IDomainEvent domainEvent)
        {
            var e = domainEvent as CourseSegmentUpdated;
            if (e == null)
            {
                throw new InvalidOperationException("Invalid domain event.");
            }

            Entities.Course courseInDb = _courseRepository.Get(e.AggregateId);
            var courseSegment = courseInDb.SegmentIndex[e.SegmentId];
            courseSegment.Name = e.Name;
            courseSegment.Description = e.Description;
            courseSegment.Type = e.Type;
            courseSegment.Content = e.Content;

            _courseRepository.Save(courseInDb);
        }
    }
}