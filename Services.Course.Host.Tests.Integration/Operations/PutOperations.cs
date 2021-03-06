﻿using BpeProducts.Common.Contract;
using BpeProducts.Common.WebApiTest.Framework;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace BpeProducts.Services.Course.Host.Tests.Integration.Operations
{
    public static class PutOperations
    {
        public static HttpResponseMessage ProgramSupportsLearningOutcome(ProgramResource program, LearningOutcomeResource outcome)
        {
            var requestUri = string.Format("{0}/program/{1}/supports/{2}", ApiFeature.LeadingPath, program.Id, outcome.Id);
            var request = new {};
            return ApiFeature.CourseTestHost.Put(requestUri, request);
        }

        public static HttpResponseMessage CourseSupportsLearningOutcome(CourseResource course, LearningOutcomeResource outcome)
        {
            var requestUri = string.Format("{0}/course/{1}/supports/{2}", ApiFeature.LeadingPath, course.Id, outcome.Id);
            var request = new {};
            return ApiFeature.CourseTestHost.Put(requestUri, request);
        }

        public static HttpResponseMessage SegmentSupportsLearningOutcome(CourseSegmentResource segment, LearningOutcomeResource outcome)
        {
            var requestUri = string.Format("{0}/segment/{1}/supports/{2}", ApiFeature.LeadingPath, segment.Id, outcome.Id);
            var request = new {};
            return ApiFeature.CourseTestHost.Put(requestUri, request);
        }

        public static HttpResponseMessage OutcomeSupportsLearningOutcome(LearningOutcomeResource supportingOutcome, LearningOutcomeResource supportedOutcome, bool isAsync = false)
        {
            var requestUri = string.Format("{0}/outcome/{1}/supports/{2}", ApiFeature.LeadingPath, supportingOutcome.Id, supportedOutcome.Id);

            HttpResponseMessage response = null;
            var request = new {};
            if (isAsync)
            {
                ApiFeature.CourseTestHost.Client.PutAsJsonAsync(requestUri, request);
            }
            else
            {
                response = ApiFeature.CourseTestHost.Put(requestUri, request);
            }

            return response;
        }

        public static HttpResponseMessage AssociateCourseWithPrograms(CourseResource course, IList<ProgramResource> programs)
        {
            var courseInfo = GetOperations.GetCourse(course);

            var request = new SaveCourseRequest
            {
                Code = courseInfo.Code,
                Description = courseInfo.Description,
                Name = courseInfo.Name,
                ProgramIds = courseInfo.ProgramIds,
                TenantId = ApiFeature.TenantId,
                OrganizationId = courseInfo.OrganizationId,
                IsTemplate = courseInfo.IsTemplate,
                MetaData=courseInfo.MetaData,
                PrerequisiteCourseIds = courseInfo.PrerequisiteCourseIds
            };

            var programIds = (from p in programs select p.Id).ToList();
            request.ProgramIds.AddRange(programIds);

            var requestUri = course.ResourceUri.ToString();
            return ApiFeature.CourseTestHost.Put(requestUri, request);
        }

        public static HttpResponseMessage DisassociateCourseWithPrograms(CourseResource course, List<ProgramResource> programs)
        {
            var courseInfo = GetOperations.GetCourse(course);

            var request = new SaveCourseRequest
            {
                Code = courseInfo.Code,
                Description = courseInfo.Description,
                Name = courseInfo.Name,
                ProgramIds = courseInfo.ProgramIds,
                TenantId = ApiFeature.TenantId,
                OrganizationId = courseInfo.OrganizationId,
                IsTemplate = courseInfo.IsTemplate,
                PrerequisiteCourseIds = courseInfo.PrerequisiteCourseIds
            };

            var idsToRemove = (from p in programs select p.Id).ToList();
            request.ProgramIds.RemoveAll(idsToRemove.Contains);

            var requestUri = course.ResourceUri.ToString();
            return ApiFeature.CourseTestHost.Put(requestUri, request);
        }
     

        public static HttpResponseMessage Publish(IResource resource, PublishRequest request)
        {
            var requestUri = string.Format("{0}/publish", resource.ResourceUri);
            return ApiFeature.CourseTestHost.Put(requestUri, request);
        }

  
        public static HttpResponseMessage SetCoursePrerequisites(CourseResource course, UpdateCoursePrerequisites request)
        {
            var requestUri = string.Format("{0}/prerequisite", course.ResourceUri);
            return ApiFeature.CourseTestHost.Put(requestUri, request);
        }

        public static HttpResponseMessage UpdateCourseLearningActivity(CourseLearningActivityResource courseLearningActivity, SaveCourseLearningActivityRequest request)
        {
            var requestUri = courseLearningActivity.ResourceUri.ToString();
            return ApiFeature.CourseTestHost.Put(requestUri, request);
        }

        public static HttpResponseMessage UpdateCourse(CourseResource course, UpdateCourseRequest request)
        {
            var requestUri = course.ResourceUri.ToString();
            return ApiFeature.CourseTestHost.Put(requestUri, request);
        }

        public static HttpResponseMessage UpdateCourseSegmentRequest(CourseSegmentResource courseSegment, SaveCourseSegmentRequest request)
        {
            var requestUri = courseSegment.ResourceUri.ToString();
            return ApiFeature.CourseTestHost.Put(requestUri, request);
        }

        public static HttpResponseMessage UpdateResource(IResource resource, object request)
        {
            var requestUri = resource.ResourceUri.ToString();
            return ApiFeature.CourseTestHost.Put(requestUri, request);
        }

        public static HttpResponseMessage UpdateBulkCourseSegments(CourseResource course, IList<UpdateCourseSegmentRequest> request)
        {
            var requestUri = string.Format("{0}/segment", course.ResourceUri);
            return ApiFeature.CourseTestHost.Put(requestUri, request);
        }

        public static HttpResponseMessage UpdateLearningMaterial(LearningMaterialResource learningMaterial, UpdateLearningMaterialRequest request)
        {
            var requestUri = learningMaterial.ResourceUri.ToString();
            return ApiFeature.CourseTestHost.Put(requestUri, request);
        }

        public static HttpResponseMessage UpdateActivationStatus(IResource resource, ActivationRequest activationRequest)
        {
            var requestUri = string.Format("{0}/activate", resource.ResourceUri);
            return ApiFeature.CourseTestHost.Put(requestUri, activationRequest);
        }
    }
}
