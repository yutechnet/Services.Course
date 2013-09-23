using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using BpeProducts.Common.WebApiTest.Framework;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources;
using BpeProducts.Common.WebApiTest.Extensions;

namespace BpeProducts.Services.Course.Host.Tests.Integration.Operations
{
    public static class PostOperations
    {
        public static HttpResponseMessage CreateCourse(string name, SaveCourseRequest request)
        {
            request.TenantId = ApiFeature.TenantId;
            var response = ApiFeature.ApiTestHost.Client.PostAsync(ApiFeature.LeadingPath + "/course", request, new JsonMediaTypeFormatter()).Result;

            response.BuildResource<CourseResource>(name);
            return response;
        }

        public static HttpResponseMessage CreateCourseVersion(string name, VersionRequest request)
        {
            var response = ApiFeature.ApiTestHost.Client.PostAsync(ApiFeature.LeadingPath + "/course/version", request, new JsonMediaTypeFormatter()).Result;

            response.BuildResource<CourseResource>(name);
            return response;
        }

        public static HttpResponseMessage CreateProgram(string name, SaveProgramRequest request)
        {
            request.TenantId = ApiFeature.TenantId;
            var response = ApiFeature.ApiTestHost.Client.PostAsync(ApiFeature.LeadingPath + "/program", request, new JsonMediaTypeFormatter()).Result;

            response.BuildResource<ProgramResource>(name);
            return response;
        }

        public static HttpResponseMessage CreateSegment(string name, CourseResource course, SaveCourseSegmentRequest request)
        {
            request.TenantId = ApiFeature.TenantId;

            var uri = string.Format("{0}/course/{1}/segments", ApiFeature.LeadingPath, course.Id);
            var response = ApiFeature.ApiTestHost.Client.PostAsJsonAsync(uri, request).Result;

            response.BuildResource<CourseSegmentResource>(name);
            return response;
        }

        public static HttpResponseMessage CreateLearningOutcome(string name, OutcomeRequest request)
        {
            request.TenantId = ApiFeature.TenantId;

            var response = ApiFeature.ApiTestHost.Client.PostAsync(ApiFeature.LeadingPath + "/outcome", request, new JsonMediaTypeFormatter()).Result;

            response.BuildResource<LearningOutcomeResource>(name);
            return response;
        }

        public static HttpResponseMessage CreateLearningOutcomeVersion(string name, LearningOutcomeResource learningOutcome, VersionRequest request)
        {
            var postUri = string.Format("{0}/outcome/version", ApiFeature.LeadingPath);
            var response = ApiFeature.ApiTestHost.Client.PostAsync(postUri, request, new JsonMediaTypeFormatter()).Result;

            response.BuildResource<LearningOutcomeResource>(name);
            return response;
        }

        public static HttpResponseMessage CreateEntityLearningOutcome(string name, IResource entityResource, OutcomeRequest request)
        {
            request.TenantId = ApiFeature.TenantId;

            var uri = string.Format("{0}/supports", entityResource.ResourceUri);
            var response = ApiFeature.ApiTestHost.Client.PostAsync(uri, request, new JsonMediaTypeFormatter()).Result;

            response.BuildResource<LearningOutcomeResource>(name);
            return response;
        }

        public static HttpResponseMessage CreateCourseLearningActivity(string name, CourseSegmentResource segment, SaveCourseLearningActivityRequest request)
        {
            request.TenantId = ApiFeature.TenantId;

            var uri = string.Format("{0}/learningactivity", segment.ResourceUri);
            var response = ApiFeature.ApiTestHost.Client.PostAsJsonAsync(uri, request).Result;

            response.BuildResource<CourseLearningActivityResource>(name);
            return response;
        }
    }
}
