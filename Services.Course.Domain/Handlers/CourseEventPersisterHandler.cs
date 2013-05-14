using System;
using AutoMapper;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Domain.Handlers
{
    public class CourseEventPersisterHandler : IHandle<CourseAssociatedWithProgram>,
                                               IHandle<CourseDisassociatedWithProgram>,
                                               IHandle<CourseCreated>, IHandle<CourseInfoUpdated>,
                                               IHandle<CourseDeleted>, IHandle<CourseSegmentAdded>,
                                               IHandle<CourseSegmentUpdated>
    {
        public void Handle(IDomainEvent domainEvent)
        {
            var store = new CourseEventStore();
            store.Store(domainEvent);
        }
    }

    public class UpdateModelOnAddingCourseSegment:IHandle<CourseSegmentAdded>
    {
        private readonly ICourseRepository _courseRepository;
        public UpdateModelOnAddingCourseSegment(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }
        public void Handle(IDomainEvent domainEvent)
        {

            var e = (CourseSegmentAdded) domainEvent;
            Entities.Course courseInDb = _courseRepository.GetById(e.AggregateId);
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
            _courseRepository.Update(courseInDb);
        }
    }
}