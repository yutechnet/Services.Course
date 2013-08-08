using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Courses;
using BpeProducts.Services.Course.Host.Tests.Integration.StepSetups;
using TechTalk.SpecFlow;
using NUnit.Framework;
using TechTalk.SpecFlow.Assist;

namespace BpeProducts.Services.Course.Host.Tests.Integration
{
    [Binding]
    public class CourseLearningActivitySteps
    {
        private const string LearningActivityUrl = "/learningactivity/";
        public Uri RequestUri = ScenarioContext.Current.Get<Uri>("Week1");

        [Given(@"the following learning activity:")]
        public void GivenTheFollowingLearningActivity(Table table)
        {
            var activity = table.CreateInstance<SaveCourseLearningActivityRequest>();
	    activity.TenantId = ApiFeature.TenantId;
	    //TODO: handle enum
            activity.Type=CourseLearningActivityType.Assignment;
            var response = ApiFeature.ApiTestHost.Client.PostAsJsonAsync(RequestUri.ToString() + LearningActivityUrl, activity).Result;
            if (!response.IsSuccessStatusCode)
            {
                var responseBody = response.Content.ReadAsStringAsync().Result;
                throw new Exception(responseBody);
            }

            var activityResponse = response.Content.ReadAsAsync<CourseLearningActivityResponse>().Result;
            ScenarioContext.Current.Add("LearningActivity", activityResponse);
            ScenarioContext.Current.Add(activityResponse.Name, activityResponse);
        }

        [When(@"I update '(.*)' learning activity with the following info")]
        public void WhenIUpdateLearningActivityWithTheFollowingInfo(string activityName, Table table)
        {
            var original = ScenarioContext.Current.Get<CourseLearningActivityResponse>(activityName);
            var id = original.Id;
            var activity = table.CreateInstance<SaveCourseLearningActivityRequest>();
            activity.Type=CourseLearningActivityType.Assignment;
            var response = ApiFeature.ApiTestHost.Client.PutAsJsonAsync(RequestUri.ToString() + LearningActivityUrl + id, activity).Result;
            if (ScenarioContext.Current.ContainsKey(activityName))
                ScenarioContext.Current.Remove(activityName);
                ScenarioContext.Current.Add(activityName, response.RequestMessage.RequestUri);
        }

        [Then(@"the learning activity '(.*)' should have the following info")]
        public void ThenTheLearningActivityShouldHaveTheFollowingInfo(string activityName, Table table)
        {
            var getUri = ScenarioContext.Current.Get<Uri>(activityName);
            var response = ApiFeature.ApiTestHost.Client.GetAsync(getUri).Result;
            var getResponse = response.Content.ReadAsAsync<CourseLearningActivityResponse>().Result;
            table.CompareToInstance(getResponse);
        }


        [When(@"I add the following learning activity:")]
        public void WhenIAddTheFollowingLearningActivity(Table table)
        {
            var learningActivities = table.CreateSet<SaveCourseLearningActivityRequest>();

            foreach (var activity in learningActivities)
            {
             
                activity.TenantId = ApiFeature.TenantId;
		activity.Type=CourseLearningActivityType.Assignment;
		var response = ApiFeature.ApiTestHost.Client.PostAsJsonAsync(RequestUri.ToString() + LearningActivityUrl, activity).Result;
                if (!response.IsSuccessStatusCode)
                {
                    var responseBody = response.Content.ReadAsStringAsync().Result;
                    throw new Exception(responseBody);
                }
                var saveLearningActivity = response.Content.ReadAsAsync<CourseLearningActivityResponse>().Result;
                ScenarioContext.Current.Add(saveLearningActivity.Name, saveLearningActivity);
            }
        }

        [When(@"I update the learning activity with the following:")]
        public void WhenIUpdateTheLearningActivityWithTheFollowing(Table table)
        {
            var learningActivities = table.CreateSet<SaveCourseLearningActivityRequest>();
            foreach (var activity in learningActivities)
            {
                activity.Type = CourseLearningActivityType.Assignment;
                var activityResponse = ScenarioContext.Current.Get<CourseLearningActivityResponse>(table.Rows[0]["Name"]);
                var response = ApiFeature.ApiTestHost.Client.PutAsJsonAsync(RequestUri.ToString() + LearningActivityUrl + activityResponse.Id, activity).Result;
                if (!response.IsSuccessStatusCode)
                {
                    var responseBody = response.Content.ReadAsStringAsync().Result;
                    throw new Exception(responseBody);
                }
            }
        }

        [When(@"I remove ""(.*)"" learning activity")]
        public void WhenIRemoveLearningActivity(string activityName)
        {
            var activity = ScenarioContext.Current.Get<CourseLearningActivityResponse>(activityName);
            var response = ApiFeature.ApiTestHost.Client.DeleteAsync(RequestUri.ToString() + LearningActivityUrl + activity.Id).Result;
            if (!response.IsSuccessStatusCode)
            {
                var responseBody = response.Content.ReadAsStringAsync().Result;
                throw new Exception(responseBody);
            }

            var getResponse = ApiFeature.ApiTestHost.Client.GetAsync(RequestUri + LearningActivityUrl + activity.Id).Result;
            ScenarioContext.Current.Add("ResponseToValidate", getResponse);
        }

        [When(@"I add a learning activity to a course that has already been published")]
        public void WhenIAddALearningActivityToACourseThatHasAlreadyBeenPublished(Table table)
        {
            var learningActivity = table.CreateInstance<SaveCourseLearningActivityRequest>();
            learningActivity.Type=CourseLearningActivityType.Assignment;
            var response = ApiFeature.ApiTestHost.Client.PostAsJsonAsync(RequestUri.ToString() + LearningActivityUrl, learningActivity).Result;            
            ScenarioContext.Current.Add("ResponseToValidate", response);
            Whens.ResponseMessages.Add(response);
        }

        [Then(@"my learning activity contains the following:")]
        public void ThenMyLearningActivityContainsTheFollowing(Table table)
        {
            foreach (var row in table.Rows)
            {
                var activities = ScenarioContext.Current.Get<CourseLearningActivityResponse>(row["Name"]);
                var getResponse = ApiFeature.ApiTestHost.Client.GetAsync(RequestUri + LearningActivityUrl + activities.Id).Result;
                var response = getResponse.Content.ReadAsAsync<CourseLearningActivityResponse>().Result;
                Assert.That(response.Name, Is.EqualTo(activities.Name));
                Assert.That(response.Type, Is.EqualTo(activities.Type));
                Assert.That(response.IsGradeable, Is.EqualTo(activities.IsGradeable));
                Assert.That(response.IsExtraCredit, Is.EqualTo(activities.IsExtraCredit));
                Assert.That(response.MaxPoint, Is.EqualTo(activities.MaxPoint));
                Assert.That(response.ObjectId, Is.EqualTo(activities.ObjectId));
            }
        }
    }
}
