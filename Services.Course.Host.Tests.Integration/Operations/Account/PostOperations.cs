using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources;
using BpeProducts.Services.Course.Host.Tests.Integration.StepSetups;

namespace BpeProducts.Services.Course.Host.Tests.Integration.Operations.Account
{
    public static class PostOperations
    {
        public static HttpResponseMessage CreateOrganization(string name, SaveOrganizationRequest request)
        {
            request.TenantId = ApiFeature.TenantId;
            var response = ApiFeature.AccountApiTestHost.Client.PutAsJsonAsync(ApiFeature.AccountLeadingPath + "/organization", request).Result;

            Whens.ResponseMessages.Add(response);

            if (response.IsSuccessStatusCode)
            {
                var id = response.Headers.Location.Segments[response.Headers.Location.Segments.Length - 1];

                var resource = new OrganizationResource
                {
                    Id = Guid.Parse(id),
                    ResourceUri = response.Headers.Location,
                };

                StepSetups.Account.Givens.Organizations.Add(name, resource);
            }

            return response;
        }

        public static HttpResponseMessage CreateRole(string name, SaveRoleRequest request)
        {
            request.TenantId = ApiFeature.TenantId;
            var response = ApiFeature.AccountApiTestHost.Client.PutAsJsonAsync(ApiFeature.AccountLeadingPath + "/role", request).Result;

            Whens.ResponseMessages.Add(response);

            if (response.IsSuccessStatusCode)
            {
                var id = response.Headers.Location.Segments[response.Headers.Location.Segments.Length - 1];

                var resource = new RoleResource
                {
                    Id = Guid.Parse(id),
                    ResourceUri = response.Headers.Location,
                };

                StepSetups.Account.Givens.Roles.Add(name, resource);
            }

            return response;
        }

        public static HttpResponseMessage GrantPermission(Guid userGuid, RoleResource role, OrganizationResource org)
        {
            var requestUri = string.Format("{0}/permission/organization/{1}/user/{2}", ApiFeature.AccountLeadingPath, org.Id, userGuid);
            var savePermissionRequest = new SavePermissionRequest { Roles = new List<Guid> { role.Id } };

            var response = ApiFeature.AccountApiTestHost.Client.PostAsJsonAsync(requestUri, savePermissionRequest).Result;

            Whens.ResponseMessages.Add(response);
            
            return response;
        }
    }
}
