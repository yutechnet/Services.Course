﻿using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
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
                    Description = table.Rows[0]["Description"],
                    TenantId = 1
                };

            ScenarioContext.Current.Add("createCourseRequest", saveCourseRequest);
            ScenarioContext.Current.Add("courseName", table.Rows[0]["Name"]);
            ScenarioContext.Current.Add("courseCode", table.Rows[0]["Code"]);
        }
        
        [When(@"I submit a creation request")]
        public void WhenISubmitACreationRequest()
        {
            var saveCourseRequest = ScenarioContext.Current.Get<SaveCourseRequest>("createCourseRequest");
            var response = ApiFeature.ApiTestHost.Client.PostAsync("/courses", saveCourseRequest, new JsonMediaTypeFormatter()).Result;

            if (ScenarioContext.Current.ContainsKey("createCourseResponse"))
            {
                ScenarioContext.Current.Remove("createCourseResponse");
            }
            ScenarioContext.Current.Add("createCourseResponse", response);

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
                Description = table.Rows[0]["Description"],
                TenantId = 1
            };

            ScenarioContext.Current.Add("editCourseRequest", editCourseRequest);
            var response = ScenarioContext.Current.Get<HttpResponseMessage>("createCourseResponse");
            var courseInfoResponse = response.Content.ReadAsAsync<CourseInfoResponse>().Result;

            var result = ApiFeature.ApiTestHost.Client.PutAsync("/courses/" + courseInfoResponse.Id, editCourseRequest, new JsonMediaTypeFormatter()).Result;
            ScenarioContext.Current.Add("editCourseResponse", result);
            ScenarioContext.Current.Add("courseId", courseInfoResponse.Id);

            // this is the response to ensure the success code
            if (ScenarioContext.Current.ContainsKey("responseToValidate"))
            {
                ScenarioContext.Current.Remove("responseToValidate");
            }
            ScenarioContext.Current.Add("responseToValidate", result);
        }

        [When(@"I request a course name that does not exist")]
        public void WhenIRequestACourseNameThatDoesNotExist()
        {
            var courseName = "someCoureName";
            var result = ApiFeature.ApiTestHost.Client.GetAsync("/courses?name=" + courseName + ScenarioContext.Current.Get<long>("ticks")).Result;

            ScenarioContext.Current.Add("getCourseName", result);
        }
        
        [Then(@"I should get a success confirmation message")]
        public void ThenIShouldGetASuccessConfirmationMessage()
        {
            var response = ScenarioContext.Current.Get<HttpResponseMessage>("responseToValidate");
            response.EnsureSuccessStatusCode();
        }

        [Then(@"I can retrieve the course by course name")]
        public void ThenICanRetrieveTheCourseByCourseName()
        {
            var courseName = ScenarioContext.Current.Get<string>("courseName");
            var result = ApiFeature.ApiTestHost.Client.GetAsync("/courses?name=" + courseName + ScenarioContext.Current.Get<long>("ticks")).Result;
            result.EnsureSuccessStatusCode();
        }

        [Then(@"I can retrieve the course by course code")]
        public void ThenICanRetrieveTheCourseByCourseCode()
        {
            var courseCode = ScenarioContext.Current.Get<string>("courseCode");
            var result = ApiFeature.ApiTestHost.Client.GetAsync("/courses?code=" + courseCode + ScenarioContext.Current.Get<long>("ticks")).Result;
            result.EnsureSuccessStatusCode();
        }


        [Then(@"my course info is changed")]
        public void ThenMyCourseInfoIsChanged()
        {
            var courseId = ScenarioContext.Current.Get<Guid>("courseId");
            var response = ApiFeature.ApiTestHost.Client.GetAsync("/courses/" + courseId).Result;
            response.EnsureSuccessStatusCode();

            var courseInfo = response.Content.ReadAsAsync<CourseInfoResponse>().Result;
            var originalRequest = ScenarioContext.Current.Get<SaveCourseRequest>("editCourseRequest");
            Assert.AreEqual(courseInfo.Name, originalRequest.Name);
            Assert.AreEqual(courseInfo.Code, originalRequest.Code);   
        }

        [Then(@"I should get a not found message returned")]
        public void ThenIShouldGetANotFoundMessageReturned()
        {
            var response = ScenarioContext.Current.Get<HttpResponseMessage>("getCourseName");
            var expected = (HttpStatusCode)Enum.Parse(typeof (HttpStatusCode), "NotFound");
            Assert.That(response.StatusCode, Is.EqualTo(expected));
        }

        [Given(@"I have an existing course with following info:")]
        public void GivenIHaveAnExistingCourseWithFollowingInfo(Table table)
        {
            // This is creating a course for us.
            GivenIHaveACourseWithFollowingInfo(table);
            WhenISubmitACreationRequest();
            ThenIShouldGetASuccessConfirmationMessage();
        }

        [Given(@"I delete this course")]
        public void GivenIDeleteThisCourse()
        {
            var response = ScenarioContext.Current.Get<HttpResponseMessage>("createCourseResponse");
            var courseInfoResponse = response.Content.ReadAsAsync<CourseInfoResponse>().Result;
            ScenarioContext.Current.Add("courseId", courseInfoResponse.Id);
            var delSuccess = ApiFeature.ApiTestHost.Client.DeleteAsync("/courses/" + courseInfoResponse.Id).Result;
            delSuccess.EnsureSuccessStatusCode();
        }

        [Then(@"my course no longer exists")]
        public void ThenMyCourseNoLongerExists()
        {
            var courseId = ScenarioContext.Current.Get<Guid>("courseId");
            var getResponse = ApiFeature.ApiTestHost.Client.GetAsync("/courses/" + courseId).Result;
            Assert.That(getResponse.StatusCode.Equals(HttpStatusCode.NotFound));
        }

        [When(@"I create a new course with (.*), (.*), (.*)")]
        public void WhenICreateANewCourseWith(string name, string code, string description)
        {
            var saveCourseRequest = new SaveCourseRequest
            {
                Name = string.IsNullOrEmpty(name) ? name : name + ScenarioContext.Current.Get<long>("ticks") ,
                Code = string.IsNullOrEmpty(code) ? code : code + ScenarioContext.Current.Get<long>("ticks"),
                Description = description,
                TenantId = 1
            };

            if (ScenarioContext.Current.ContainsKey("createCourseRequest"))
            {
                ScenarioContext.Current.Remove("createCourseRequest");
            }
            ScenarioContext.Current.Add("createCourseRequest", saveCourseRequest);
        }

        [Then(@"I should get the status code (.*)")]
        public void ThenIShouldGetA(string status)
        {
            var response = ScenarioContext.Current.Get<HttpResponseMessage>("responseToValidate");

            var expectedStatusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), status);

            Assert.That(response.StatusCode.Equals(expectedStatusCode));
        }
    }
}
