﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using AutoMapper;
using BpeProducts.Common.Exceptions;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Domain
{
    public class CourseSegmentService : ICourseSegmentService
    {
        private readonly IRepository _repository;
        private readonly ICourseFactory _courseFactory;
        private readonly IDomainEvents _domainEvents;

        public CourseSegmentService(IRepository repository, ICourseFactory courseFactory, IDomainEvents domainEvents)
        {
            _repository = repository;
            _courseFactory = courseFactory;
            _domainEvents = domainEvents;
        }

        public IEnumerable<CourseSegment> Get(Guid courseId)
        {
            var course = _repository.Get<Entities.Course>(courseId);
            if (course == null || !course.ActiveFlag)
            {
                throw new NotFoundException(string.Format("Course {0} not found.", courseId));
            }

            return Mapper.Map<IList<CourseSegment>>(course.Segments);
        }

        public CourseSegment Get(Guid courseId, Guid segmentId)
        {
            var course = _repository.Get<Entities.Course>(courseId);
            if (course == null || !course.ActiveFlag)
            {
                throw new NotFoundException(string.Format("Course {0} not found.", courseId));
            }

            var segment = course.Segments.FirstOrDefault(s => s.Id == segmentId);
            if (segment == null)
            {
                throw new NotFoundException(string.Format("Segment {0} for Course {1} is not found.", segmentId, courseId));
            }

            var returnValue = Mapper.Map<CourseSegment>(segment);
            return returnValue;
        }

        public IEnumerable<CourseSegment> GetSubSegments(Guid courseId, Guid segmentId)
        {
            var course = _repository.Get<Entities.Course>(courseId);
            if (course == null || !course.ActiveFlag)
            {
                throw new NotFoundException(string.Format("Course {0} not found.", courseId));
            }

            var segment = course.Segments.FirstOrDefault(s => s.Id == segmentId);
            if (segment == null)
            {
                throw new NotFoundException(string.Format("Segment {0} for Course {1} is not found.", segmentId, courseId));
            }

            return Mapper.Map<IList<CourseSegment>>(segment.ChildSegments);
        }

        public CourseSegment Create(Guid courseId, SaveCourseSegmentRequest saveCourseSegmentRequest)
        {
            var course = _courseFactory.Reconstitute(courseId);
            // TODO Need to validate this. Does the Event Store return a null object or SegmentId with Guid.Empty?
            if (course.Id == Guid.Empty || !course.ActiveFlag)
            {
                throw new NotFoundException(string.Format("Course {0} not found.", courseId));
            }

            var newSegmentId = Guid.NewGuid();
            // TODO: Embed the object in the message
            _domainEvents.Raise<CourseSegmentAdded>(new CourseSegmentAdded
            {
                AggregateId = courseId,
                SegmentId = newSegmentId,
                ParentSegmentId = Guid.Empty,
                Request = saveCourseSegmentRequest
            });

            return new CourseSegment
                {
                    Description = saveCourseSegmentRequest.Description,
                    Content = saveCourseSegmentRequest.Content ?? new List<Content>(),
                    Name = saveCourseSegmentRequest.Name,
                    ParentSegmentId = Guid.Empty,
                    Id = newSegmentId,
                    Type = saveCourseSegmentRequest.Type
                };
        }

        public CourseSegment Create(Guid courseId, Guid segmentId, SaveCourseSegmentRequest saveCourseSegmentRequest)
        {
            var course = _courseFactory.Reconstitute(courseId);
            // TODO Need to validate this. Does the Event Store return a null object or SegmentId with Guid.Empty?
            if (course.Id == Guid.Empty || !course.ActiveFlag)
            {
                throw new NotFoundException(string.Format("Course {0} not found.", courseId));
            }

            var newSegmentId = Guid.NewGuid();
            // TODO: Embed the object in the message
            _domainEvents.Raise<CourseSegmentAdded>(new CourseSegmentAdded
            {
                AggregateId = courseId,
                SegmentId = newSegmentId,
                ParentSegmentId = segmentId,
                Request = saveCourseSegmentRequest
            });

            return new CourseSegment
                {
                    Name = saveCourseSegmentRequest.Name,
                    Description = saveCourseSegmentRequest.Description,
                    Content = saveCourseSegmentRequest.Content,
                    ParentSegmentId = segmentId,
                    Type = saveCourseSegmentRequest.Type,
                    Id = newSegmentId
                };
        }

        public void Update(Guid courseId, Guid segmentId, SaveCourseSegmentRequest saveCourseSegmentRequest)
        {
            var course = _courseFactory.Reconstitute(courseId);
            // TODO Need to validate this. Does the Event Store return a null object or SegmentId with Guid.Empty?
            if (course.Id == Guid.Empty || !course.ActiveFlag)
            {
                throw new NotFoundException(string.Format("Course {0} not found.", courseId));
            }

            _domainEvents.Raise<CourseSegmentUpdated>(new CourseSegmentUpdated
            {
                AggregateId = courseId,
                SegmentId = segmentId,
                Request = saveCourseSegmentRequest
            });
        }

        public void Update(Guid courseId, Guid segmentId, IEnumerable<SaveCourseSegmentRequest> childrentSegments)
        {
            throw new NotImplementedException();
        }
    }
}