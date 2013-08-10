using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources;

namespace BpeProducts.Services.Course.Host.Tests.Integration.Operations
{
    public static class DeleteOperations
    {
        public static HttpResponseMessage CourseLearningActivity(CourseLearningActivityResource learningActivity)
        {
            var response = ApiFeature.ApiTestHost.Client.DeleteAsync(learningActivity.ResourceUri).Result;
            return response;
        }

        public static HttpResponseMessage LearningOutcome(LearningOutcomeResource learningOutcome)
        {
            var response = ApiFeature.ApiTestHost.Client.DeleteAsync(learningOutcome.ResourceUri).Result;
            return response;
        }
    }
}
