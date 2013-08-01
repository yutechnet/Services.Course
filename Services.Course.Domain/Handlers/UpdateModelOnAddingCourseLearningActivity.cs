using System;
using System.Collections.Generic;
using AutoMapper;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;
using CourseLearningActivity = BpeProducts.Services.Course.Domain.Courses.CourseLearningActivity;


namespace BpeProducts.Services.Course.Domain.Handlers
{
    public class UpdateModelOnAddingCourseLearningActivity : IHandle<CourseLearningActivityAdded>
    {
        private readonly IRepository _repository;

        public UpdateModelOnAddingCourseLearningActivity(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(IDomainEvent domainEvent)
        {
            var e = domainEvent as CourseLearningActivityAdded;
            if (e == null)
            {
                throw new InvalidOperationException("Invalid domain event.");
            }

            var course = _repository.Get<Courses.Course>(e.AggregateId);
            course.AddLearningActivity(e.SegmentId, e.Request);

            _repository.Save(course);
        }
    }
}