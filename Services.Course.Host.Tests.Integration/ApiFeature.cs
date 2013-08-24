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
        public static readonly string AccountLeadingPath;

        public static WebApiTestHost ApiTestHost
        {
            get { return (WebApiTestHost)FeatureContext.Current["ApiTestHost"]; }
        }

        public static WebApiTestHost AccountApiTestHost
        {
            get { return (WebApiTestHost)FeatureContext.Current["AccountApiTestHost"]; }
        }

        static ApiFeature()
        {
            var targetUri = new Uri(ConfigurationManager.AppSettings["TestHostBaseAddress"]);
            LeadingPath = targetUri.Host.Equals("localhost") ? "" : targetUri.PathAndQuery;

            var remoteTargetUri = new Uri(ConfigurationManager.AppSettings["AccountTestHostBaseAddress"]);
            AccountLeadingPath = remoteTargetUri.Host.Equals("localhost") ? "" : remoteTargetUri.PathAndQuery;
        }

        [BeforeFeature("Api")]
        public static void BeforeFeature()
        {
            var featureContext = FeatureContext.Current;
           
            var apiTestHost = new WebApiTestHost(WebApiApplication.ConfigureWebApi, new Uri(ConfigurationManager.AppSettings["TestHostBaseAddress"]));
            var accountApiTestHost = new WebApiTestHost(WebApiApplication.ConfigureWebApi, new Uri(ConfigurationManager.AppSettings["AccountTestHostBaseAddress"]));
            featureContext.Add("ApiTestHost", apiTestHost);
            featureContext.Add("AccountApiTestHost", accountApiTestHost);
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
            ScenarioContext.Current.Add("Organizations", new Dictionary<string, OrganizationResource>());

            var defaultOrg = new OrganizationResource
                {
                    Id = Guid.Parse("E2DF063D-E2A1-4F83-9BE0-218EC676C05F")
                };

            StepSetups.Account.Givens.Organizations.Add("Default", defaultOrg);

            //Some scenarios change the user, so make sure we set it to a know user for each scenario
            ApiTestHost.SetTestUser(DefaultTestUser);
            AccountApiTestHost.SetTestUser(TestUserName.SuperSaml);
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