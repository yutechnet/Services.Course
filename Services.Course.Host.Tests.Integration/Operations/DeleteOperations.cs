using BpeProducts.Common.WebApiTest.Framework;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources;
using System.Net.Http;

namespace BpeProducts.Services.Course.Host.Tests.Integration.Operations
{
    public static class DeleteOperations
    {
        public static HttpResponseMessage DeleteResource(IResource resource)
        {
            var response = ApiFeature.ApiTestHost.Client.DeleteAsync(resource.ResourceUri).Result;
            Responses.Add(response);
            
            return response;
        }

        public static HttpResponseMessage ProgramDoesNotSupportLearningOutcome(ProgramResource program, LearningOutcomeResource outcome)
        {
            var requestUri = string.Format("{0}/program/{1}/supports/{2}", ApiFeature.LeadingPath, program.Id, outcome.Id);
            var response = ApiFeature.ApiTestHost.Client.DeleteAsync(requestUri).Result;

            Responses.Add(response);
            return response;
        }
        
        public static HttpResponseMessage CourseDoesNotSupportLearningOutcome(CourseResource course, LearningOutcomeResource outcome)
        {
            var requestUri = string.Format("{0}/course/{1}/supports/{2}", ApiFeature.LeadingPath, course.Id, outcome.Id);
            var response = ApiFeature.ApiTestHost.Client.DeleteAsync(requestUri).Result;

            Responses.Add(response);
            return response;
        }
        
        public static HttpResponseMessage SegmentDoesNotSupportLearningOutcome(CourseSegmentResource segment, LearningOutcomeResource outcome)
        {
            var requestUri = string.Format("{0}/segment/{1}/supports/{2}", ApiFeature.LeadingPath, segment.Id, outcome.Id);
            var response = ApiFeature.ApiTestHost.Client.DeleteAsync(requestUri).Result;

            Responses.Add(response);
            return response;
        }
    }
}
