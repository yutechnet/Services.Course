using System;
using System.Linq;
using AutoMapper;
using System.Collections.Generic;
using BpeProducts.Common.Exceptions;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Courses;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Domain
{
    public class CourseLearningActivityService : ICourseLearningActivityService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IDomainEvents _domainEvents;

        public CourseLearningActivityService(ICourseRepository courseRepository, IDomainEvents domainEvents)
        {
            _courseRepository = courseRepository;
            _domainEvents = domainEvents;
        }

        public CourseLearningActivityResponse Get(Guid courseId, Guid segmentId, Guid learningActivityId)
        {
            var course = _courseRepository.Get(courseId);

            var learningActivity = course.GetLearningActivity(segmentId, learningActivityId);
     
            return Mapper.Map<CourseLearningActivityResponse>(learningActivity);
        }

        public IEnumerable<CourseLearningActivityResponse> Get(Guid courseId, Guid segmentId)
        {
            var course = _courseRepository.Get(courseId);

            var learningActivity = course.GetLearningActivity(segmentId);

            return Mapper.Map<IList<CourseLearningActivityResponse>>(learningActivity);
        }

        public void Update(Guid courseId, Guid segmentId, Guid learningActivityId, SaveCourseLearningActivityRequest request)
        {

            _domainEvents.Raise<CourseLearningActivityUpdated>(new CourseLearningActivityUpdated
                {
                    AggregateId = courseId, 
                    SegmentId = segmentId,
                    LearningActivityId = learningActivityId,
                    Request = request
                });
        }

        public void Delete(Guid courseId, Guid segmentId, Guid learningActivityId)
        {
            _domainEvents.Raise<CourseLearningActivityDeleted>(new CourseLearningActivityDeleted
            {
                AggregateId = courseId,
                SegmentId = segmentId,
                LearningActivityId = learningActivityId
            });
        }

        public CourseLearningActivityResponse Create(Guid courseId, Guid segmentId, SaveCourseLearningActivityRequest request)
        {
            var learningActivityId = Guid.NewGuid();

            _domainEvents.Raise<CourseLearningActivityAdded>(new CourseLearningActivityAdded
                {
                    AggregateId = courseId,
                    LearningActivityId = learningActivityId,
                    Request = request,
                    SegmentId = segmentId
                });

            var response = new CourseLearningActivityResponse
                {
                    Id = learningActivityId,
                    Name = request.Name,
                    Type = request.Type,
                    IsGradeable = request.IsGradeable,
                    IsExtraCredit = request.IsExtraCredit,
                    Weight = request.Weight,
                    MaxPoint = request.MaxPoint,
                    ObjectId = request.ObjectId,
                    ActiveDate = request.ActiveDate,
                    InactiveDate = request.InactiveDate,
                    DueDate = request.DueDate
                };
            return response;
        }
    }

}