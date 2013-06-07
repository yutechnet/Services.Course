using System;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Domain.Handlers
{
    public class UpdateModelOnCourseVersionCreation : IHandle<CourseVersionCreated>
    {
        private readonly ICourseRepository _courseRepository;
        public UpdateModelOnCourseVersionCreation(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public void Handle(IDomainEvent domainEvent)
        {
            var e = domainEvent as CourseVersionCreated;
            if (e == null)
            {
                throw new InvalidOperationException("Invalid domain event.");
            }

            // create a new version based on the parent version
            var courseInDb = _courseRepository.GetById(e.ParentCourseId);
            courseInDb.Id = e.AggregateId;
            courseInDb.OriginalEntityId = e.OriginalCourseId;
            courseInDb.ParentEntityId = e.ParentCourseId;

            courseInDb.IsPublished = e.IsPublished;
            courseInDb.VersionNumber = e.VersionNumber;

            _courseRepository.Add(courseInDb);
        }
    }
}