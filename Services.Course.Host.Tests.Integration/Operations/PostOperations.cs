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
        public static CourseResource CreateCourse(string name, SaveCourseRequest request)
        {
            var response = ApiFeature.ApiTestHost.Client.PostAsync(ApiFeature.LeadingPath + "/course", request, new JsonMediaTypeFormatter()).Result;

            var resource = new CourseResource
            {
                Response = response
            };

            if (response.IsSuccessStatusCode)
            {
                var dto = response.Content.ReadAsAsync<CourseInfoResponse>().Result;

                resource.Id = dto.Id;
                resource.ResourceUri = response.Headers.Location;
                resource.SaveRequest = request;
                resource.Dto = dto;
            }
            
            Givens.Courses.Add(name, resource);
            return resource;
        }

        public static CourseResource CreateCourseVersion(string name, VersionRequest request)
        {
            var response = ApiFeature.ApiTestHost.Client.PostAsync(ApiFeature.LeadingPath + "/course/version", request, new JsonMediaTypeFormatter()).Result;

            var resource = new CourseResource
            {
                Response = response
            };

            if (response.IsSuccessStatusCode)
            {
                var id = response.Headers.Location.Segments[response.Headers.Location.Segments.Length - 1];
                resource.Id = Guid.Parse(id);
                resource.ResourceUri = response.Headers.Location;
            }

            Givens.Courses.Add(name, resource);
            return resource;
        }

        public static ProgramResource CreateProgram(string name, SaveProgramRequest request)
        {
            var response = ApiFeature.ApiTestHost.Client.PostAsync(ApiFeature.LeadingPath + "/program", request, new JsonMediaTypeFormatter()).Result;
            response.EnsureSuccessStatusCode();

            var resource = new ProgramResource
            {
                Response = response
            };

            if (response.IsSuccessStatusCode)
            {
                var dto = response.Content.ReadAsAsync<ProgramResponse>().Result;

                resource.Id = dto.Id;
                resource.ResourceUri = response.Headers.Location;
                resource.SaveRequest = request;
                resource.Dto = dto;
            }

            Givens.Programs.Add(name, resource);
            return resource;
        }

        public static CourseSegmentResource CreateSegment(string name, CourseResource course, SaveCourseSegmentRequest request)
        {
            var uri = string.Format("{0}/course/{1}/segments", ApiFeature.LeadingPath, course.Id);

            var response = ApiFeature.ApiTestHost.Client.PostAsJsonAsync(uri, request).Result;

            var resource = new CourseSegmentResource
            {
                Response = response
            };

            if (response.IsSuccessStatusCode)
            {
                var id = response.Headers.Location.Segments[response.Headers.Location.Segments.Length - 1];

                resource.Id = Guid.Parse(id);
                resource.ResourceUri = response.Headers.Location;
                resource.SaveRequest = request;
            }

            Givens.Segments.Add(name, resource);
            return resource;
        }

        public static LearningOutcomeResource CreateLearningOutcome(string name, OutcomeRequest request)
        {
            var response = ApiFeature.ApiTestHost.Client.PostAsync(ApiFeature.LeadingPath + "/outcome", request, new JsonMediaTypeFormatter()).Result;

            var resource = new LearningOutcomeResource
            {
                Response = response
            };

            if (response.IsSuccessStatusCode)
            {
                var id = response.Headers.Location.Segments[response.Headers.Location.Segments.Length - 1];

                resource.Id = Guid.Parse(id);
                resource.ResourceUri = response.Headers.Location;
                resource.SaveRequest = request;
            }

            Givens.LearningOutcomes.Add(name, resource);
            return resource;
        }

        public static LearningOutcomeResource CreateEntityLearningOutcome(string name, IResource entityResource, OutcomeRequest request)
        {
            var uri = string.Format("{0}/supports", entityResource.ResourceUri);
            var response = ApiFeature.ApiTestHost.Client.PostAsync(uri, request, new JsonMediaTypeFormatter()).Result;

            var resource = new LearningOutcomeResource
            {
                Response = response
            };

            if (response.IsSuccessStatusCode)
            {
                var id = response.Headers.Location.Segments[response.Headers.Location.Segments.Length - 1];

                resource.Id = Guid.Parse(id);
                resource.ResourceUri = response.Headers.Location;
                resource.SaveRequest = request;
            }

            Givens.LearningOutcomes.Add(name, resource);
            return resource;
        }

        public static CourseLearningActivityResource CreateCourseLearningActivity(string name, CourseSegmentResource segment, SaveCourseLearningActivityRequest request)
        {
            var uri = string.Format("{0}/learningactivity", segment.ResourceUri);
            var response = ApiFeature.ApiTestHost.Client.PostAsJsonAsync(uri, request).Result;

            var resource = new CourseLearningActivityResource
            {
                Response = response
            };

            if (response.IsSuccessStatusCode)
            {
                var id = response.Headers.Location.Segments[response.Headers.Location.Segments.Length - 1];
                var dto = response.Content.ReadAsAsync<CourseLearningActivityResponse>().Result;

                resource.Id = Guid.Parse(id);
                resource.ResourceUri = response.Headers.Location;
                resource.SaveRequest = request;
                resource.Dto = dto;
            }

            Givens.CourseLearningActivities.Add(name, resource);
            return resource;
        }
    }
}
