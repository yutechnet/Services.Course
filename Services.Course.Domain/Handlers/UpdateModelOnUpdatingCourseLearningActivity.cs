using System;
using System.Linq;
using AutoMapper;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Domain.Handlers
{
    public class UpdateModelOnUpdatingCourseLearningActivity : IHandle<CourseLearningActivityUpdated>
    {
        private readonly IRepository _repository;

        public UpdateModelOnUpdatingCourseLearningActivity(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(IDomainEvent domainEvent)
        {
            var e = domainEvent as CourseLearningActivityUpdated;
            if (e == null)
            {
                throw new InvalidOperationException("Invalid domain event.");
            }

            var courseInDb = _repository.Get<Courses.Course>(e.AggregateId);
            courseInDb.UpdateLearningActivity(e.SegmentId, e.LearningActivityId, e.Request);

            _repository.Save(courseInDb);
        }
    }
}