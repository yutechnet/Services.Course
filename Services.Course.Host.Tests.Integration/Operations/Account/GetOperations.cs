using System;

namespace BpeProducts.Services.Course.Host.Tests.Integration.Operations.Account
{
    public static class GetOperations
    {
        public static RoleResponse GetRole(Uri resourceUri)
        {
            return ApiFeature.AccountApiTestHost.Get<RoleResponse>(resourceUri.ToString());
        }
    }
}
