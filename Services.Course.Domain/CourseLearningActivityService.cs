using System;
using AutoMapper;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Domain
{
    public class CourseLearningActivityService : ICourseLearningActivityService
    {
        private readonly ICourseRepository _courseRepository;

        public CourseLearningActivityService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public CourseLearningActivityResponse Get(Guid courseId, Guid segmentId, Guid learningActivityId)
        {
            var course = _courseRepository.Get(courseId);

            var learningActivity = course.GetLearningActivity(segmentId, learningActivityId);

            return Mapper.Map<CourseLearningActivityResponse>(learningActivity);
        }

        public void Update(Guid courseId, Guid segmentId, Guid learningActivityId, SaveCourseLearningActivityRequest request)
        {
            var course = _courseRepository.Get(courseId);

            var learningActivity = course.UpdateLearningActivity(segmentId, learningActivityId, request);
        }

        public void Delete(Guid courseId, Guid segmentId, Guid learningActivityId)
        {
            throw new NotImplementedException();
        }

        public CourseLearningActivityResponse Create(Guid courseId, Guid segmentId, SaveCourseLearningActivityRequest request)
        {
            var course = _courseRepository.Get(courseId);

            var learningActivity = course.AddLearningActivity(segmentId, request);

            return Mapper.Map<CourseLearningActivityResponse>(learningActivity);
        }
    }
}