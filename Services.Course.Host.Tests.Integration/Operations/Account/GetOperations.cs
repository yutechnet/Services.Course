using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Host.Tests.Integration.StepSetups;

namespace BpeProducts.Services.Course.Host.Tests.Integration.Operations.Account
{
    public static class GetOperations
    {
        public static RoleResponse GetRole(Uri resourceUri)
        {
            var response = ApiFeature.ApiTestHost.Client.GetAsync(resourceUri).Result;
            Whens.ResponseMessages.Add(response);

            return response.IsSuccessStatusCode
                       ? response.Content.ReadAsAsync<RoleResponse>().Result
                       : null;
        }
    }
}
