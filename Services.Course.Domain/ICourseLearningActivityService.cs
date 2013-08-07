using System;
using System.Collections.Generic;
using BpeProducts.Services.Course.Contract;

namespace BpeProducts.Services.Course.Domain
{
    public interface ICourseLearningActivityService
    {
        CourseLearningActivityResponse Get(Guid courseId, Guid segmentId, Guid learningActivityId);
        IEnumerable<CourseLearningActivityResponse> Get(Guid courseId, Guid segmentId);
        void Update(Guid courseId, Guid segmentId, Guid learningActivityId, SaveCourseLearningActivityRequest request);
        void Delete(Guid courseId, Guid segmentId, Guid learningActivityId);
        CourseLearningActivityResponse Create(Guid courseId, Guid segmentId, SaveCourseLearningActivityRequest request);
    }
}