using BpeProducts.Services.Course.Host.Tests.Integration.StepSetups;
using System;
using System.Net.Http;

namespace BpeProducts.Services.Course.Host.Tests.Integration.Operations.Account
{
    public static class GetOperations
    {
        public static RoleResponse GetRole(Uri resourceUri)
        {
            var response = ApiFeature.AccountApiTestHost.Client.GetAsync(resourceUri).Result;
            Whens.ResponseMessages.Add(response);

            return response.IsSuccessStatusCode
                       ? response.Content.ReadAsAsync<RoleResponse>().Result
                       : null;
        }
    }
}
