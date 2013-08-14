using System;
using System.Collections.Generic;
using System.Net.Http;
using BpeProducts.Common.Capabilities;
using BpeProducts.Common.WebApiTest;
using TechTalk.SpecFlow;

namespace BpeProducts.Services.Course.Host.Tests.Integration
{
    //TODO: Move methods to Given/When/Then/AccountGive/AccountWhen/etc.
    [Binding]
    public class CourseCreateCapabilitySteps
    {
        private const int TenantId = 999999;
        private const string OrganizationUrl = "account/organization";
        private const string RoleUrl = "account/role";
        private const string PermissionUrl = "account/permission";

        public static WebApiTestHost RemoteApiTestHost
        {
            get { return (WebApiTestHost) FeatureContext.Current["RemoteApiTestHost"]; }
        }

        [Given(@"I create an organization ""(.*)"" with no parent")]
        public void GivenICreateAnOrganizationWithNoParent(string organizationName)
        {
            GivenICreateAnOrganizationWithParentOrganization(organizationName);
        }

        [Given(@"I create an organization ""(.*)"" with parent organization ""(.*)""")]
        public void GivenICreateAnOrganizationWithParentOrganization(string organizationName, string parentOrganizationName = "")
        {
            var saveOrganizationRequest = new SaveOrganizationRequest { Name = organizationName, TenantId = TenantId };
            if (parentOrganizationName != "")
            {
                var parent = (Guid) ScenarioContext.Current[parentOrganizationName];
                saveOrganizationRequest.Parent = parent;
            }

            var response = RemoteApiTestHost.Client.PostAsJsonAsync(OrganizationUrl, saveOrganizationRequest).Result;

            if (!response.IsSuccessStatusCode)
            {
                var responseBody = response.Content.ReadAsStringAsync().Result;
                throw new Exception(responseBody);
            }

            var result = response.Content.ReadAsAsync<OrganizationResponse>().Result;
            var organizationId = result.Id;
            ScenarioContext.Current[organizationName] = organizationId;
        }

        [Given(@"I create a role ""(.*)""")]
        public void GivenICreateARole(string roleName)
        {
            var request = new SaveRoleRequest()
            {
                Name = roleName,
                TenantId = TenantId,
                Capabilities = new List<Capability>(),
                OrganizationId = null
            };

            var response = RemoteApiTestHost.Client.PostAsJsonAsync(RoleUrl, request).Result;

            if (!response.IsSuccessStatusCode)
            {
                var responseBody = response.Content.ReadAsStringAsync().Result;
                throw new Exception(responseBody);
            }

            var result = response.Content.ReadAsAsync<RoleResponse>().Result;
            var roleId = result.Id;
            ScenarioContext.Current[roleName] = roleId;
        }

        [Given(@"I give capability (.*) to role ""(.*)""")]
        public void GivenIGiveCapabilityToRole(string capability, string roleName)
        {
            if (capability == "")
            {
                return;
            }
            var capabilityEnum = (Capability)Enum.Parse(typeof(Capability), capability);
            var roleId = (Guid)ScenarioContext.Current[roleName];
            var roleResponse = GetRole(roleId);
            UpdateRoleWithCapability(roleResponse, capabilityEnum);
        }

        private RoleResponse GetRole(Guid roleId)
        {
            var response = RemoteApiTestHost.Client.GetAsync(string.Format("{0}/{1}", RoleUrl, roleId)).Result;
            if (!response.IsSuccessStatusCode)
            {
                var responseBody = response.Content.ReadAsStringAsync().Result;
                throw new Exception(responseBody);
            }

            var roleResponse = response.Content.ReadAsAsync<RoleResponse>().Result;
            return roleResponse;
        }

        private void UpdateRoleWithCapability(RoleResponse roleResponse, Capability capability)
        {
            var saveRoleRequest = new SaveRoleRequest
            {
                Name = roleResponse.Name,
                TenantId = roleResponse.TenantId,
                OrganizationId = roleResponse.OrganizationId,
                Capabilities = roleResponse.Capabilities
            };
            saveRoleRequest.Capabilities.Add(capability);

            var requestUri = string.Format("{0}/{1}", RoleUrl, roleResponse.Id);
            var response = RemoteApiTestHost.Client.PutAsJsonAsync(requestUri, saveRoleRequest).Result;
            if (!response.IsSuccessStatusCode)
            {
                var responseBody = response.Content.ReadAsStringAsync().Result;
                throw new Exception(responseBody);
            }
        }

        [Given(@"I give test user role ""(.*)"" for object (.*) of type (.*)")]
        public void GivenIGiveTestUserRoleForObject(string roleName, string orgObjectName, string objectOrgType)
        {
            var userId = TestUserFactory.GetGuid(ApiFeature.DefaultTestUser);
            var roleId = (Guid)ScenarioContext.Current[roleName];
            var orgObjectNameId = (Guid)ScenarioContext.Current[orgObjectName];

            GivenIGiveRoleToUserForObjectOfType(roleId, userId, orgObjectNameId, objectOrgType);
        }

        private void GivenIGiveRoleToUserForObjectOfType(Guid roleGuid, Guid userGuid, Guid orgObjectGuid, string objectOrgType)
        {
            var requestUri = string.Format("{0}/{1}/{2}/user/{3}", PermissionUrl, objectOrgType, orgObjectGuid, userGuid);
            var savePermissionRequest = new SavePermissionRequest { Roles = new List<Guid> { roleGuid } };
            var permissionResponse = RemoteApiTestHost.Client.PostAsJsonAsync(requestUri, savePermissionRequest).Result;

            if (!permissionResponse.IsSuccessStatusCode)
            {
                var responseBody = permissionResponse.Content.ReadAsStringAsync().Result;
                throw new Exception(responseBody);
            }
        }
    }

    public class SaveOrganizationRequest
    {
        public int? TenantId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Guid Parent { get; set; }
    }

    public class SaveRoleRequest
    {
        public Guid? OrganizationId { get; set; }

        public int? TenantId { get; set; }

        public string Name { get; set; }

        public List<Capability> Capabilities { get; set; }
    }

    public class SavePermissionRequest
    {
        public List<Guid> Roles { get; set; }
    }

    public class OrganizationResponse
    {
        public Guid Id { get; set; }
    }

    public class RoleResponse
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public int TenantId { get; set; }

        public string Name { get; set; }
        public List<Capability> Capabilities { get; set; }
    }
}
