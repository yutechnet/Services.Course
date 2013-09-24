using BpeProducts.Services.Course.Host.Tests.Integration.Resources.Account;
using System.Net.Http;

namespace BpeProducts.Services.Course.Host.Tests.Integration.Operations.Account
{
    public class PutOperations
    {
        public static HttpResponseMessage UpdateRoleWithCapability(RoleResource role, UpdateRoleRequest request)
        {
            var requestUri = role.ResourceUri.ToString();
            return ApiFeature.AccountApiTestHost.Put(requestUri, request);
        }
    }
}
