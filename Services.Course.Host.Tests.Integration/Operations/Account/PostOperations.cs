using System;
using System.Collections.Generic;
using System.Net.Http;
using BpeProducts.Common.WebApiTest.Framework;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources.Account;
using BpeProducts.Common.WebApiTest.Extensions;

namespace BpeProducts.Services.Course.Host.Tests.Integration.Operations.Account
{
    public static class PostOperations
    {
        public static HttpResponseMessage CreateOrganization(string name, SaveOrganizationRequest request)
        {
            request.TenantId = ApiFeature.TenantId;
			var response = ApiFeature.AccountApiTestHost.Client.PostAsJsonAsync(ApiFeature.AccountLeadingPath == "/" ? "/organization" : ApiFeature.AccountLeadingPath + "/organization", request).Result;

            response.BuildResource<OrganizationResource>(name);

            return response;
        }

        public static HttpResponseMessage CreateRole(string name, SaveRoleRequest request)
        {
			var response = ApiFeature.AccountApiTestHost.Client.PostAsJsonAsync(ApiFeature.AccountLeadingPath == "/" ? "/role" : ApiFeature.AccountLeadingPath + "/role", request).Result;
            response.BuildResource<RoleResource>(name);

            return response;
        }

        public static HttpResponseMessage GrantPermission(Guid userGuid, RoleResource role, OrganizationResource org)
        {
			var requestUri = string.Format("{0}/permission/organization/{1}/user/{2}", ApiFeature.AccountLeadingPath == "/" ? "" : ApiFeature.AccountLeadingPath, org.Id, userGuid);
            var savePermissionRequest = new SavePermissionRequest { Roles = new List<Guid> { role.Id } };

            var response = ApiFeature.AccountApiTestHost.Client.PostAsJsonAsync(requestUri, savePermissionRequest).Result;

            Responses.Add(response);
            
            return response;
        }

	    public static HttpResponseMessage GrantPermission(Guid userGuid, RoleResource role, Guid objId)
	    {
			//"permission/object/{objectId}/user/{userId}"
			var requestUri = string.Format("{0}/permission/object/{1}/user/{2}", ApiFeature.AccountLeadingPath == "/" ? "" : ApiFeature.AccountLeadingPath, objId, userGuid);
			var savePermissionRequest = new SavePermissionRequest { Roles = new List<Guid> { role.Id } };

			var response = ApiFeature.AccountApiTestHost.Client.PostAsJsonAsync(requestUri, savePermissionRequest).Result;

			Responses.Add(response);

			return response;
	    }
    }
}
