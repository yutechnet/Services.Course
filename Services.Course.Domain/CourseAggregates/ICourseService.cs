using System;
using System.Collections.Generic;
using BpeProducts.Common.Authorization;
using BpeProducts.Common.Contract;
using BpeProducts.Services.Authorization.Client;
using BpeProducts.Services.Authorization.Contract;
using BpeProducts.Services.Course.Contract;

namespace BpeProducts.Services.Course.Domain.CourseAggregates
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
        /// Delete a course by marking it deleted
        /// </summary>
        /// <param name="courseId">Course to soft-delete</param>
        void Delete(Guid courseId);

        /// <summary>
        /// Modify the list of prerequisites for an existing course
        /// </summary>
        /// <param name="courseId">Course to update</param>
        /// <param name="newPrerequisiteIds">List of course IDs that serve as the prerequisite list</param>
        void UpdatePrerequisiteList(Guid courseId, List<Guid> newPrerequisiteIds);

        /// <summary>
        /// Returns the list of published courses for an organization
        /// </summary>
        /// <param name="organizationId">Organization to return the published courses from</param>
        /// <returns>List of published courses</returns>
        IEnumerable<CourseInfoResponse> GetPublishedCourses(Guid organizationId);

	    CourseInfoResponse CreateVersion(Guid parentVersionId, string versionNumber);
	    void PublishVersion(Guid courseId, string publishNote);

        [AuthByAcl(Capability = Capability.CourseCreate, OrganizationObject = "request")]
        CourseInfoResponse Create(CreateCourseFromTemplateRequest request);

        void UpdateActiviationStatus(Guid courseId, ActivationRequest request);
    }
}
