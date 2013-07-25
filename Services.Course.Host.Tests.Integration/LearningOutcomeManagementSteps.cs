using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using BpeProducts.Services.Course.Contract;
using System.Net.Http.Formatting;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace BpeProducts.Services.Course.Host.Tests.Integration
{
    [Binding]
    public class LearningOutcomeManagementSteps
    {
        private const int Tenant = 999999;

        [When(@"I create a learning outcome with the description '(.*)'")]
        [Given(@"I have a learning outcome with the description '(.*)'")]
        public void WhenICreateALearningOutcomeWithTheDescription(string description)
        {
            var outcomeRequest = new OutcomeRequest { Description = description, TenantId = Tenant };
            var postUrl = FeatureContext.Current.Get<String>("OutcomeLeadingPath");
            var response =
                ApiFeature.ApiTestHost.Client.PostAsync(postUrl, outcomeRequest, new JsonMediaTypeFormatter()).Result;
            response.EnsureSuccessStatusCode();

            ScenarioContext.Current.Add("learningoutcomeResourceUrl", response.Headers.Location);
        }

        [When(@"I change the description to '(.*)'")]
        public void WhenIChangeTheDescriptionTo(string description)
        {
            var outcomeRequest = new OutcomeRequest { Description = description, TenantId = Tenant };
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

            var outcomeResponse = response.Content.ReadAsAsync<OutcomeInfo>().Result;

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

			var postUrl = string.Format("{0}/{1}/ToBeDetermined", FeatureContext.Current.Get<String>("ProgramLeadingPath"), program.Id);

			var request = new OutcomeRequest
				{
					Description = outCome,
                    TenantId = Tenant
				};
			var response = ApiFeature.ApiTestHost.Client.PostAsync(postUrl,request,new JsonMediaTypeFormatter()).Result;
			response.EnsureSuccessStatusCode();
			ScenarioContext.Current.Add("learningoutcomeResourceUrl", response.Headers.Location);
		}

		[When(@"I associate the following learning outcomes to '(.*)' program:")]
		public void WhenIAssociateTheFollowingLearningOutcomesToProgram(string programName, Table table)
		{
			// PUT /program/programid/outcome
			ProgramResponse program = null;
			
			var programs = ScenarioContext.Current.Get<IList<ProgramResponse>>("programs");
			program = programs.First(p => p.Name.Equals(programName));
			

			var postUrl = string.Format("{0}/{1}/ToBeDetermined/outcome",
										FeatureContext.Current.Get<String>("ProgramLeadingPath"), program.Id);
			var outcomeUrls = new Dictionary<string, Uri>();
			if (ScenarioContext.Current.ContainsKey("learningoutcomeResourceUrls"))
			{
				outcomeUrls = ScenarioContext.Current.Get<Dictionary<string, Uri>>("learningoutcomeResourceUrls") ?? new Dictionary<string, Uri>();
			}
			

			foreach (var item in table.Rows)
			{
				var request = new OutcomeRequest
				{
					Description = item["Description"],
                    TenantId = Tenant
				};
				var response = ApiFeature.ApiTestHost.Client.PostAsync(postUrl, request, new JsonMediaTypeFormatter()).Result;
				response.EnsureSuccessStatusCode();
				
				if (outcomeUrls.ContainsKey(item["Description"]))
				{
					outcomeUrls[item["Description"]] = response.Headers.Location;
				}
				else
				{
					outcomeUrls.Add(item["Description"], response.Headers.Location);
				}
				
				
			}

			if (ScenarioContext.Current.ContainsKey("learningoutcomeResourceUrls"))
			{
				ScenarioContext.Current["learningoutcomeResourceUrls"] = outcomeUrls;
				
			}
			else
			{
				ScenarioContext.Current.Add("learningoutcomeResourceUrls", outcomeUrls);
			}
			
			
		}

		[Then(@"the '(.*)' '(.*)' is associated with learning outcome '(.*)'")]
		public void ThenTheIsAssociatedWithLearningOutcome(string entityType, string entityName, string outcome)
		{
			var getUrl = ScenarioContext.Current.Get<Uri>("learningoutcomeResourceUrl");
			var response = ApiFeature.ApiTestHost.Client.GetAsync(getUrl.ToString()).Result;
			response.EnsureSuccessStatusCode();
			var outcomeResponse = response.Content.ReadAsAsync<OutcomeInfo>().Result;
			Assert.That(outcomeResponse.Description, Is.EqualTo(outcome));
		}

		

		[Then(@"'(.*)' program is associated with the following learning outcomes:")]
		public void ThenProgramIsAssociatedWithTheFollowingLearningOutcomes(string programName, Table table)
		{
			var outcomeUrls = ScenarioContext.Current.Get<Dictionary<string, Uri>>("learningoutcomeResourceUrls");
			foreach (var item in table.Rows)
			{
				var getUrl = outcomeUrls[item["Description"]];
				var response = ApiFeature.ApiTestHost.Client.GetAsync(getUrl.ToString()).Result;
				response.EnsureSuccessStatusCode();
				var outcomeResponse = response.Content.ReadAsAsync<OutcomeInfo>().Result;
				Assert.That(outcomeResponse.Description, Is.EqualTo(item["Description"]));
			}

		}

		[When(@"I disassociate the following learning outcomes from '(.*)' program:")]
		public void WhenIDisassociateTheFollowingLearningOutcomesFromProgram(string programName, Table table)
		{
			var outcomeUrls = ScenarioContext.Current.Get<Dictionary<string, Uri>>("learningoutcomeResourceUrls");
			foreach (var item in table.Rows)
			{
				var deleteUrl = outcomeUrls[item["Description"]];
				var response = ApiFeature.ApiTestHost.Client.DeleteAsync(deleteUrl.ToString()).Result;
				ScenarioContext.Current.Add("Delete" + item["Description"],response);
				response.EnsureSuccessStatusCode();
			}
		}

		[Then(@"'(.*)' program is associated with the only following learning outcomes:")]
		public void ThenProgramIsAssociatedWithTheOnlyFollowingLearningOutcomes(string programName, Table table)
		{
			//get the program and its outcome and assert on count and desc
			// PUT /program/programid/outcome
			ProgramResponse program = null;

			var programs = ScenarioContext.Current.Get<IList<ProgramResponse>>("programs");
			program = programs.First(p => p.Name.Equals(programName));


            var getUrl = string.Format("{0}/{1}/ToBeDetermined",
										FeatureContext.Current.Get<String>("ProgramLeadingPath"), program.Id);

			var response = ApiFeature.ApiTestHost.Client.GetAsync(getUrl).Result;
			response.EnsureSuccessStatusCode();
			var outcomes = response.Content.ReadAsAsync<List<OutcomeInfo>>().Result;
			Assert.That(outcomes.Count,Is.EqualTo(table.Rows.Count));
			foreach (var row in table.Rows)
			{
				Assert.That(outcomes.Any(o=>o.Description==row["Description"]));
			}
		}

		[Then(@"disassociating the following from '(.*)' program should result:")]
		public void ThenDisassociatingTheFollowingFromProgramShouldResult(string p0, Table table)
		{
			var outcomeUrls = ScenarioContext.Current.Get<Dictionary<string, Uri>>("learningoutcomeResourceUrls");
			String deleteUrl="";
			foreach (var item in table.Rows)
			{
				
				if (outcomeUrls.ContainsKey(item["Description"]))
				{
					deleteUrl = outcomeUrls[item["Description"]].ToString();
				}
				else
				{
					deleteUrl=deleteUrl.Substring(0,deleteUrl.LastIndexOf("/")+1) + Guid.NewGuid().ToString();//craft a fake
				}
				var response = ApiFeature.ApiTestHost.Client.DeleteAsync(deleteUrl).Result;

				var expectedStatusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), item["Disassociation Response"]);

				Assert.That(response.StatusCode.Equals(expectedStatusCode));

			}
		}

        /*
        [Given(@"I assoicate the following outcomes to '(.*)'")]
        public void GivenIAssoicateTheFollowingOutcomesTo(string parentOutcome, Table table)
        {
            // create the parent outcome
            var outcomeRequest = new OutcomeRequest {Description = parentOutcome, TenantId = 1};
            var postUri = FeatureContext.Current.Get<string>("OutcomeLeadningPath");
            
            var response =
                ApiFeature.ApiTestHost.Client.PostAsync(postUri, outcomeRequest, new JsonMediaTypeFormatter()).Result;
            response.EnsureSuccessStatusCode();

            ScenarioContext.Current.Add(parentOutcome, response.Headers.Location);

            var outcomeRequests = table.CreateSet<OutcomeRequest>();
            outcomeRequests.ToList().ForEach(o =>
                {
                    response =
                        ApiFeature.ApiTestHost.Client.PostAsync(postUri, o, new JsonMediaTypeFormatter()).Result;
                    response.EnsureSuccessStatusCode();
                });
        }

        [Then(@"'(.*)' has the following learning outcomes:")]
        public void ThenHasTheFollowingLearningOutcomes(string parentOutcome, Table table)
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I disassociate '(.*)' from '(.*)'")]
        public void GivenIDisassociateFrom(string p0, string p1)
        {
            ScenarioContext.Current.Pending();
        }
        */

        [Given(@"the following course exists:")]
        public void GivenTheFollowingCourseExists(Table table)
        {
            var saveCourseRequest = table.CreateInstance<SaveCourseRequest>();
            var postUri = FeatureContext.Current.Get<string>("CourseLeadingPath");

            var response =
                ApiFeature.ApiTestHost.Client.PostAsync(postUri, saveCourseRequest, new JsonMediaTypeFormatter()).Result;
            response.EnsureSuccessStatusCode();

            ScenarioContext.Current.Add(saveCourseRequest.Code, response.Headers.Location);
        }

        [Given(@"I associate the following learning outcomes to '(.*)' program:")]
        public void GivenIAssociateTheFollowingLearningOutcomesToProgram(string programName, Table table)
        {
            // PUT /program/programid/outcome
            ProgramResponse program = null;

            var programs = ScenarioContext.Current.Get<IList<ProgramResponse>>("programs");
            program = programs.First(p => p.Name.Equals(programName));
            var postUri = string.Format("{0}/{1}/ToBeDetermined", FeatureContext.Current.Get<string>("ProgramLeadingPath"), program.Id);

            var outcomeRequests = table.CreateSet<OutcomeRequest>();
            outcomeRequests.ToList().ForEach(o =>
                {
                    var response = ApiFeature.ApiTestHost.Client.PostAsync(postUri, o, new JsonMediaTypeFormatter()).Result;
                    response.EnsureSuccessStatusCode();

                    ScenarioContext.Current.Add(o.Description, response.Headers.Location);
                });
        }

        [Given(@"I assoicate the following learning outcomes to '(.*)' course:")]
        public void GivenIAssoicateTheFollowingLearningOutcomesToCourse(string courseName, Table table)
        {
            var postUri = string.Format("{0}/ToBeDetermined", ScenarioContext.Current.Get<Uri>(courseName));
            var outcomeRequests = table.CreateSet<OutcomeRequest>();
            outcomeRequests.ToList().ForEach(o =>
            {
                var response = ApiFeature.ApiTestHost.Client.PostAsync(postUri, o, new JsonMediaTypeFormatter()).Result;
                response.EnsureSuccessStatusCode();

                ScenarioContext.Current.Add(o.Description, response.Headers.Location);
            });
        }

        [Given(@"I assoicate the following outcomes to '(.*)'")]
        [When(@"I assoicate the following outcomes to '(.*)'")]
        public void WhenIAssoicateTheFollowingOutcomesTo(string outcomeDescription, Table table)
        {
            var parentOutcomeResourceUri = ScenarioContext.Current.Get<Uri>(outcomeDescription);
            var parentOutcomeId = ExtractGuid(parentOutcomeResourceUri.ToString(), 1);

            foreach (var row in table.Rows)
            {
                var childOutcomeDescription = row["Description"];
                var childOutcomeResourceUri = ScenarioContext.Current.Get<Uri>(childOutcomeDescription);

                var childOutcomeId = ExtractGuid(childOutcomeResourceUri.ToString(), 1);
                var putUri = string.Format("{0}/{1}/supports/{2}", 
                    FeatureContext.Current.Get<string>("OutcomeLeadingPath"), childOutcomeId, parentOutcomeId);

                var response = ApiFeature.ApiTestHost.Client.PutAsync(putUri, new {}, new JsonMediaTypeFormatter()).Result;
                response.EnsureSuccessStatusCode();
            }
        }

        private string ExtractGuid(string str, int i)
        {
            var p = @"([a-z0-9]{8}[-][a-z0-9]{4}[-][a-z0-9]{4}[-][a-z0-9]{4}[-][a-z0-9]{12})";
            var mc = Regex.Matches(str, p);

            return mc[i].ToString();
        }

        [Then(@"'(.*)' has the following learning outcomes:")]
        public void ThenHasTheFollowingLearningOutcomes(string outcomeDescription, Table table)
        {
            var parentOutcomeResourceUri = ScenarioContext.Current.Get<Uri>(outcomeDescription);
            var parentOutcomeId = ExtractGuid(parentOutcomeResourceUri.ToString(), 1);

            var getUri = string.Format("{0}/{1}/supports", FeatureContext.Current.Get<string>("OutcomeLeadingPath"), parentOutcomeId);

            var response = ApiFeature.ApiTestHost.Client.GetAsync(getUri).Result;
            response.EnsureSuccessStatusCode();

            var childOutcomes = response.Content.ReadAsAsync<IList<OutcomeInfo>>().Result;
            table.CompareToSet(childOutcomes);
        }

        [When(@"I disassociate '(.*)' from '(.*)'")]
        public void WhenIDisassociateFrom(string childOutcome, string parentOutcome)
        {
            var parentOutcomeResourceUri = ScenarioContext.Current.Get<Uri>(parentOutcome);
            var parentOutcomeId = ExtractGuid(parentOutcomeResourceUri.ToString(), 1);
            var childOutcomeResourceUri = ScenarioContext.Current.Get<Uri>(childOutcome);
            var childOutcomeId = ExtractGuid(childOutcomeResourceUri.ToString(), 1);

            var deleteUri = string.Format("{0}/{1}/supports/{2}", FeatureContext.Current.Get<string>("OutcomeLeadingPath"),childOutcomeId, parentOutcomeId);

            var response = ApiFeature.ApiTestHost.Client.DeleteAsync(deleteUri).Result;
            response.EnsureSuccessStatusCode();
        }

        [Then(@"the course '(.*)' includes the following learning outcomes:")]
        public void ThenTheCourseIncludesTheFollowingLearningOutcomes(string description, Table table)
        {
            var getUri = ScenarioContext.Current.Get<Uri>(description);
            var response = ApiFeature.ApiTestHost.Client.GetAsync(getUri.ToString() + "/" + "ToBeDetermined").Result;
            var outcomes = response.Content.ReadAsAsync<List<OutcomeInfo>>().Result;
            table.CompareToSet(outcomes);
        }
    }
}
