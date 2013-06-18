using System;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Domain.Handlers
{
    public class UpdateModelOnCourseVersionPublish : IHandle<CourseVersionPublished>
    {
        private readonly ICourseRepository _courseRepository;
        public UpdateModelOnCourseVersionPublish(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }
        public void Handle(IDomainEvent domainEvent)
        {
            var e = domainEvent as CourseVersionPublished;
            if (e == null)
            {
                throw new InvalidOperationException("Invalid domain event.");
            }
            var courseInDb = _courseRepository.GetById(e.AggregateId);
            courseInDb.IsPublished = true;
            courseInDb.PublishNote = e.PublishNote;
            courseInDb.VersionNumber = e.VersionNumber;

            _courseRepository.Update(courseInDb);
        }
    }
}