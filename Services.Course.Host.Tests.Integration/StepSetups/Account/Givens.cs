using BpeProducts.Common.Authorization;
using BpeProducts.Common.WebApiTest;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources.Account;
using Moq;
using Services.Authorization.Contract;
using TechTalk.SpecFlow;
using BpeProducts.Common.WebApiTest.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Integration.StepSetups.Account
{
    [Binding]
    public class Givens
    {
        [Given(@"I have the following capabilities")]
        public void GivenIHaveTheFollowingCapabilities(Table table)
        {
            var assigned = from row
                           in table.Rows
                           select row["Capability"]
                               into value
                               where !string.IsNullOrWhiteSpace(value)
                               select (Capability)Enum.Parse(typeof(Capability), value);

            foreach (var capability in assigned)
            {
                var closureCopy = capability;
                ApiFeature.MockAclClient.Setup(a => a.HasAccess(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<Guid>(), (int)closureCopy)).Returns(true);
                ApiFeature.MockAclClient.Setup(a => a.HasAccess(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), (int)closureCopy)).Returns(true);
            }
        }

        [Given(@"I have the '(.*)' capability")]
        public void GivenIHaveTheCapability(string capabilityName)
        {
            if (!string.IsNullOrWhiteSpace(capabilityName))
            {
                var capability = (Capability)Enum.Parse(typeof(Capability), capabilityName);
                ApiFeature.MockAclClient.Setup(a => a.HasAccess(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<Guid>(), (int)capability)).Returns(true);
            }
        }

        [Given(@"I have the following object capabilities")]
        public void GivenIHaveTheFollowingObjectCapabilities(Table table)
        {
            foreach (var row in table.Rows)
            {
                var capabilityName = row["Capability"];
                var objectType = row["ObjectType"];
                var objectName = row["ObjectName"];

                var capability = (Capability)Enum.Parse(typeof(Capability), capabilityName);
                var resource = ResourceFactory.Get(objectType, objectName);

                ApiFeature.MockAclClient.Setup(a => a.HasAccess(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<Guid>(), resource.Id, (int)capability)).Returns(true);
            }
        }

        [Given(@"the following organizations exist")]
        public void GivenTheFollowingOrganizationsExist(Table table)
        {
            foreach (var row in table.Rows)
            {
                var orgName = row["Name"];
                Resources<OrganizationResource>.Add(orgName, new OrganizationResource { Id = Guid.NewGuid() });
            }
        }
    }
}
