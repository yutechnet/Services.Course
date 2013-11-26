using System;
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
            var requestUri = string.Format("{0}/course", ApiFeature.LeadingPath);
            return ApiFeature.CourseTestHost.Post<CourseResource, SaveCourseRequest>(name, requestUri, request);
        }

        public static HttpResponseMessage CreateCourseVersion(string name, VersionRequest request)
        {
            var requestUri = string.Format("{0}/course/version", ApiFeature.LeadingPath);
            return ApiFeature.CourseTestHost.Post<CourseResource, VersionRequest>(name, requestUri, request);
        }

        public static HttpResponseMessage CreateProgram(string name, SaveProgramRequest request)
        {
            request.TenantId = ApiFeature.TenantId;
            var requestUri = string.Format("{0}/program", ApiFeature.LeadingPath);
            return ApiFeature.CourseTestHost.Post<ProgramResource, SaveProgramRequest>(name, requestUri, request);
        }

        public static HttpResponseMessage CreateSegment(string name, CourseResource course, SaveCourseSegmentRequest request)
        {
            request.TenantId = ApiFeature.TenantId;
            var requestUri = string.Format("{0}/course/{1}/segments", ApiFeature.LeadingPath, course.Id);
            return ApiFeature.CourseTestHost.Post<CourseSegmentResource, SaveCourseSegmentRequest>(name, requestUri, request);
        }

        public static HttpResponseMessage CreateLearningOutcome(string name, OutcomeRequest request)
        {
            request.TenantId = ApiFeature.TenantId;
            var requestUri = string.Format("{0}/outcome", ApiFeature.LeadingPath);
            return ApiFeature.CourseTestHost.Post<LearningOutcomeResource, OutcomeRequest>(name, requestUri, request);
        }

        public static HttpResponseMessage CreateLearningOutcomeVersion(string name, LearningOutcomeResource learningOutcome, VersionRequest request)
        {
            var requestUri = string.Format("{0}/outcome/version", ApiFeature.LeadingPath);
            return ApiFeature.CourseTestHost.Post<LearningOutcomeResource, VersionRequest>(name, requestUri, request);
        }

        public static HttpResponseMessage CreateEntityLearningOutcome(string name, IResource entityResource, OutcomeRequest request)
        {
            request.TenantId = ApiFeature.TenantId;
            var requestUri = string.Format("{0}/supports", entityResource.ResourceUri);
            return ApiFeature.CourseTestHost.Post<LearningOutcomeResource, OutcomeRequest>(name, requestUri, request);
        }

        public static HttpResponseMessage CreateCourseLearningActivity(string name, CourseSegmentResource segment, SaveCourseLearningActivityRequest request)
        {
            request.TenantId = ApiFeature.TenantId;
            var requestUri = string.Format("{0}/learningactivity", segment.ResourceUri);
            return ApiFeature.CourseTestHost.Post<CourseLearningActivityResource, SaveCourseLearningActivityRequest>(name, requestUri, request);
        }

        public static HttpResponseMessage CreateSection(string name, CourseResource course, CourseSectionRequest request)
        {           
            var requestUri = string.Format("{0}/section", course.ResourceUri);
            return ApiFeature.CourseTestHost.Post<SectionResource, CourseSectionRequest>(name, requestUri, request);
        }

        public static HttpResponseMessage AddLearningMaterial(string name, CourseLearningActivityResource learningActivity, LearningMaterialRequest request)
        {
            var requestUri = string.Format("{0}/learningmaterial", learningActivity.ResourceUri);
            return ApiFeature.CourseTestHost.Post<LearningMaterialResource, LearningMaterialRequest>(name, requestUri, request);
        }

        public static HttpResponseMessage AssociateRubric(string name, CourseLearningActivityResource resource, CourseRubricRequest request)
        {
            var requestUri = string.Format("{0}/courserubric", resource.ResourceUri);
            return ApiFeature.CourseTestHost.Post<CourseLearningActivityResource, CourseRubricRequest>(name, requestUri, request);
        }
    }
}
