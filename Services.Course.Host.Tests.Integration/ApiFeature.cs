using System.Threading.Tasks;
using Autofac;
using BpeProducts.Common.Authorization;
using BpeProducts.Common.WebApi.ApiKeys;
using BpeProducts.Common.WebApiTest;
using BpeProducts.Services.Course.Domain;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources.Account;
using System;
using Moq;
using TechTalk.SpecFlow;
using BpeProducts.Common.WebApiTest.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Integration
{
    [Binding]
    public static class ApiFeature
    {
        private const TestUserName DefaultTestUser = TestUserName.TestUser3;
        private static TestUserName _currentTestUser = DefaultTestUser;
        public static TestUserName CurrentTestUser {
            get { return _currentTestUser; }
            set { 
                _currentTestUser = value;
                CourseTestHost.SetTestUser(CurrentTestUser);
            }
        }

        public static int TenantId = 999999;
        public static readonly string LeadingPath;

        public const string BaseAddress = "http://localhost:12003";

        public static WebApiSelfTestHost CourseTestHost
        {
            get { return (WebApiSelfTestHost)FeatureContext.Current["CourseTestHost"]; }
        }

        public static Mock<IAclHttpClient> MockAclClient { get; private set; }
        public static Mock<ISectionClient> MockSectionClient { get; private set; }
        public static Mock<IAssetServiceClient> MockAssetClient { get; private set; }
        public static Mock<IAssessmentClient> MockAssessmentClient { get; private set; }
        public static Mock<ITenantClient> MockTenantClient { get; private set; }

        static ApiFeature()
        {
            var targetUri = new Uri(BaseAddress);
            LeadingPath = targetUri.Host.Equals("localhost") ? "" : targetUri.PathAndQuery;
        }

        [BeforeFeature("Api")]
        public static void BeforeFeature()
        {
            var featureContext = FeatureContext.Current;

            var courseApiTestHost = new WebApiSelfTestHost(new Uri(BaseAddress));
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

            MockAclClient = new Mock<IAclHttpClient>();
            MockSectionClient = new Mock<ISectionClient>();
            MockAssetClient = new Mock<IAssetServiceClient>();
            MockAssessmentClient = new Mock<IAssessmentClient>();
            MockTenantClient = new Mock<ITenantClient>();
            MockTenantClient.Setup(x => x.ApiKeyGet(It.IsAny<Guid>()))
                .Returns(
                    Task.FromResult(new ApiKeyResponse
                    {
                        SecretKey =
                            "LqzGgBRX8UE68xxWMQX5psO+qfoWWCBGGMJXivAdPc9kctbM17xcMZxJ0EyQqY3gRoC8dEtIHonOG2K3o9n3Cw==",
                        TenantId = TenantId
                    }));
            
            var updater = new ContainerBuilder();
            updater.RegisterInstance(MockAclClient.Object).As<IAclHttpClient>();
            updater.RegisterInstance(MockSectionClient.Object).As<ISectionClient>();
            updater.RegisterInstance(MockAssetClient.Object).As<IAssetServiceClient>();
            updater.RegisterInstance(MockAssessmentClient.Object).As<IAssessmentClient>();
            updater.RegisterInstance(MockTenantClient.Object).As<ITenantClient>();
            updater.Update(CourseTestHost.Container);

            //Some scenarios change the user, so make sure we set it to a know user for each scenario
            CurrentTestUser = DefaultTestUser;
        }
    }
}