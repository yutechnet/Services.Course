using BpeProducts.Services.Course.Host.Tests.Integration.Resources.Account;
using BpeProducts.Services.Course.Host.Tests.Integration.StepSetups;
using System.Net.Http;

namespace BpeProducts.Services.Course.Host.Tests.Integration.Operations.Account
{
    public class PutOperations
    {
        public static HttpResponseMessage UpdateRoleWithCapability(RoleResource role, UpdateRoleRequest request)
        {
            var response = ApiFeature.AccountApiTestHost.Client.PutAsJsonAsync(role.ResourceUri.ToString(), request).Result;

            Whens.ResponseMessages.Add(response);
            return response;
        }
    }
}
