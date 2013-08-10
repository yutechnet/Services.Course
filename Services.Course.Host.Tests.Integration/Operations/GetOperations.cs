using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using BpeProducts.Services.Course.Contract;

namespace BpeProducts.Services.Course.Host.Tests.Integration.Operations
{
    public static class GetOperations
    {
        public static Dictionary<Guid, List<OutcomeInfo>> GetEntityLearningOutcomes(IList<Guid> entityIds)
        {
            var queryString = entityIds.Aggregate("?entityIds=", (current, entityId) => current + (entityId + ",")).TrimEnd(',');

            var response = ApiFeature.ApiTestHost.Client.GetAsync(ApiFeature.LeadingPath + "/outcome/entityoutcomes" + queryString).Result;
            response.EnsureSuccessStatusCode();

            var outcomes = response.Content.ReadAsAsync<Dictionary<Guid, List<OutcomeInfo>>>().Result;
            return outcomes;
        }

        public static CourseInfoResponse GetCourse(Uri resourceUri)
        {
            var response = ApiFeature.ApiTestHost.Client.GetAsync(resourceUri).Result;
            response.EnsureSuccessStatusCode();

            var result = response.Content.ReadAsAsync<CourseInfoResponse>().Result;
            return result;
        }

        public static ProgramResponse GetProgram(Uri resourceUri)
        {
            var response = ApiFeature.ApiTestHost.Client.GetAsync(resourceUri).Result;
            response.EnsureSuccessStatusCode();

            var result = response.Content.ReadAsAsync<ProgramResponse>().Result;
            return result;
        }

        public static CourseLearningActivityResponse GetCourseLearningActivity(Uri resourseUri)
        {
            var response = ApiFeature.ApiTestHost.Client.GetAsync(resourseUri).Result;
            response.EnsureSuccessStatusCode();

            var result = response.Content.ReadAsAsync<CourseLearningActivityResponse>().Result;
            return result;
        }

        public static OutcomeInfo GetLearningOutcome(Uri resourceUri)
        {
            var response = ApiFeature.ApiTestHost.Client.GetAsync(resourceUri).Result;
            response.EnsureSuccessStatusCode();

            var result = response.Content.ReadAsAsync<OutcomeInfo>().Result;
            return result;
        }

        public static List<OutcomeInfo> GetSupportedOutcomes(Uri resourceUri)
        {
            var uri = string.Format("{0}/supports", resourceUri);
            var response = ApiFeature.ApiTestHost.Client.GetAsync(uri).Result;
            response.EnsureSuccessStatusCode();

            var result = response.Content.ReadAsAsync<List<OutcomeInfo>>().Result;
            return result;
        }
    }
}
