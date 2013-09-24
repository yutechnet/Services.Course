using BpeProducts.Services.Course.Host.Tests.Integration.Resources.Account;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace BpeProducts.Services.Course.Host.Tests.Integration.Operations.Account
{
    public static class PostOperations
    {
        public static HttpResponseMessage CreateOrganization(string name, SaveOrganizationRequest request)
        {
            request.TenantId = ApiFeature.TenantId;
            var requestUri = ApiFeature.AccountLeadingPath == "/" ? "/organization" : ApiFeature.AccountLeadingPath + "/organization";
            return ApiFeature.AccountApiTestHost.Post<OrganizationResource, SaveOrganizationRequest>(name, requestUri, request);
        }

        public static HttpResponseMessage CreateRole(string name, SaveRoleRequest request)
        {
            var requestUri = ApiFeature.AccountLeadingPath == "/" ? "/role" : ApiFeature.AccountLeadingPath + "/role";
            return ApiFeature.AccountApiTestHost.Post<RoleResource, SaveRoleRequest>(name, requestUri, request);
        }

        public static HttpResponseMessage GrantPermission(Guid userGuid, RoleResource role, OrganizationResource org)
        {
			var requestUri = string.Format("{0}/permission/organization/{1}/user/{2}", ApiFeature.AccountLeadingPath == "/" ? "" : ApiFeature.AccountLeadingPath, org.Id, userGuid);
            var request = new SavePermissionRequest { Roles = new List<Guid> { role.Id } };
            return ApiFeature.AccountApiTestHost.Post<RoleResource, SavePermissionRequest>(Guid.NewGuid().ToString(), requestUri, request);
        }

	    public static HttpResponseMessage GrantPermission(Guid userGuid, RoleResource role, Guid objId)
	    {
			var requestUri = string.Format("{0}/permission/object/{1}/user/{2}", ApiFeature.AccountLeadingPath == "/" ? "" : ApiFeature.AccountLeadingPath, objId, userGuid);
			var request = new SavePermissionRequest { Roles = new List<Guid> { role.Id } };
            return ApiFeature.AccountApiTestHost.Post<RoleResource, SavePermissionRequest>(Guid.NewGuid().ToString(), requestUri, request);
	    }
    }
}
