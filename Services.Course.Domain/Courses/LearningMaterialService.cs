using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BpeProducts.Services.Asset.Contracts;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Domain.Courses
{
    public interface ILearningMaterialService
    {
        LearningMaterialInfo AddLearningMaterial(Guid courseId, Guid segmentId, LearningMaterialRequest request);
        void UpdateLearningMaterial(Guid courseId, Guid segmentId, Guid learningMaterialId, UpdateLearningMaterialRequest request);
        LearningMaterialInfo Get(Guid courseId, Guid segmentId, Guid learningMaterialId);
        void Delete(Guid courseId, Guid segmentId, Guid learningMaterialId);
    }

    public class LearningMaterialService : ILearningMaterialService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IAssetServiceClient _assetService;

        public LearningMaterialService(ICourseRepository courseRepository, IAssetServiceClient assetService)
        {
            _courseRepository = courseRepository;
            _assetService = assetService;
        }

        public LearningMaterialInfo AddLearningMaterial(Guid courseId, Guid segmentId, LearningMaterialRequest request)
        {
            var course = _courseRepository.GetOrThrow(courseId);

            //   var libraryInfo = _assetService.AddAssetToLibrary("course", course.Id, request.AssetId);

            var learningMaterial = course.AddLearningMaterial(segmentId, request);

            return Mapper.Map<LearningMaterialInfo>(learningMaterial);
        }

        public LearningMaterialInfo Get(Guid courseId, Guid segmentId, Guid learningMaterialId)
        {
            var learningMaterial = _courseRepository.GetLearningMaterial(learningMaterialId);
            return Mapper.Map<LearningMaterialInfo>(learningMaterial);
        }

        public void Delete(Guid courseId, Guid segmentId, Guid learningMaterialId)
        {
            var course = _courseRepository.GetOrThrow(courseId);
            course.DeleteLearningMaterial(segmentId, learningMaterialId);

            _courseRepository.Save(course);
        }


        public void UpdateLearningMaterial(Guid courseId, Guid segmentId, Guid learningMaterialId, UpdateLearningMaterialRequest request)
        {
            var course = _courseRepository.GetOrThrow(courseId);
            course.UpdateLearningMaterial(segmentId, learningMaterialId, request);

            _courseRepository.Save(course);
        }
    }
}
