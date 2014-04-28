using System;
using System.Collections.Generic;

namespace BpeProducts.Services.Course.Domain.CourseAggregates
{
    public interface ICourseRepository
    {
        Course Get(Guid courseId);
        Course GetOrThrow(Guid courseId);
        void Save(Course course);
        void Delete(Course course);
        Course GetVersion(Guid originalCourseId, string versionNumber);
        IList<Course> ODataQuery(string queryString);
        IList<Course> Get(List<Guid> ids);
        CourseSegment GetSegment(Guid courseId,Guid segmentId);
        LearningMaterial GetLearningMaterial(Guid learningMaterialId);
        IEnumerable<Course> GetPublishedCourses(Guid organizationId);
    }
}
