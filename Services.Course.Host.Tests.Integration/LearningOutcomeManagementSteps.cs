using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using BpeProducts.Services.Course.Contract;
using System.Net.Http.Formatting;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace BpeProducts.Services.Course.Host.Tests.Integration
{
    [Binding]
    public class LearningOutcomeManagementSteps
    {
        [When(@"I create a learning outcome with the description '(.*)'")]
        [Given(@"I have a learning outcome with the description '(.*)'")]
        public void WhenICreateALearningOutcomeWithTheDescription(string description)
        {
            var outcomeRequest = new OutcomeRequest {Description = description, TenantId = 1};
            var postUrl = FeatureContext.Current.Get<String>("OutcomeLeadingPath");
            var response =
                ApiFeature.ApiTestHost.Client.PostAsync(postUrl, outcomeRequest, new JsonMediaTypeFormatter()).Result;
            response.EnsureSuccessStatusCode();

            ScenarioContext.Current.Add("learningoutcomeResourceUrl", response.Headers.Location);
        }

        [When(@"I change the description to '(.*)'")]
        public void WhenIChangeTheDescriptionTo(string description)
        {
            var outcomeRequest = new OutcomeRequest { Description = description, TenantId = 1 };
            var putUrl = ScenarioContext.Current.Get<Uri>("learningoutcomeResourceUrl");
            var response =
                ApiFeature.ApiTestHost.Client.PutAsync(putUrl.ToString(), outcomeRequest, new JsonMediaTypeFormatter()).Result;
            response.EnsureSuccessStatusCode();
        }
        
        [When(@"I delete this learning outcome")]
        public void WhenIDeleteThisLearningOutcome()
        {
            var deleteUrl = ScenarioContext.Current.Get<Uri>("learningoutcomeResourceUrl");
            var response = ApiFeature.ApiTestHost.Client.DeleteAsync(deleteUrl).Result;
            response.EnsureSuccessStatusCode();
        }
        
        [Then(@"the learning outcome should be with the description '(.*)'")]
        public void ThenTheLearningOutcomeShouldBeWithTheDescription(string description)
        {
            var getUrl = ScenarioContext.Current.Get<Uri>("learningoutcomeResourceUrl");
            var response = ApiFeature.ApiTestHost.Client.GetAsync(getUrl.ToString()).Result;
            response.EnsureSuccessStatusCode();

            var outcomeResponse = response.Content.ReadAsAsync<OutcomeResponse>().Result;

            Assert.That(outcomeResponse.Description, Is.EqualTo(description));
        }
        
        [Then(@"the learning outcome shoud no longer exist")]
        public void ThenTheLearningOutcomeShoudNoLongerExist()
        {
            var getUrl = ScenarioContext.Current.Get<Uri>("learningoutcomeResourceUrl");
            var response = ApiFeature.ApiTestHost.Client.GetAsync(getUrl.ToString()).Result;
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

		[When(@"I associate to the '(.*)' '(.*)' a new learning outcome with the description '(.*)'")]
		public void WhenIAssociateToTheANewLearningOutcomeWithTheDescription(string entityType, string entityName, string outCome)
		{
			// PUT /program/programid/outcome
			ProgramResponse program = null;
			if (entityType.ToLower() == "program")
			{
				var programs = ScenarioContext.Current.Get<IList<ProgramResponse>>("programs");
				program = programs.First(p => p.Name.Equals(entityName));
			}

			var postUrl = string.Format("{0}/{1}/outcome",
			                            FeatureContext.Current.Get<String>("ProgramLeadingPath"), program.Id);

			var request = new OutcomeRequest
				{
					Description = outCome,
					TenantId = 1
				};
			var response = ApiFeature.ApiTestHost.Client.PostAsync(postUrl,request,new JsonMediaTypeFormatter()).Result;
			response.EnsureSuccessStatusCode();
			ScenarioContext.Current.Add("learningoutcomeResourceUrl", response.Headers.Location);
		}

		[Then(@"the '(.*)' '(.*)' is associated with learning outcome '(.*)'")]
		public void ThenTheIsAssociatedWithLearningOutcome(string entityType, string entityName, string outcome)
		{
			var getUrl = ScenarioContext.Current.Get<Uri>("learningoutcomeResourceUrl");
			var response = ApiFeature.ApiTestHost.Client.GetAsync(getUrl.ToString()).Result;
			response.EnsureSuccessStatusCode();
			var outcomeResponse = response.Content.ReadAsAsync<OutcomeResponse>().Result;
			Assert.That(outcomeResponse.Description, Is.EqualTo(outcome));
		}


    }
}
