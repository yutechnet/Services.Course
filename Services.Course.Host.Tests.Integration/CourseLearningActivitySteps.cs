using System;
using System.Collections.Generic;
using System.Linq;
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
        
        public Guid NullGuid = Guid.Parse("00000000-0000-0000-0000-000000000000");

        [When(@"I add the following learning activity:")]
        public void WhenIAddTheFollowingLearningActivity(Table table)
        {
            foreach (var row in table.Rows)
            {
                var learningActivity = new SaveCourseLearningActivityRequest()
                {
                    TenantId = int.Parse(row["TenantId"]),
                    Name = row["Name"],
                    Type = row["Type"],
                    IsGradeable = bool.Parse(row["IsGradeable"]),
                    IsExtraCredit = bool.Parse(row["IsExtraCredit"]),
                    Weight = int.Parse(row["Weight"]),
                    MaxPoint = int.Parse(row["MaxPoint"]),
                    ObjectId = Guid.Parse(row["ObjectId"]),
                };

                var response = ApiFeature.ApiTestHost.Client.PostAsJsonAsync(RequestUri.ToString() + LearningActivityUrl, learningActivity).Result;
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
            foreach (var row in table.Rows)
            {
                var learningActivity = new SaveCourseLearningActivityRequest()
                    {
                        TenantId = int.Parse(row["TenantId"]),
                        Name = row["Name"],
                        Type = row["Type"],
                        IsGradeable = bool.Parse(row["IsGradeable"]),
                        IsExtraCredit = bool.Parse(row["IsExtraCredit"]),
                        Weight = int.Parse(row["Weight"]),
                        MaxPoint = int.Parse(row["MaxPoint"]),
                        ObjectId = Guid.Parse(row["ObjectId"]),
                    };

                var activity = ScenarioContext.Current.Get<CourseLearningActivityResponse>(table.Rows[0]["Name"]);
                var response = ApiFeature.ApiTestHost.Client.PutAsJsonAsync(RequestUri.ToString() + LearningActivityUrl + activity.Id, learningActivity).Result;
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
            ScenarioContext.Current.Add("ResponseToValidate", response);
        }

        [When(@"I add a learning activity to a course that has already been published")]
        public void WhenIAddALearningActivityToACourseThatHasAlreadyBeenPublished(Table table)
        {
            var learningActivity = table.CreateInstance<SaveCourseLearningActivityRequest>();
            var response = ApiFeature.ApiTestHost.Client.PostAsJsonAsync(RequestUri.ToString() + LearningActivityUrl, learningActivity).Result;            
            ScenarioContext.Current.Add("ResponseToValidate", response);
        }


        [Then(@"the learning activity below no longer exists:")]
        public void ThenTheLearningActivityBelowNoLongerExists(Table table)
        {
            var activityResponse = ScenarioContext.Current.Get<CourseLearningActivityResponse>(table.Rows[0]["Name"]);
            var response = ApiFeature.ApiTestHost.Client.GetAsync(RequestUri + LearningActivityUrl + activityResponse.Id).Result;
            var getResponse = response.Content.ReadAsAsync<CourseLearningActivityResponse>().Result;
            Assert.That(getResponse.Name, Is.Null);
        }

        [Then(@"my learning activity contains the following:")]
        public void ThenMyLearningActivityContainsTheFollowing(Table table)
        {
            var activityResponse = ScenarioContext.Current.Get<CourseLearningActivityResponse>(table.Rows[0]["Name"]);
            var response = ApiFeature.ApiTestHost.Client.GetAsync(RequestUri + LearningActivityUrl + activityResponse.Id).Result;
            var getResponse = response.Content.ReadAsAsync<CourseLearningActivityResponse>().Result;
            var newObjId = getResponse.ObjectId.ToString().ToUpper();

            foreach (var row in table.Rows)
            {
                Assert.That(getResponse.Name, Is.EqualTo(row["Name"]));
                Assert.That(getResponse.Type, Is.EqualTo(row["Type"]));
                Assert.That(getResponse.IsGradeable, Is.EqualTo(bool.Parse(row["IsGradeable"])));
                Assert.That(getResponse.IsExtraCredit, Is.EqualTo(bool.Parse(row["IsExtraCredit"])));
                Assert.That(getResponse.Weight, Is.EqualTo(int.Parse(row["Weight"])));
                Assert.That(getResponse.MaxPoint, Is.EqualTo(int.Parse(row["MaxPoint"])));
                Assert.That(newObjId, Is.EqualTo(row["ObjectId"]));
            }
        }
    }
}
