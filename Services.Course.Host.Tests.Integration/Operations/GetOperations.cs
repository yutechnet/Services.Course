using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Host.Tests.Integration.StepSetups;

namespace BpeProducts.Services.Course.Host.Tests.Integration.Operations
{
    public static class GetOperations
    {
        public static Dictionary<Guid, List<OutcomeInfo>> GetEntityLearningOutcomes(IList<Guid> entityIds)
        {
            var queryString = entityIds.Aggregate("?entityIds=", (current, entityId) => current + (entityId + ",")).TrimEnd(',');

            var response = ApiFeature.ApiTestHost.Client.GetAsync(ApiFeature.LeadingPath + "/outcome/entityoutcomes" + queryString).Result;
            Whens.ResponseMessages.Add(response);

            return response.IsSuccessStatusCode
                       ? response.Content.ReadAsAsync<Dictionary<Guid, List<OutcomeInfo>>>().Result
                       : null;
        }

        public static CourseInfoResponse GetCourse(Uri resourceUri)
        {
            var response = ApiFeature.ApiTestHost.Client.GetAsync(resourceUri).Result;
            Whens.ResponseMessages.Add(response);

            return response.IsSuccessStatusCode
                       ? response.Content.ReadAsAsync<CourseInfoResponse>().Result
                       : null;
        }

        public static ProgramResponse GetProgram(Uri resourceUri)
        {
            var response = ApiFeature.ApiTestHost.Client.GetAsync(resourceUri).Result;
            Whens.ResponseMessages.Add(response);

            return response.IsSuccessStatusCode
                       ? response.Content.ReadAsAsync<ProgramResponse>().Result
                       : null;
        }

        public static CourseLearningActivityResponse GetCourseLearningActivity(Uri resourseUri)
        {
            var response = ApiFeature.ApiTestHost.Client.GetAsync(resourseUri).Result;
            Whens.ResponseMessages.Add(response);

            return response.IsSuccessStatusCode
                       ? response.Content.ReadAsAsync<CourseLearningActivityResponse>().Result
                       : null;
        }

	    public static CourseSegmentInfo GetSegment(Uri resourseUri)
        {
            var response = ApiFeature.ApiTestHost.Client.GetAsync(resourseUri).Result;
            Whens.ResponseMessages.Add(response);

            return response.IsSuccessStatusCode
                       ? response.Content.ReadAsAsync<CourseSegmentInfo>().Result
                       : null;
        }

        public static OutcomeInfo GetLearningOutcome(Uri resourceUri)
        {
            var response = ApiFeature.ApiTestHost.Client.GetAsync(resourceUri).Result;
            Whens.ResponseMessages.Add(response);

            return response.IsSuccessStatusCode
                       ? response.Content.ReadAsAsync<OutcomeInfo>().Result
                       : null;
        }

        public static List<OutcomeInfo> GetSupportedOutcomes(Uri resourceUri)
        {
            var uri = string.Format("{0}/supports", resourceUri);
            var response = ApiFeature.ApiTestHost.Client.GetAsync(uri).Result;
            Whens.ResponseMessages.Add(response);

            return response.IsSuccessStatusCode
                       ? response.Content.ReadAsAsync<List<OutcomeInfo>>().Result
                       : null;
        }

        public static List<Guid> GetCourseTemplateIds(Guid organizationId)
        {
            //TODO: Replace with we implement the service.
            return new List<Guid> { Guid.NewGuid() };
        }
    }
}
