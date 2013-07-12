using System;
using System.Collections.Generic;
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

            return course.Segments;
        }

        public CourseSegment Get(Guid courseId, Guid segmentId)
        {
            var course = _repository.Get<Entities.Course>(courseId);
            if (course == null || !course.ActiveFlag)
            {
                throw new NotFoundException(string.Format("Course {0} not found.", courseId));
            }

            return course.SegmentIndex[segmentId];
        }

        public IEnumerable<CourseSegment> GetSubSegments(Guid courseId, Guid segmentId)
        {
            var course = _repository.Get<Entities.Course>(courseId);
            if (course == null || !course.ActiveFlag)
            {
                throw new NotFoundException(string.Format("Course {0} not found.", courseId));
            }

            return course.SegmentIndex[segmentId].ChildrenSegments;
        }

        public CourseSegment Create(Guid courseId, SaveCourseSegmentRequest saveCourseSegmentRequest)
        {
            var course = _courseFactory.Reconstitute(courseId);
            // TODO Need to validate this. Does the Event Store return a null object or Id with Guid.Empty?
            if (course.Id == Guid.Empty || !course.ActiveFlag)
            {
                throw new NotFoundException(string.Format("Course {0} not found.", courseId));
            }

            var newSegmentId = Guid.NewGuid();
            // TODO: Embed the object in the message
            _domainEvents.Raise<CourseSegmentAdded>(new CourseSegmentAdded
            {
                AggregateId = courseId,
                Description = saveCourseSegmentRequest.Description,
                DiscussionId = saveCourseSegmentRequest.DiscussionId,
                Name = saveCourseSegmentRequest.Name,
                ParentSegmentId = Guid.Empty,
                Id = newSegmentId,
                Type = saveCourseSegmentRequest.Type
            });

            return new CourseSegment
                {
                    Description = saveCourseSegmentRequest.Description,
                    DiscussionId = saveCourseSegmentRequest.DiscussionId,
                    Name = saveCourseSegmentRequest.Name,
                    ParentSegmentId = Guid.Empty,
                    Id = newSegmentId,
                    Type = saveCourseSegmentRequest.Type
                };
        }

        public CourseSegment Create(Guid courseId, Guid segmentId, SaveCourseSegmentRequest saveCourseSegmentRequest)
        {
            var course = _courseFactory.Reconstitute(courseId);
            // TODO Need to validate this. Does the Event Store return a null object or Id with Guid.Empty?
            if (course.Id == Guid.Empty || !course.ActiveFlag)
            {
                throw new NotFoundException(string.Format("Course {0} not found.", courseId));
            }

            var newSegmentId = Guid.NewGuid();
            // TODO: Embed the object in the message
            _domainEvents.Raise<CourseSegmentAdded>(new CourseSegmentAdded
            {
                AggregateId = courseId,
                Name = saveCourseSegmentRequest.Name,
                Description = saveCourseSegmentRequest.Description,
                DiscussionId = saveCourseSegmentRequest.DiscussionId,
                ParentSegmentId = segmentId,
                Type = saveCourseSegmentRequest.Type,
                Id = newSegmentId
            });

            return new CourseSegment
                {
                    Name = saveCourseSegmentRequest.Name,
                    Description = saveCourseSegmentRequest.Description,
                    DiscussionId = saveCourseSegmentRequest.DiscussionId,
                    ParentSegmentId = segmentId,
                    Type = saveCourseSegmentRequest.Type,
                    Id = newSegmentId
                };
        }

        public void Update(Guid courseId, Guid segmentId, SaveCourseSegmentRequest saveCourseSegmentRequest)
        {
            var course = _courseFactory.Reconstitute(courseId);
            // TODO Need to validate this. Does the Event Store return a null object or Id with Guid.Empty?
            if (course.Id == Guid.Empty || !course.ActiveFlag)
            {
                throw new NotFoundException(string.Format("Course {0} not found.", courseId));
            }

            var courseSegment = course.SegmentIndex[segmentId];
            Mapper.Map(saveCourseSegmentRequest, courseSegment);
            courseSegment.Id = segmentId;

            _domainEvents.Raise<CourseSegmentUpdated>(new CourseSegmentUpdated
            {
                AggregateId = courseId,
                Description = courseSegment.Description,
                Name = courseSegment.Name,
                ParentSegmentId = courseSegment.ParentSegmentId,
                SegmentId = courseSegment.Id,
                Type = courseSegment.Type,
                Content = courseSegment.Content
            });
        }

        public void Update(Guid courseId, Guid segmentId, IEnumerable<SaveCourseSegmentRequest> childrentSegments)
        {
            throw new NotImplementedException();
        }
    }
}