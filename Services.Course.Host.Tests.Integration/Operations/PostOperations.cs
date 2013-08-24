using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources;
using BpeProducts.Services.Course.Host.Tests.Integration.StepSetups;

namespace BpeProducts.Services.Course.Host.Tests.Integration.Operations
{
    public static class PostOperations
    {
        public static HttpResponseMessage CreateCourse(string name, SaveCourseRequest request)
        {
            request.TenantId = ApiFeature.TenantId;
            var response = ApiFeature.ApiTestHost.Client.PostAsync(ApiFeature.LeadingPath + "/course", request, new JsonMediaTypeFormatter()).Result;

            BuildResource<CourseResource>(name, response);
            return response;
        }

        public static HttpResponseMessage CreateCourseVersion(string name, VersionRequest request)
        {
            var response = ApiFeature.ApiTestHost.Client.PostAsync(ApiFeature.LeadingPath + "/course/version", request, new JsonMediaTypeFormatter()).Result;

            BuildResource<CourseResource>(name, response);
            return response;
        }

        public static HttpResponseMessage CreateProgram(string name, SaveProgramRequest request)
        {
            request.TenantId = ApiFeature.TenantId;
            var response = ApiFeature.ApiTestHost.Client.PostAsync(ApiFeature.LeadingPath + "/program", request, new JsonMediaTypeFormatter()).Result;

            BuildResource<ProgramResource>(name, response);
            return response;
        }

        public static HttpResponseMessage CreateSegment(string name, CourseResource course, SaveCourseSegmentRequest request)
        {
            request.TenantId = ApiFeature.TenantId;

            var uri = string.Format("{0}/course/{1}/segments", ApiFeature.LeadingPath, course.Id);
            var response = ApiFeature.ApiTestHost.Client.PostAsJsonAsync(uri, request).Result;

            BuildResource<CourseSegmentResource>(name, response);
            return response;
        }

        public static HttpResponseMessage CreateLearningOutcome(string name, OutcomeRequest request)
        {
            request.TenantId = ApiFeature.TenantId;

            var response = ApiFeature.ApiTestHost.Client.PostAsync(ApiFeature.LeadingPath + "/outcome", request, new JsonMediaTypeFormatter()).Result;

            BuildResource<LearningOutcomeResource>(name, response);
            return response;
        }

        public static HttpResponseMessage CreateLearningOutcomeVersion(string name, LearningOutcomeResource learningOutcome, VersionRequest request)
        {
            var postUri = string.Format("{0}/outcome/version", ApiFeature.LeadingPath);
            var response = ApiFeature.ApiTestHost.Client.PostAsync(postUri, request, new JsonMediaTypeFormatter()).Result;

            BuildResource<LearningOutcomeResource>(name, response);
            return response;
        }

        public static HttpResponseMessage CreateEntityLearningOutcome(string name, IResource entityResource, OutcomeRequest request)
        {
            request.TenantId = ApiFeature.TenantId;

            var uri = string.Format("{0}/supports", entityResource.ResourceUri);
            var response = ApiFeature.ApiTestHost.Client.PostAsync(uri, request, new JsonMediaTypeFormatter()).Result;

            BuildResource<LearningOutcomeResource>(name, response);
            return response;
        }

        public static HttpResponseMessage CreateCourseLearningActivity(string name, CourseSegmentResource segment, SaveCourseLearningActivityRequest request)
        {
            request.TenantId = ApiFeature.TenantId;

            var uri = string.Format("{0}/learningactivity", segment.ResourceUri);
            var response = ApiFeature.ApiTestHost.Client.PostAsJsonAsync(uri, request).Result;

            BuildResource<CourseLearningActivityResource>(name, response);
            return response;
        }

        private static IResource BuildResource<T>(string name, HttpResponseMessage response) where T : IResource, new()
        {
            Whens.ResponseMessages.Add(response);

            if (response.IsSuccessStatusCode)
            {
                var id = response.Headers.Location.Segments[response.Headers.Location.Segments.Length - 1];

                var resource = new T
                {
                    Id = Guid.Parse(id),
                    ResourceUri = response.Headers.Location
                };

                Resources<T>.Add(name, resource);

                return resource;
            }

            return null;
        }
    }
}
