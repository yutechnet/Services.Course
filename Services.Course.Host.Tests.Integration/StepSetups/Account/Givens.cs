using BpeProducts.Common.Capabilities;
using BpeProducts.Common.WebApiTest;
using BpeProducts.Services.Course.Host.Tests.Integration.Operations.Account;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources.Account;
using TechTalk.SpecFlow;

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

        [Given(@"I am user ""(.*)""")]
        public void GivenIAmUser(TestUserName testUserName)
        {
            //TODO: replace this with an api call when we can create a user and get saml token on the fly
            //Code was copied from Services.Account
            var futureUserId = TestUserFactory.GetGuid(testUserName);
            const string query = "Insert into [User] (UserId, DateAdded, DateUpdated) Values (@uuid,@dateAdded,@dateUpdated) ";

            using (var connection = new SqlConnection(ApiFeature.GetDefaultConnectionString("AccountConnection")))
            {
                try
                {
                    var command = new SqlCommand(query, connection);
                    command.Parameters.Add(new SqlParameter("uuid", futureUserId));
                    command.Parameters.Add(new SqlParameter("dateAdded", DateTime.UtcNow));
                    command.Parameters.Add(new SqlParameter("dateUpdated", DateTime.UtcNow));
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                catch (SqlException exception)
                {
                    //2627 is a primary key constraint violation that assures us that the record is already in db and life is good
                    if (exception.Number != 2627) throw;
                }
            }
            ScenarioContext.Current[testUserName.ToString()] = futureUserId;
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

                PostOperations.CreateOrganization(request.Name, request);
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
                        OrganizationId = string.IsNullOrEmpty(parentOrgName) ? Guid.Empty : Organizations[parentOrgName].Id,
                        TenantId = ApiFeature.TenantId
                    };

                var capabilities = row["Capabilities"];
                if (capabilities != "")
                {
                    var capabilityList = capabilities.Split(',').Select(c => (Capability)Enum.Parse(typeof(Capability), c)).ToList();
                    request.Capabilities = capabilityList;
                }

                PostOperations.CreateRole(request.Name, request);
            }
        }

        [Given(@"I add capability ""(.*)"" to role ""(.*)""")]
        [Given(@"I add capability (.*) to role ""(.*)""")]
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

        [Given(@"I update the role ""(.*)"" with capabilities ""(.*)""")]
        public void GivenIUpdateTheRoleWithCapabilities(string roleName, string capabilities)
        {
            var resource = Roles[roleName];
            var role = GetOperations.GetRole(resource.ResourceUri);

            var request = new UpdateRoleRequest
                {
                    Name = role.Name
                };

            if (capabilities!="")
            {
                var capabilityList = capabilities.Split(',').Select(c=> (Capability)Enum.Parse(typeof(Capability), c)).ToList();
                request.Capabilities = capabilityList;
            }

            PutOperations.UpdateRoleWithCapability(resource, request);
        }

        [Given(@"I give the user role ""(.*)"" for organization (.*)")]
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
