using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Domain.Courses
{
    public interface ILearningMaterialService
    {
        LearningMaterialInfo AddLearningMaterial(Guid courseId, Guid segmentId, Guid learningActivityId, LearningMaterialRequest request);
        LearningMaterialInfo Get(Guid courseId, Guid segmentId, Guid learningActivityId, Guid learningMaterialId);
    }

    public class LearningMaterialService : ILearningMaterialService
    {
        private readonly ICourseRepository _courseRepository;

        public LearningMaterialService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public LearningMaterialInfo AddLearningMaterial(Guid courseId, Guid segmentId, Guid learningActivityId, LearningMaterialRequest request)
        {
            var course = _courseRepository.GetOrThrow(courseId);

            var learningMaterial = course.AddLearningMaterial(segmentId, learningActivityId, request);
            return Mapper.Map<LearningMaterialInfo>(learningMaterial);
        }

        public LearningMaterialInfo Get(Guid courseId, Guid segmentId, Guid learningActivityId, Guid learningMaterialId)
        {
            var learningMaterial = _courseRepository.GetLearningMaterial(learningMaterialId);
            return Mapper.Map<LearningMaterialInfo>(learningMaterial);
        }
    }
}
