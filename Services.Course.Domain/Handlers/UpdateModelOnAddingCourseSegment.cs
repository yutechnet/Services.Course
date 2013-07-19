using System;
using System.Collections.Generic;
using AutoMapper;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;
using CourseSegment = BpeProducts.Services.Course.Domain.Courses.CourseSegment;

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

            var course = _repository.Get<Entities.Course>(e.AggregateId);
            course.AddSegment(e.SegmentId, e.ParentSegmentId, e.Request);

            _repository.Save(course);
        }
    }
}