using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using BpeProducts.Common.WebApiTest;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources;
using TechTalk.SpecFlow;

namespace BpeProducts.Services.Course.Host.Tests.Integration
{
    [Binding]
    public static class ApiFeature
    {
        public const TestUserName DefaultTestUser = TestUserName.TestUser3;

        public static int TenantId = 999999;
        public static readonly string LeadingPath;

        public static WebApiTestHost ApiTestHost
        {
            get { return (WebApiTestHost)FeatureContext.Current["ApiTestHost"]; }
        }

        public static WebApiTestHost RemoteApiTestHost
        {
            get { return (WebApiTestHost)FeatureContext.Current["RemoteApiTestHost"]; }
        }

        static ApiFeature()
        {
            var targetUri = new Uri(ConfigurationManager.AppSettings["TestHostBaseAddress"]);
            LeadingPath = targetUri.Host.Equals("localhost") ? "" : targetUri.PathAndQuery;
        }

        [BeforeFeature("Api")]
        public static void BeforeFeature()
        {
            var featureContext = FeatureContext.Current;
           
            var apiTestHost = new WebApiTestHost(WebApiApplication.ConfigureWebApi, new Uri(ConfigurationManager.AppSettings["TestHostBaseAddress"]));
            var remoteApiTestHost = new WebApiTestHost(WebApiApplication.ConfigureWebApi, new Uri(ConfigurationManager.AppSettings["RemoteTestHostBaseAddress"]));
            featureContext.Add("ApiTestHost", apiTestHost);
            featureContext.Add("RemoteApiTestHost", remoteApiTestHost);
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
            ApiTestHost.Dispose();
        }
        
        [BeforeScenario("Api")]
        public static void BeforeScenario()
        {
            ScenarioContext.Current.Add("Responses", new List<HttpResponseMessage>());

            ScenarioContext.Current.Add("Courses", new Dictionary<string, CourseResource>());
            ScenarioContext.Current.Add("Programs", new Dictionary<string, ProgramResource>());
            ScenarioContext.Current.Add("Segments", new Dictionary<string, CourseSegmentResource>());
            ScenarioContext.Current.Add("LearningOutcomes", new Dictionary<string, LearningOutcomeResource>());
            ScenarioContext.Current.Add("CourseLearningActivities", new Dictionary<string, CourseLearningActivityResource>());

            //Some scenarios change the user, so make sure we set it to a know user for each scenario
            ApiTestHost.SetTestUser(DefaultTestUser);
            RemoteApiTestHost.SetTestUser(TestUserName.SuperSaml);
        }

        public static string GetDefaultConnectionString()
        {
            return GetDefaultConnectionString("DefaultConnection");
        }

        public static string GetDefaultConnectionString(string connectionName)
        {
            return ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }

    }
}