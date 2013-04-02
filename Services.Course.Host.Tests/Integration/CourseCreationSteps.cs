using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using BpeProducts.Common.WebApi.Test;
using BpeProducts.Services.Course.Contract;
using TechTalk.SpecFlow;

namespace BpeProducts.Services.Course.Host.Tests.Integration
{
    [Binding]
    public class CourseCreationSteps
    {
        [BeforeScenario]
        public void BeforeScenario()
        {
            ScenarioContext.Current.Add("ticks", DateTime.Now.Ticks);
        }

        [Given(@"I have a course with following info:")]
        public void GivenIHaveACourseWithFollowingInfo(Table table)
        {
            var saveCourseRequest = new SaveCourseRequest
                {
                    Name = table.Rows[0]["Name"] + ScenarioContext.Current.Get<long>("ticks"),
                    Code = table.Rows[0]["Code"] + ScenarioContext.Current.Get<long>("ticks"),
                    Description = table.Rows[0]["Description"]
                };

            ScenarioContext.Current.Add("saveCourseRequest", saveCourseRequest);
        }
        
        [When(@"I submit a creation request")]
        public void WhenISubmitACreationRequest()
        {
            var saveCourseRequest = ScenarioContext.Current.Get<SaveCourseRequest>("saveCourseRequest");
            var response = ApiFeature.ApiTestHost.Client.PostAsync("/api/courses", saveCourseRequest, new JsonMediaTypeFormatter()).Result;
            ScenarioContext.Current.Add("saveCourseResponse",response);
        }
        
        [Then(@"I should get a success confirmation message")]
        public void ThenIShouldGetASuccessConfirmationMessage()
        {
            var response = ScenarioContext.Current.Get<HttpResponseMessage>("saveCourseResponse");
            response.EnsureSuccessStatusCode();
        }

    }
}
