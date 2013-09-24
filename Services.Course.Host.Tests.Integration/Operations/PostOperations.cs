using BpeProducts.Common.WebApiTest.Framework;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources;
using System.Net.Http;

namespace BpeProducts.Services.Course.Host.Tests.Integration.Operations
{
    public static class PostOperations
    {
        public static HttpResponseMessage CreateCourse(string name, SaveCourseRequest request)
        {
            request.TenantId = ApiFeature.TenantId;
            var requestUri = ApiFeature.LeadingPath + "/course";
            return ApiFeature.ApiTestHost.Post<CourseResource, SaveCourseRequest>(name, requestUri, request);
        }

        public static HttpResponseMessage CreateCourseVersion(string name, VersionRequest request)
        {
            var requestUri = ApiFeature.LeadingPath + "/course/version";
            return ApiFeature.ApiTestHost.Post<CourseResource, VersionRequest>(name, requestUri, request);
        }

        public static HttpResponseMessage CreateProgram(string name, SaveProgramRequest request)
        {
            request.TenantId = ApiFeature.TenantId;
            var requestUri = ApiFeature.LeadingPath + "/program";
            return ApiFeature.ApiTestHost.Post<ProgramResource, SaveProgramRequest>(name, requestUri, request);
        }

        public static HttpResponseMessage CreateSegment(string name, CourseResource course, SaveCourseSegmentRequest request)
        {
            request.TenantId = ApiFeature.TenantId;
            var requestUri = string.Format("{0}/course/{1}/segments", ApiFeature.LeadingPath, course.Id);
            return ApiFeature.ApiTestHost.Post<CourseSegmentResource, SaveCourseSegmentRequest>(name, requestUri, request);
        }

        public static HttpResponseMessage CreateLearningOutcome(string name, OutcomeRequest request)
        {
            request.TenantId = ApiFeature.TenantId;
            var requestUri = ApiFeature.LeadingPath + "/outcome";
            return ApiFeature.ApiTestHost.Post<LearningOutcomeResource, OutcomeRequest>(name, requestUri, request);
        }

        public static HttpResponseMessage CreateLearningOutcomeVersion(string name, LearningOutcomeResource learningOutcome, VersionRequest request)
        {
            var requestUri = string.Format("{0}/outcome/version", ApiFeature.LeadingPath);
            return ApiFeature.ApiTestHost.Post<LearningOutcomeResource, VersionRequest>(name, requestUri, request);
        }

        public static HttpResponseMessage CreateEntityLearningOutcome(string name, IResource entityResource, OutcomeRequest request)
        {
            request.TenantId = ApiFeature.TenantId;
            var requestUri = string.Format("{0}/supports", entityResource.ResourceUri);
            return ApiFeature.ApiTestHost.Post<LearningOutcomeResource, OutcomeRequest>(name, requestUri, request);
        }

        public static HttpResponseMessage CreateCourseLearningActivity(string name, CourseSegmentResource segment, SaveCourseLearningActivityRequest request)
        {
            request.TenantId = ApiFeature.TenantId;
            var requestUri = string.Format("{0}/learningactivity", segment.ResourceUri);
            return ApiFeature.ApiTestHost.Post<CourseLearningActivityResource, SaveCourseLearningActivityRequest>(name, requestUri, request);
        }
    }
}