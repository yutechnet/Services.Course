using System;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Domain.Handlers
{
    public class UpdateModelOnCourseVersionPublish : IHandle<CourseVersionPublished>
    {
        private readonly IRepository _repository;

        public UpdateModelOnCourseVersionPublish(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(IDomainEvent domainEvent)
        {
            var e = domainEvent as CourseVersionPublished;
            if (e == null)
            {
                throw new InvalidOperationException("Invalid domain event.");
            }

            var course = _repository.Get<Entities.Course>(e.AggregateId);
            course.Publish(e.PublishNote);

            _repository.Save(course);
        }
    }
}