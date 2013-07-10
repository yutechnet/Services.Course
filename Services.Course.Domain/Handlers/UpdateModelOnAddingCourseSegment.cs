﻿using System;
using AutoMapper;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Domain.Handlers
{
    public class UpdateModelOnAddingCourseSegment : IHandle<CourseSegmentAdded>
    {
        private readonly ICourseRepository _courseRepository;

        public UpdateModelOnAddingCourseSegment(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public void Handle(IDomainEvent domainEvent)
        {
            var e = domainEvent as CourseSegmentAdded;
            if (e == null)
            {
                throw new InvalidOperationException("Invalid domain event.");
            }

            Entities.Course courseInDb = _courseRepository.Get(e.AggregateId);
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
            _courseRepository.Save(courseInDb);
        }
    }
}