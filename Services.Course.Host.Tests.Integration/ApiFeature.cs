using System.Linq;
using Autofac;
using BpeProducts.Common.Authorization;
using BpeProducts.Common.WebApiTest;
using BpeProducts.Services.Asset.Contracts;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources.Account;
using System;
using System.Collections.Generic;
using System.Configuration;
using BpeProducts.Services.Section.Contracts;
using Moq;
using Services.Assessment.Contract;
using BpeProducts.Services.Section.Contracts;
using TechTalk.SpecFlow;
using BpeProducts.Common.WebApiTest.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Integration
{
    [Binding]
    public static class ApiFeature
    {
        public const TestUserName DefaultTestUser = TestUserName.TestUser3;

        public static int TenantId = 999999;
        public static readonly string LeadingPath;

        public const string BaseAddress = "https://localhost"; //ConfigurationManager.AppSettings["TestHostBaseAddress"]

        public static WebApiTestHost CourseTestHost
        {
            get { return (WebApiTestHost)FeatureContext.Current["CourseTestHost"]; }
        }

        public static Mock<IAclHttpClient> MockAclClient { get; private set; }
        public static Mock<ISectionClient> MockSectionClient { get; private set; }
        public static Mock<IAssetServiceClient> MockAssetClient { get; private set; }
        public static Mock<IAssessmentClient> MockAssessmentClient { get; private set; }

        static ApiFeature()
        {
            var targetUri = new Uri(BaseAddress);
            LeadingPath = targetUri.Host.Equals("localhost") ? "" : targetUri.PathAndQuery;
        }

        [BeforeFeature("Api")]
        public static void BeforeFeature()
        {
            var featureContext = FeatureContext.Current;

            var courseApiTestHost = new WebApiTestHost(WebApiApplication.ConfigureWebApi, new Uri(BaseAddress));
            featureContext.Add("CourseTestHost", courseApiTestHost);
            featureContext.Add("TenantId", TenantId);

            var courseLeadingPath = string.Format("{0}/{1}", LeadingPath, "course");
            var programLeadingPath = string.Format("{0}/{1}", LeadingPath, "program");
            var outcomeLeadingPath = string.Format("{0}/{1}", LeadingPath, "outcome");
            const string accountLeadingPath = "/account";

            featureContext.Add("CourseLeadingPath", courseLeadingPath);
            featureContext.Add("ProgramLeadingPath", programLeadingPath);
            featureContext.Add("OutcomeLeadingPath", outcomeLeadingPath);
            featureContext.Add("AccountLeadingPath", accountLeadingPath);
        }

        [AfterFeature("Api")]
        public static void AfterFeature()
        {
            CourseTestHost.Dispose();
        }
        
        [BeforeScenario("Api")]
        public static void BeforeScenario()
        {
            var defaultOrg = new OrganizationResource
                {
                    Id = Guid.Parse("E2DF063D-E2A1-4F83-9BE0-218EC676C05F")
                };

            Resources<OrganizationResource>.Add("Default", defaultOrg);

            //Some scenarios change the user, so make sure we set it to a know user for each scenario
            CourseTestHost.SetTestUser(DefaultTestUser);;

            MockAclClient = new Mock<IAclHttpClient>();
            MockSectionClient = new Mock<ISectionClient>();
            MockAssetClient = new Mock<IAssetServiceClient>();
            MockAssessmentClient = new Mock<IAssessmentClient>();
            
            var updater = new ContainerBuilder();
            updater.RegisterInstance(MockAclClient.Object).As<IAclHttpClient>();
            updater.RegisterInstance(MockSectionClient.Object).As<ISectionClient>();
            updater.RegisterInstance(MockAssetClient.Object).As<IAssetServiceClient>();
            updater.RegisterInstance(MockAssessmentClient.Object).As<IAssessmentClient>();
            updater.Update(CourseTestHost.Container);
        }
    }
}