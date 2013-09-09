using System;
using System.Collections.Generic;
using BpeProducts.Services.Course.Contract;

namespace BpeProducts.Services.Course.Domain
{
    public interface ICourseSegmentService
    {
        /// <summary>
        /// Get the entire tree of course segments for the given course
        /// </summary>
        /// <param name="courseId">Parent course</param>
        /// <returns>Nested list of course segments</returns>
        IEnumerable<CourseSegmentInfo> Get(Guid courseId);

        /// <summary>
        /// Get the specific course segment from a course
        /// </summary>
        /// <param name="courseId">Parent course</param>
        /// <param name="segmentId">Segment id</param>
        /// <returns>Course segment info</returns>
        CourseSegmentInfo Get(Guid courseId, Guid segmentId);

        /// <summary>
        /// Create a root-level course segment
        /// </summary>
        /// <param name="courseId">Parent course</param>
        /// <param name="saveCourseSegmentRequest">Request object</param>
        /// <returns>Course segment created</returns>
        CourseSegmentInfo Create(Guid courseId, SaveCourseSegmentRequest saveCourseSegmentRequest);

        /// <summary>
        /// Update a segment (root or non-root)
        /// </summary>
        /// <param name="courseId">Parent course</param>
        /// <param name="segmentId">Segment to update</param>
        /// <param name="saveCourseSegmentRequest">Request object</param>
        void Update(Guid courseId, Guid segmentId, SaveCourseSegmentRequest saveCourseSegmentRequest);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="segmentId"></param>
        void Delete(Guid courseId, Guid segmentId);

        void Update(Guid courseId, IList<UpdateCourseSegmentRequest> updateCourseSegmentRequest);
    }
}