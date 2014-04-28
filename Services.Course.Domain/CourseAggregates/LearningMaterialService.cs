using System;
using AutoMapper;
using BpeProducts.Common.Authorization;
using BpeProducts.Services.Authorization.Contract;
using BpeProducts.Services.Course.Contract;

namespace BpeProducts.Services.Course.Domain.CourseAggregates
{
    public interface ILearningMaterialService
    {
        LearningMaterialInfo AddLearningMaterial(Guid courseId, LearningMaterialRequest request);
        void UpdateLearningMaterial(Guid courseId, Guid learningMaterialId, UpdateLearningMaterialRequest request);
        LearningMaterialInfo Get(Guid courseId, Guid learningMaterialId);
        void Delete(Guid courseId, Guid learningMaterialId);

        LearningMaterialInfo AddLearningMaterial(Guid courseId, Guid segmentId, LearningMaterialRequest request);
        void UpdateLearningMaterial(Guid courseId, Guid segmentId, Guid learningMaterialId, UpdateLearningMaterialRequest request);
        LearningMaterialInfo Get(Guid courseId, Guid segmentId, Guid learningMaterialId);
        void Delete(Guid courseId, Guid segmentId, Guid learningMaterialId);
    }

    public class LearningMaterialService : ILearningMaterialService
    {
        private readonly ICourseRepository _courseRepository;

        public LearningMaterialService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        [AuthByAcl(Capability = Capability.EditCourse, ObjectId = "courseId", ObjectType = typeof(Course))]
        public LearningMaterialInfo AddLearningMaterial(Guid courseId, LearningMaterialRequest request)
        {
            var course = _courseRepository.GetOrThrow(courseId);

            var learningMaterial = course.AddLearningMaterial(request);

            return Mapper.Map<LearningMaterialInfo>(learningMaterial);
        }

        [AuthByAcl(Capability = Capability.CourseView, ObjectId = "courseId", ObjectType = typeof(Course))]
        public LearningMaterialInfo Get(Guid courseId, Guid learningMaterialId)
        {
            var learningMaterial = _courseRepository.GetLearningMaterial(learningMaterialId);
            return Mapper.Map<LearningMaterialInfo>(learningMaterial);
        }

        [AuthByAcl(Capability = Capability.EditCourse, ObjectId = "courseId", ObjectType = typeof(Course))]
        public void Delete(Guid courseId, Guid learningMaterialId)
        {
            var course = _courseRepository.GetOrThrow(courseId);
            course.DeleteLearningMaterial(learningMaterialId);
            _courseRepository.Save(course);
        }

        [AuthByAcl(Capability = Capability.EditCourse, ObjectId = "courseId", ObjectType = typeof(Course))]
        public void UpdateLearningMaterial(Guid courseId, Guid learningMaterialId, UpdateLearningMaterialRequest request)
        {
            var course = _courseRepository.GetOrThrow(courseId);
            course.UpdateLearningMaterial(learningMaterialId, request);

            _courseRepository.Save(course);
        }

        [AuthByAcl(Capability = Capability.EditCourse, ObjectId = "courseId", ObjectType = typeof(Course))]
        public LearningMaterialInfo AddLearningMaterial(Guid courseId, Guid segmentId, LearningMaterialRequest request)
        {
            var course = _courseRepository.GetOrThrow(courseId);

            //   var libraryInfo = _assetService.AddAssetToLibrary("course", course.Id, request.AssetId);

            var learningMaterial = course.AddLearningMaterial(segmentId, request);

            return Mapper.Map<LearningMaterialInfo>(learningMaterial);
        }

        [AuthByAcl(Capability = Capability.CourseView, ObjectId = "courseId", ObjectType = typeof(Course))]
        public LearningMaterialInfo Get(Guid courseId, Guid segmentId, Guid learningMaterialId)
        {
            var learningMaterial = _courseRepository.GetLearningMaterial(learningMaterialId);
            return Mapper.Map<LearningMaterialInfo>(learningMaterial);
        }

        [AuthByAcl(Capability = Capability.EditCourse, ObjectId = "courseId", ObjectType = typeof(Course))]
        public void Delete(Guid courseId, Guid segmentId, Guid learningMaterialId)
        {
            var course = _courseRepository.GetOrThrow(courseId);
            course.DeleteLearningMaterial(segmentId, learningMaterialId);

            _courseRepository.Save(course);
        }

        [AuthByAcl(Capability = Capability.EditCourse, ObjectId = "courseId", ObjectType = typeof(Course))]
        public void UpdateLearningMaterial(Guid courseId, Guid segmentId, Guid learningMaterialId, UpdateLearningMaterialRequest request)
        {
            var course = _courseRepository.GetOrThrow(courseId);
            course.UpdateLearningMaterial(segmentId, learningMaterialId, request);

            _courseRepository.Save(course);
        }
    }
}
