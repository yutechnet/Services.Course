using System;
using System.Linq;
using AutoMapper;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Domain.Handlers
{
    public class UpdateModelOnDeletingCourseLearningActivity : IHandle<CourseLearningActivityDeleted>
    {
        private readonly IRepository _repository;

        public UpdateModelOnDeletingCourseLearningActivity(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(IDomainEvent domainEvent)
        {
            var e = domainEvent as CourseLearningActivityDeleted;
            if (e == null)
            {
                throw new InvalidOperationException("Invalid domain event.");
            }

            var courseInDb = _repository.Get<Courses.Course>(e.AggregateId);
            
            courseInDb.DeleteLearningActivity(e.SegmentId, e.LearningActivityId);

            _repository.Save(courseInDb);
        }
    }
}