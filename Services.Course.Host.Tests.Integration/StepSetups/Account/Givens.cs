using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Common.Capabilities;
using BpeProducts.Common.WebApiTest;
using BpeProducts.Services.Course.Host.Tests.Integration.Operations.Account;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace BpeProducts.Services.Course.Host.Tests.Integration.StepSetups.Account
{
    [Binding]
    public class Givens
    {
        public static IDictionary<string, OrganizationResource> Organizations
        {
            get
            {
                return ScenarioContext.Current.Get<IDictionary<string, OrganizationResource>>("Organizations");
            }
        }

        public static IDictionary<string, RoleResource> Roles
        {
            get
            {
                return ScenarioContext.Current.Get<IDictionary<string, RoleResource>>("Roles");
            }
        }

        [Given(@"I create the following roles")]
        public void GivenICreateTheFollowingRoles(Table table)
        {
            foreach (var row in table.Rows)
            {
                var parentOrgName = row["Organization"];

                var request = new SaveRoleRequest
                    {
                        Name = row["Name"],
                        OrganizationId =
                            string.IsNullOrEmpty(parentOrgName) ? Guid.Empty : Organizations[parentOrgName].Id
                    };

                Operations.Account.PostOperations.CreateRole(request.Name, request);
            }
        }

        [Given(@"the following organizations exist")]
        public void GivenTheFollowingOrganizationsExist(Table table)
        {
            foreach (var row in table.Rows)
            {
                var parentOrgName = row["ParentOrganization"];

                var request = new SaveOrganizationRequest
                {
                    Name = row["Name"],
                    Description = row["Description"],
                    Parent = string.IsNullOrEmpty(parentOrgName) ? Guid.Empty : Organizations[parentOrgName].Id
                };

                Operations.Account.PostOperations.CreateOrganization(request.Name, request);
            }
        }

        [Given(@"I give capability ""(.*)"" to role ""(.*)""")]
        [Given(@"I give capability (.*) to role ""(.*)""")]
        public void GivenIGiveCapabilityToRole(string capability, string roleName)
        {
            var resource = Roles[roleName];
            
            if (capability == "")
            {
                return;
            }

            var role = GetOperations.GetRole(resource.ResourceUri);

            var request = new UpdateRoleRequest
                {
                    Name = role.Name,
                    Capabilities = role.Capabilities
                };

            var capabilityEnum = (Capability)Enum.Parse(typeof(Capability), capability);
            request.Capabilities.Add(capabilityEnum);

            PutOperations.UpdateRoleWithCapability(resource, request);
        }

        [Given(@"I give the user role ""(.*)"" for organization ""(.*)""")]
        public void GivenIGiveTheUserRoleForOrganizationOrgTop(string roleName, string organizatonName)
        {
            var role = Roles[roleName];
            var org = Organizations[organizatonName];
            var userGuid = TestUserFactory.GetGuid(ApiFeature.DefaultTestUser);

            PostOperations.GrantPermission(userGuid, role, org);
        }
    }
}
