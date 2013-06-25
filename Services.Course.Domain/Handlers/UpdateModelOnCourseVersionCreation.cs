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
            _courseRepository.Save(e.NewVersion);
        }
    }
}
