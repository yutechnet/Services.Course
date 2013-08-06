using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Courses;
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

        [When(@"I add the following learning activity:")]
        public void WhenIAddTheFollowingLearningActivity(Table table)
        {
            var row = table.Rows[0];
            var learningActivity = new CourseLearningActivity
                {
                    TenantId = int.Parse(row["TenantId"]),
                    Name = row["Name"],
                    Type = row["Type"],
                    IsGradeable = true,
                    IsExtraCredit = false,
                    Weight = Convert.ToInt32(row["Weight"]),
                    MaxPoint = Convert.ToInt32(row["MaxPoint"])
                };
            
            var request = ApiFeature.ApiTestHost.Client.PostAsJsonAsync(RequestUri.ToString() + LearningActivityUrl, learningActivity).Result;
            request.EnsureSuccessStatusCode();
            var saveLearningActivity = request.Content.ReadAsAsync<CourseLearningActivityResponse>().Result;
            ScenarioContext.Current.Add("LearningActivityRequest", saveLearningActivity);

        }
        
        [Then(@"my learning activity contains the following:")]
        public void ThenMyLearningActivityContainsTheFollowing(Table table)
        {
            var activityResponse = ScenarioContext.Current.Get<CourseLearningActivityResponse>("LearningActivityRequest");
            var response = ApiFeature.ApiTestHost.Client.GetAsync(RequestUri + LearningActivityUrl + activityResponse.Id).Result;
            var getResponse = response.Content.ReadAsAsync<CourseLearningActivityResponse>().Result;

            Assert.That(activityResponse.Name, Is.EqualTo(getResponse.Name));
            Assert.That(activityResponse.Type, Is.EqualTo(getResponse.Type));
            Assert.That(activityResponse.IsGradeable, Is.EqualTo(getResponse.IsGradeable));
            Assert.That(activityResponse.IsExtraCredit, Is.EqualTo(getResponse.IsExtraCredit));
            Assert.That(activityResponse.Weight, Is.EqualTo(getResponse.Weight));
            Assert.That(activityResponse.MaxPoint, Is.EqualTo(getResponse.MaxPoint));
            Assert.That(activityResponse.ObjectId, Is.EqualTo(getResponse.ObjectId));
        }
    }
}
