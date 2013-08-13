using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData.Query;
using BpeProducts.Services.Course.Contract;

namespace BpeProducts.Services.Course.Domain
{
    public interface ICourseService
    {
        /// <summary>
        /// Create a new course, either from scratch or from a template
        /// </summary>
        /// <param name="request">Request object</param>
        /// <returns>Course info</returns>
        CourseInfoResponse Create(SaveCourseRequest request);

        /// <summary>
        /// Modify an existing course
        /// </summary>
        /// <param name="courseId">Course to update</param>
        /// <param name="request">Request object</param>
        void Update(Guid courseId, UpdateCourseRequest request);

        /// <summary>
        /// Get the course
        /// </summary>
        /// <param name="courseId">Course to get</param>
        /// <returns>Course info object</returns>
        CourseInfoResponse Get(Guid courseId);

        /// <summary>
        /// Search for a course using OData query string
        /// </summary>
        /// <param name="queryString">oData Query string</param>
        /// <returns>list of course info</returns>
        IEnumerable<CourseInfoResponse> Search(string queryString);

        /// <summary>
        /// Delete a course by marking it inactive
        /// </summary>
        /// <param name="courseId">Course to soft-delete</param>
        void Delete(Guid courseId);

        /// <summary>
        /// Modify the list of prerequisites for an existing course
        /// </summary>
        /// <param name="courseId">Course to update</param>
        /// <param name="prerequisiteIds">List of course IDs that serve as the prerequisite list</param>
        void UpdatePrerequisiteList(Guid courseId, List<Guid> prerequisiteIds);
    }
}
