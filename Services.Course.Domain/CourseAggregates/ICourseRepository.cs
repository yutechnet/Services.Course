using System;
using System.Collections.Generic;

namespace BpeProducts.Services.Course.Domain.Courses
{
    public interface ICourseRepository
    {
        Courses.Course Get(Guid courseId);
        Courses.Course GetOrThrow(Guid courseId);
        void Save(Courses.Course course);
        void Delete(Courses.Course course);
        Courses.Course GetVersion(Guid originalCourseId, string versionNumber);
        IList<Courses.Course> ODataQuery(string queryString);
        IList<Courses.Course> Get(List<Guid> ids);
        CourseSegment GetSegment(Guid courseId,Guid segmentId);
        LearningMaterial GetLearningMaterial(Guid learningMaterialId);
        IEnumerable<Courses.Course> GetPublishedCourses(Guid organizationId);
    }
}
