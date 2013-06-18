using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
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

            var newCourseVersion = new Entities.Course(courseInDb);
            newCourseVersion.Id = e.AggregateId;
            newCourseVersion.OriginalEntityId = e.OriginalCourseId;
            newCourseVersion.ParentEntityId = e.ParentCourseId;

            newCourseVersion.IsPublished = e.IsPublished;
            newCourseVersion.VersionNumber = e.VersionNumber;

            _courseRepository.Add(newCourseVersion);
        }
    }
}
