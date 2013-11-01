using System;
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
        private readonly ICourseFactory _courseFactory;
        private readonly IDomainEvents _domainEvents;
        private readonly ICourseRepository _courseRepository;

        public CourseSegmentService(ICourseRepository courseRepository, ICourseFactory courseFactory, IDomainEvents domainEvents)
        {
            _courseRepository = courseRepository;
            _courseFactory = courseFactory;
            _domainEvents = domainEvents;
        }

        public IEnumerable<CourseSegmentInfo> Get(Guid courseId)
        {
            var course = _courseRepository.Get(courseId);
            
            return Mapper.Map<IList<CourseSegmentInfo>>(course.Segments);
        }

        public CourseSegmentInfo Get(Guid courseId, Guid segmentId)
        {
            var segment = _courseRepository.GetSegment(courseId, segmentId); 
            if (segment == null)
            {
                throw new NotFoundException(string.Format("Segment {0} for Course {1} is not found.", segmentId, courseId));
            }

            var returnValue = Mapper.Map<CourseSegmentInfo>(segment);
            return returnValue;
        }

        public CourseSegmentInfo Create(Guid courseId, SaveCourseSegmentRequest saveCourseSegmentRequest)
        {
            var course = _courseFactory.Reconstitute(courseId);
            if (course.Id == Guid.Empty || !course.ActiveFlag)
            {
                throw new NotFoundException(string.Format("Course {0} not found.", courseId));
            }

            var parentSegmentId = saveCourseSegmentRequest.ParentSegmentId ?? Guid.Empty;

            var newSegmentId = Guid.NewGuid();
            _domainEvents.Raise<CourseSegmentAdded>(new CourseSegmentAdded
            {
                AggregateId = courseId,
                SegmentId = newSegmentId,
                ParentSegmentId = parentSegmentId,
                Request = saveCourseSegmentRequest
            });

            return new CourseSegmentInfo
                {
                    Name = saveCourseSegmentRequest.Name,
                    Description = saveCourseSegmentRequest.Description,
                    Content = saveCourseSegmentRequest.Content,
                    ParentSegmentId = parentSegmentId,
                    Type = saveCourseSegmentRequest.Type,
                    Id = newSegmentId
                };
        }

        public void Update(Guid courseId, Guid segmentId, SaveCourseSegmentRequest saveCourseSegmentRequest)
        {
			// TODO: Rename Reconstitute to something else since we aren't using the event store anymore
            var course = _courseFactory.Reconstitute(courseId);
            // TODO Check for CourseId no longer needed once an appropriate factory.load is used
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

        public void Delete(Guid courseId, Guid segmentId)
        {
            _domainEvents.Raise<CourseSegmentDeleted>(new CourseSegmentDeleted
                {
                    AggregateId = courseId,
                    SegmentId = segmentId,
                });
        }

        public void Update(Guid courseId, IList<UpdateCourseSegmentRequest> updateCourseSegmentRequest)
        {
            for (int i = 0; i < updateCourseSegmentRequest.Count(); i++)
            {
                _domainEvents.Raise<CourseSegmentReordered>(new CourseSegmentReordered
                    {
                        AggregateId = courseId,
                        SegmentId = updateCourseSegmentRequest[i].Id,
                        Request = updateCourseSegmentRequest[i],
                        DisplayOrder = i
                    });

                if (updateCourseSegmentRequest[i].ChildrenSegments.Count > 0)
                {
                    Update(courseId, updateCourseSegmentRequest[i].ChildrenSegments);
                }
            }
        }
    }
}