using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using BpeProducts.Common.Capabilities;
using BpeProducts.Common.WebApiTest;
using BpeProducts.Services.Course.Contract;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace BpeProducts.Services.Course.Host.Tests.Integration
{
	[Binding]
	public class CourseAuthorizationSteps
	{
        private const int TenantId = 999999;
        private const string OrganizationUrl = "https://devapi.thuze.com/Account/organization";
        private const string RoleUrl = "account/role";
        private const string PermissionUrl = "account/permission";
        private readonly string _leadingPath;

		private AuthenticationHeaderValue _originalToken;
        
        //TODO: Move methods to Given/When/Then/AccountGive/AccountWhen/etc.

		// For additional details on SpecFlow step definitions see http://go.specflow.org/doc-stepdef
		public CourseAuthorizationSteps()
        {
            var targetUri = new Uri(ConfigurationManager.AppSettings["TestHostBaseAddress"]);
            if (!targetUri.Host.Equals("localhost"))
            {
                _leadingPath = targetUri.PathAndQuery + "/course";
            }
            else
            {
                _leadingPath = "/course";
            }
        }

		[BeforeScenario]
		public void BeforeScenario()
		{
			_originalToken = ApiFeature.ApiTestHost.Client.DefaultRequestHeaders.Authorization;
		}

		[AfterScenario]
		public void AfterScenario()
		{
			ApiFeature.ApiTestHost.Client.DefaultRequestHeaders.Authorization = _originalToken;
		}

		[Given(@"That I am admin")]
        public void GivenThatIAmAdmin()
        {
            ApiFeature.ApiTestHost.SetTestUser(TestUserName.SuperSaml);
        }

		[Given(@"That I am guest")]
		public void GivenThatIAmGuest()
		{
            ApiFeature.ApiTestHost.SetTestUser(TestUserName.GuestUser1);           
		}

		[When(@"I submit an authorized creation request")]
		public void WhenISubmitAnAuthorizedCreationRequest()
		{
			var saveCourseRequest = new SaveCourseRequest()
				{
					Name = "Test",
					Code = "Test",
					Description = "Test",
					TenantId = 999999,					              
					OrganizationId = Guid.NewGuid()
				};
			var response = ApiFeature.ApiTestHost.Client.PostAsync(_leadingPath, saveCourseRequest, new JsonMediaTypeFormatter()).Result;

			if (ScenarioContext.Current.ContainsKey("createCourseResponse"))
			{
				ScenarioContext.Current.Remove("createCourseResponse");
			}
			ScenarioContext.Current.Add("createCourseResponse", response);

			// this is the response to ensure the success code
			if (ScenarioContext.Current.ContainsKey("responseToValidate"))
			{
				ScenarioContext.Current.Remove("responseToValidate");
			}
			ScenarioContext.Current.Add("responseToValidate", response);
		}

		[Then(@"I should get a success response")]
		public void ThenIShouldGetASuccessResponse()
		{
			var response = ScenarioContext.Current.Get<HttpResponseMessage>("responseToValidate");
			response.EnsureSuccessStatusCode();
		}

		[Then(@"I should get a failure response")]
		public void ThenIShouldGetAFailureResponse()
		{
			var response = ScenarioContext.Current.Get<HttpResponseMessage>("responseToValidate");
			Assert.AreEqual(response.StatusCode,HttpStatusCode.Unauthorized);
		}

        

        

        public static WebApiTestHost RemoteApiTestHost
        {
            get { return (WebApiTestHost)FeatureContext.Current["RemoteApiTestHost"]; }
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
                var parent = (Guid)ScenarioContext.Current[parentOrganizationName];
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

        [Given(@"I give role ""(.*)"" to user ""(.*)"" for object ""(.*)"" of type ""(.*)""")]
        public void GivenIGiveRoleToUserForObjectOfType(string uniqueRoleKey, string userNameToAssign, string orgObjectName, string objectOrgType)
        {
            var userId = (Guid)ScenarioContext.Current[userNameToAssign];
            var roleId = (Guid)ScenarioContext.Current[uniqueRoleKey];
            var orgObjectNameId = (Guid)ScenarioContext.Current[orgObjectName];

            var requestUri = string.Format("{0}/{1}/{2}/user/{3}", PermissionUrl, objectOrgType, orgObjectNameId, userId);
            var savePermissionRequest = new SavePermissionRequest { Roles = new List<Guid> { roleId } };
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

    public class UpdateRoleRequest
    {
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
