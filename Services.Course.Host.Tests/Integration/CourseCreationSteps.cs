using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using BpeProducts.Common.WebApi.Test;
using BpeProducts.Services.Course.Contract;
using NUnit.Framework;
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

            ScenarioContext.Current.Add("createCourseRequest", saveCourseRequest);
            ScenarioContext.Current.Add("tenantId", table.Rows[0]["Tenant Id"]);
        }
        
        [When(@"I submit a creation request")]
        public void WhenISubmitACreationRequest()
        {
            var saveCourseRequest = ScenarioContext.Current.Get<SaveCourseRequest>("createCourseRequest");
            ApiFeature.ApiTestHost.Client.DefaultRequestHeaders.Add("tenant", ScenarioContext.Current.Get<string>("tenantId"));
            var response = ApiFeature.ApiTestHost.Client.PostAsync("/api/courses", saveCourseRequest, new JsonMediaTypeFormatter()).Result;
            ScenarioContext.Current.Add("createCourseResponse",response);
            // this is the response to ensure the success code
            if (ScenarioContext.Current.ContainsKey("responseToValidate"))
            {
                ScenarioContext.Current.Remove("responseToValidate");
            }
            ScenarioContext.Current.Add("responseToValidate", response);
        }

        [When(@"I change the info to reflect the following:")]
        public void WhenIChangeTheInfoToReflectTheFollowing(Table table)
        {
            var editCourseRequest = new SaveCourseRequest
            {
                Name = table.Rows[0]["Name"] + ScenarioContext.Current.Get<long>("ticks"),
                Code = table.Rows[0]["Code"] + ScenarioContext.Current.Get<long>("ticks"),
                Description = table.Rows[0]["Description"]
            };

            ScenarioContext.Current.Add("editCourseRequest", editCourseRequest);
            var response = ScenarioContext.Current.Get<HttpResponseMessage>("createCourseResponse");
            var courseId = response.Content.ReadAsAsync<string>().Result;

            var result = ApiFeature.ApiTestHost.Client.PutAsync("/api/courses/"+courseId, editCourseRequest, new JsonMediaTypeFormatter()).Result;
            ScenarioContext.Current.Add("editCourseResponse", result);
            ScenarioContext.Current.Add("courseId", courseId);

            // this is the response to ensure the success code
            if (ScenarioContext.Current.ContainsKey("responseToValidate"))
            {
                ScenarioContext.Current.Remove("responseToValidate");
            }
            ScenarioContext.Current.Add("responseToValidate", result);
        }
        
        [Then(@"I should get a success confirmation message")]
        public void ThenIShouldGetASuccessConfirmationMessage()
        {
            var response = ScenarioContext.Current.Get<HttpResponseMessage>("responseToValidate");
            response.EnsureSuccessStatusCode();
        }

        [Then(@"my course info is changed")]
        public void ThenMyCourseInfoIsChanged()
        {
            var courseId = ScenarioContext.Current.Get<string>("courseId");
            var response = ApiFeature.ApiTestHost.Client.GetAsync("api/courses/" + courseId).Result;
            response.EnsureSuccessStatusCode();

            var courseInfo = response.Content.ReadAsAsync<CourseInfoResponse>().Result;
            var originalRequest = ScenarioContext.Current.Get<SaveCourseRequest>("editCourseRequest");
            Assert.AreEqual(courseInfo.Name, originalRequest.Name);
            Assert.AreEqual(courseInfo.Code, originalRequest.Code);   
        }


    }
}
