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
    public interface IContentService
    {
        LearningMaterialInfo AddContent(Guid courseId, Guid segmentId, Guid learningActivityId, ContentRequest request);
        LearningMaterialInfo Get(Guid courseId, Guid segmentId, Guid learningActivityId, Guid contentId);
    }

    public class ContentService : IContentService
    {
        private readonly ICourseRepository _courseRepository;

        public ContentService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public LearningMaterialInfo AddContent(Guid courseId, Guid segmentId, Guid learningActivityId, ContentRequest request)
        {
            var course = _courseRepository.GetOrThrow(courseId);

            var content = course.AddLearningMaterial(segmentId, learningActivityId, request);
            return Mapper.Map<LearningMaterialInfo>(content);
        }

        public LearningMaterialInfo Get(Guid courseId, Guid segmentId, Guid learningActivityId, Guid contentId)
        {
            var content = _courseRepository.GetContent(contentId);
            return Mapper.Map<LearningMaterialInfo>(content);
        }
    }
}
