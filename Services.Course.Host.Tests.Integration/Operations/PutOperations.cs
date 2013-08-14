using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources;

namespace BpeProducts.Services.Course.Host.Tests.Integration.Operations
{
    public static class PutOperations
    {
        public static HttpResponseMessage ProgramSupportsLearningOutcome(ProgramResource program, LearningOutcomeResource outcome)
        {
            var uri = string.Format("{0}/program/{1}/supports/{2}", ApiFeature.LeadingPath, program.Id, outcome.Id);
            var response = ApiFeature.ApiTestHost.Client.PutAsJsonAsync(uri, new { }).Result;

            return response;
        }

        public static HttpResponseMessage ProgramDoesNotSupportLearningOutcome(ProgramResource program, LearningOutcomeResource outcome)
        {
            var uri = string.Format("{0}/program/{1}/supports/{2}", ApiFeature.LeadingPath, program.Id, outcome.Id);
            var response = ApiFeature.ApiTestHost.Client.DeleteAsync(uri).Result;

            return response;
        }

        public static HttpResponseMessage CourseSupportsLearningOutcome(CourseResource course, LearningOutcomeResource outcome)
        {
            var uri = string.Format("{0}/course/{1}/supports/{2}", ApiFeature.LeadingPath, course.Id, outcome.Id);
            var response = ApiFeature.ApiTestHost.Client.PutAsJsonAsync(uri, new { }).Result;

            return response;
        }

        public static HttpResponseMessage CourseDoesNotSupportLearningOutcome(CourseResource course, LearningOutcomeResource outcome)
        {
            var uri = string.Format("{0}/course/{1}/supports/{2}", ApiFeature.LeadingPath, course.Id, outcome.Id);
            var response = ApiFeature.ApiTestHost.Client.DeleteAsync(uri).Result;

            return response;
        }

        public static HttpResponseMessage SegmentSupportsLearningOutcome(CourseSegmentResource segment, LearningOutcomeResource outcome)
        {
            var uri = string.Format("{0}/segment/{1}/supports/{2}", ApiFeature.LeadingPath, segment.Id, outcome.Id);
            var response = ApiFeature.ApiTestHost.Client.PutAsJsonAsync(uri, new { }).Result;

            return response;
        }

        public static HttpResponseMessage SegmentDoesNotSupportLearningOutcome(CourseSegmentResource segment, LearningOutcomeResource outcome)
        {
            var uri = string.Format("{0}/segment/{1}/supports/{2}", ApiFeature.LeadingPath, segment.Id, outcome.Id);
            var response = ApiFeature.ApiTestHost.Client.DeleteAsync(uri).Result;

            return response;
        }

        public static HttpResponseMessage OutcomeSupportsLearningOutcome(LearningOutcomeResource supportingOutcome, LearningOutcomeResource supportedOutcome)
        {
            var uri = string.Format("{0}/outcome/{1}/supports/{2}", ApiFeature.LeadingPath, supportingOutcome.Id, supportedOutcome.Id);
            var response = ApiFeature.ApiTestHost.Client.PutAsJsonAsync(uri, new { }).Result;

            return response;
        }

        public static HttpResponseMessage OutcomeDoesNotSupportLearningOutcome(LearningOutcomeResource supportingOutcome, LearningOutcomeResource supportedOutcome)
        {
            var uri = string.Format("{0}/outcome/{1}/supports/{2}", ApiFeature.LeadingPath, supportingOutcome.Id, supportedOutcome.Id);
            var response = ApiFeature.ApiTestHost.Client.DeleteAsync(uri).Result;

            return response;
        }

        public static HttpResponseMessage AssociateCourseWithPrograms(CourseResource course, IList<ProgramResource> programs)
        {
            var saveCourseRequest = new SaveCourseRequest
            {
                Code = course.Dto.Code,
                Description = course.Dto.Description,
                Name = course.Dto.Name,
                ProgramIds = course.Dto.ProgramIds,
                TenantId = ApiFeature.TenantId,
                OrganizationId = course.Dto.OrganizationId,
                IsTemplate = course.Dto.IsTemplate,
                PrerequisiteCourseIds = course.Dto.PrerequisiteCourseIds
            };

            var programIds = (from p in programs select p.Id).ToList();
            saveCourseRequest.ProgramIds.AddRange(programIds);

            var response = ApiFeature.ApiTestHost.Client.PutAsJsonAsync(course.ResourceUri.ToString(), saveCourseRequest).Result;

            return response;
        }

        public static HttpResponseMessage DisassociateCourseWithPrograms(CourseResource course, List<ProgramResource> programs)
        {
            var saveCourseRequest = new SaveCourseRequest
            {
                Code = course.Dto.Code,
                Description = course.Dto.Description,
                Name = course.Dto.Name,
                ProgramIds = course.Dto.ProgramIds,
                TenantId = ApiFeature.TenantId,
                OrganizationId = course.Dto.OrganizationId,
                IsTemplate = course.Dto.IsTemplate,
                PrerequisiteCourseIds = course.Dto.PrerequisiteCourseIds
            };

            var idsToRemove = (from p in programs select p.Id).ToList();
            saveCourseRequest.ProgramIds.RemoveAll(idsToRemove.Contains);

            var response = ApiFeature.ApiTestHost.Client.PutAsJsonAsync(course.ResourceUri.ToString(), saveCourseRequest).Result;

            return response;
        }

        public static HttpResponseMessage PublishCourse(CourseResource course, PublishRequest request)
        {
            var uri = string.Format("{0}/publish", course.ResourceUri);

            var response = ApiFeature.ApiTestHost.Client.PutAsJsonAsync(uri, request).Result;

            return response;
        }

        public static HttpResponseMessage SetCoursePrerequisites(CourseResource course, UpdateCoursePrerequisites request)
        {
            var uri = string.Format("{0}/prerequisites", course.ResourceUri);

            var response = ApiFeature.ApiTestHost.Client.PutAsJsonAsync(uri, request).Result;

            return response;
        }

        public static HttpResponseMessage UpdateCourseLearningActivity(CourseLearningActivityResource courseLearningActivity, SaveCourseLearningActivityRequest request)
        {
            var response = ApiFeature.ApiTestHost.Client.PutAsJsonAsync(courseLearningActivity.ResourceUri.ToString(), request).Result;

            return response;
        }

        public static HttpResponseMessage UpdateLearningOutcome(LearningOutcomeResource learningOutcome, OutcomeRequest request)
        {
            var response = ApiFeature.ApiTestHost.Client.PutAsJsonAsync(learningOutcome.ResourceUri.ToString(), request).Result;

            return response;
        }

        public static HttpResponseMessage UpdateCourse(CourseResource course, UpdateCourseRequest updateCourseRequest)
        {
            var response = ApiFeature.ApiTestHost.Client.PutAsJsonAsync(course.ResourceUri.ToString(), updateCourseRequest).Result;

            return response;
        }
    }
}
