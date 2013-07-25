using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text.RegularExpressions;
using BpeProducts.Services.Course.Contract;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace BpeProducts.Services.Course.Host.Tests.Integration
{
    [Binding]
    public class LearningOutcomeVersioningSteps
    {
        [Given(@"I create the following learning outcome")]
        public void GivenICreateTheFollowingLearningOutcome(Table table)
        {
            var outcomeRequest = table.CreateInstance<OutcomeRequest>();
            var postUrl = FeatureContext.Current.Get<String>("OutcomeLeadingPath");
            var response = ApiFeature.ApiTestHost.Client.PostAsync(postUrl, outcomeRequest, new JsonMediaTypeFormatter()).Result;
            response.EnsureSuccessStatusCode();

            ScenarioContext.Current.Add(outcomeRequest.Description, response.Headers.Location);
        }
        
        [When(@"I retrieve '(.*)' learning outcome")]
        public void WhenIRetrieveLearningOutcome(string description)
        {
            var getUrl = ScenarioContext.Current.Get<Uri>(description);
            var response = ApiFeature.ApiTestHost.Client.GetAsync(getUrl.ToString()).Result;
            response.EnsureSuccessStatusCode();

            var outcomeResponse = response.Content.ReadAsAsync<OutcomeInfo>().Result;
            ScenarioContext.Current.Add("outcomeResponse", outcomeResponse);
        }
        
        [When(@"I update '(.*)' learning outcome with the following info")]
        public void WhenIUpdateLearningOutcomeWithTheFollowingInfo(string description, Table table)
        {
            var outComeRequest = table.CreateInstance<OutcomeRequest>();
            var putUrl = ScenarioContext.Current.Get<Uri>(description);
            var response = ApiFeature.ApiTestHost.Client.PutAsync(putUrl.ToString(), outComeRequest, new JsonMediaTypeFormatter()).Result;
            // response.EnsureSuccessStatusCode();

            ScenarioContext.Current["ResponseToValidate"] = response;

            //var outcomeResponse = response.Content.ReadAsAsync<OutcomeResponse>().Result;
            //ScenarioContext.Current.Add("outcomeResponse", outcomeResponse);
        }

        [When(@"I create a new version of '(.*)' outcome with the following info")]
        public void WhenICreateANewVersionOfWithTheFollowingInfo(string description, Table table)
        {
            var versionRequest = table.CreateInstance<VersionRequest>();

            if (description.Equals("RandomOutcome"))
            {
                versionRequest.ParentVersionId = Guid.NewGuid();
            }
            else
            {
                var resourceUri = ScenarioContext.Current.Get<Uri>(description);
                versionRequest.ParentVersionId = Guid.Parse(ExtractGuid(resourceUri.ToString(), 0));
            }

            var postUri = string.Format("{0}/version", FeatureContext.Current.Get<string>("OutcomeLeadingPath"));

            var response = ApiFeature.ApiTestHost.Client.PostAsync(postUri, versionRequest, new JsonMediaTypeFormatter()).Result;

            ScenarioContext.Current[description] = response.Headers.Location;
            ScenarioContext.Current["ResponseToValidate"] = response;
        }

        [Given(@"I publish '(.*)' learning outcome with the following info")]
        [When(@"I publish '(.*)' learning outcome with the following info")]
        public void WhenIPublishLearningOutcomeWithTheFollowingInfo(string description, Table table)
        {
            var resourceUri = ScenarioContext.Current.Get<Uri>(description);
            var publishUri = string.Format("{0}/publish", resourceUri);
            var publishRequest = table.CreateInstance<PublishRequest>();

            var response = ApiFeature.ApiTestHost.Client.PutAsync(publishUri, publishRequest, new JsonMediaTypeFormatter()).Result;
            response.EnsureSuccessStatusCode();

            ScenarioContext.Current.Add("ResponseToValidate", response);
        }
        
        [When(@"I delete '(.*)' learning outcome")]
        public void WhenIDeleteLearningOutcome(string description)
        {
            var resourceUri = ScenarioContext.Current.Get<Uri>(description);
            var response = ApiFeature.ApiTestHost.Client.DeleteAsync(resourceUri).Result;

            ScenarioContext.Current["ResponseToValidate"] = response;
        }
        
        [When(@"I create a learning outcome without a version")]
        public void WhenICreateALearningOutcomeWithoutAVersion()
        {
            var versionRequest = new VersionRequest
            {
                ParentVersionId = Guid.NewGuid(),
                VersionNumber = null
            };
            var postUri = string.Format("{0}/version", FeatureContext.Current.Get<string>("OutcomeLeadingPath"));

            var response = ApiFeature.ApiTestHost.Client.PostAsync(postUri, versionRequest, new JsonMediaTypeFormatter()).Result;

            ScenarioContext.Current["ResponseToValidate"] = response;
        }

        //[Then(@"I get '(.*)' response")]
        //public void ThenIGetResponse(string status)
        //{
        //    var response = ScenarioContext.Current.Get<HttpResponseMessage>("ResponseToValidate");
        //    var expectedStatusCode = (HttpStatusCode) Enum.Parse(typeof (HttpStatusCode), status);

        //    Assert.That(response.StatusCode, Is.EqualTo(expectedStatusCode));
        //}

        [Then(@"the learning outcome should have the following info")]
        public void ThenTheLearningOutcomeShouldHaveTheFollowingInfo(Table table)
        {
            var outcomeInfo = ScenarioContext.Current.Get<OutcomeInfo>("outcomeResponse");
            table.CompareToInstance(outcomeInfo);
        }
        
        [Then(@"the learning outcome '(.*)' should have the following info")]
        public void ThenTheLearningOutcomeShouldHaveTheFollowingInfo(string description, Table table)
        {
            var getUri = ScenarioContext.Current.Get<Uri>(description);
            var response = ApiFeature.ApiTestHost.Client.GetAsync(getUri.ToString()).Result;
            response.EnsureSuccessStatusCode();

            var outcomeResponse = response.Content.ReadAsAsync<OutcomeInfo>().Result;
            table.CompareToInstance(outcomeResponse);
        }

        private string ExtractGuid(string str, int i)
        {
            var p = @"([a-z0-9]{8}[-][a-z0-9]{4}[-][a-z0-9]{4}[-][a-z0-9]{4}[-][a-z0-9]{12})";
            var mc = Regex.Matches(str, p);

            return mc[i].ToString();
        }
    }
}
