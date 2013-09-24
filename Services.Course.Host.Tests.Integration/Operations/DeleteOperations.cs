using BpeProducts.Common.WebApiTest.Framework;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources;
using System;
using System.Net.Http;

namespace BpeProducts.Services.Course.Host.Tests.Integration.Operations
{
    public static class DeleteOperations
    {
        public static HttpResponseMessage DeleteResource(IResource resource)
        {
            var requestUri = resource.ResourceUri.ToString();
            return ApiFeature.ApiTestHost.Delete(requestUri);
        }

        public static HttpResponseMessage ProgramDoesNotSupportLearningOutcome(ProgramResource program, LearningOutcomeResource outcome)
        {
            var requestUri = String.Format("{0}/program/{1}/supports/{2}", ApiFeature.LeadingPath, program.Id, outcome.Id);
            return ApiFeature.ApiTestHost.Delete(requestUri);
        }
        
        public static HttpResponseMessage CourseDoesNotSupportLearningOutcome(CourseResource course, LearningOutcomeResource outcome)
        {
            var requestUri = String.Format("{0}/course/{1}/supports/{2}", ApiFeature.LeadingPath, course.Id, outcome.Id);
            return ApiFeature.ApiTestHost.Delete(requestUri);
        }
        
        public static HttpResponseMessage SegmentDoesNotSupportLearningOutcome(CourseSegmentResource segment, LearningOutcomeResource outcome)
        {
            var requestUri = String.Format("{0}/segment/{1}/supports/{2}", ApiFeature.LeadingPath, segment.Id, outcome.Id);
            return ApiFeature.ApiTestHost.Delete(requestUri);
        }

        public static HttpResponseMessage OutcomeDoesNotSupportLearningOutcome(LearningOutcomeResource supportingOutcome, LearningOutcomeResource supportedOutcome)
        {
            var requestUri = String.Format("{0}/outcome/{1}/supports/{2}", ApiFeature.LeadingPath, supportingOutcome.Id, supportedOutcome.Id);
            return ApiFeature.ApiTestHost.Delete(requestUri);
        }
    }
}