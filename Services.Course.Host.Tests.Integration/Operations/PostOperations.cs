using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources;

namespace BpeProducts.Services.Course.Host.Tests.Integration.Operations
{
    public static class PostOperations
    {
        public static CourseResource CreateCourse(SaveCourseRequest request)
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
                resource.ResourseUri = response.Headers.Location;
                resource.SaveRequest = request;
                resource.Dto = dto;
            }
            
            return resource;
        }

        public static CourseResource CreateCourseVersion(VersionRequest request)
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
                resource.ResourseUri = response.Headers.Location;
            }

            return resource;
        }

        public static ProgramResource CreateProgram(SaveProgramRequest request)
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

            return resource;
        }

        public static CourseSegmentResource CreateSegment(CourseResource course, SaveCourseSegmentRequest request)
        {
            return CreateSegment(course, null, request);
        }

        public static CourseSegmentResource CreateSegment(CourseResource course, CourseSegmentResource parentSegment, SaveCourseSegmentRequest request)
        {
            var uri = parentSegment == null
                          ? string.Format("{0}/course/{1}/segments", ApiFeature.LeadingPath, course.Id)
                          : string.Format("{0}/course/{1}/segments/{2}", ApiFeature.LeadingPath, course.Id, parentSegment.Id);

            var response = ApiFeature.ApiTestHost.Client.PostAsync(uri, request, new JsonMediaTypeFormatter()).Result;

            var resource = new CourseSegmentResource
            {
                Response = response
            };

            if (response.IsSuccessStatusCode)
            {
                var id = response.Headers.Location.Segments[response.Headers.Location.Segments.Length - 1];

                resource.Id = Guid.Parse(id);
                resource.ResourseUri = response.Headers.Location;
                resource.SaveRequest = request;
            }

            return resource;
        }

        public static LearningOutcomeResource CreateLearningOutcome(OutcomeRequest request)
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

            return resource;
        }

        public static LearningOutcomeResource CreateEntityLearningOutcome(string entityType, Guid entityId, OutcomeRequest request)
        {
            var uri = string.Format("{0}/{1}/{2}/supports", ApiFeature.LeadingPath, entityType, entityId);
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

            return resource;
        }

        public static CourseLearningActivityResource CreateCourseLearningActivity(CourseSegmentResource segment, SaveCourseLearningActivityRequest request)
        {
            var uri = string.Format("{0}/learningactivity", segment.ResourseUri);
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

            return resource;
        }
    }
}
