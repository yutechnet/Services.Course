using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using BpeProducts.Services.Course.Contract;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace BpeProducts.Services.Course.Host.Tests.Integration
{
    [Binding]
    public class CourseVersioningSteps
    {
        [Given(@"I create the following course")]
        public void GivenICreateTheFollowingCourse(Table table)
        {
            var postUri = FeatureContext.Current.Get<string>("CourseLeadingPath");
            var saveCourseRequest = table.CreateInstance<SaveCourseRequest>();

            var response =
                ApiFeature.ApiTestHost.Client.PostAsync(postUri, saveCourseRequest, new JsonMediaTypeFormatter()).Result;
            response.EnsureSuccessStatusCode();

            ScenarioContext.Current.Add(saveCourseRequest.Code, response.Headers.Location);
        }
        
        [When(@"I retrieve '(.*)' course")]
        public void WhenIRetrieveCourse(string courseCode)
        {
            var getUri = ScenarioContext.Current.Get<Uri>(courseCode);
            var response = ApiFeature.ApiTestHost.Client.GetAsync(getUri).Result;
            response.EnsureSuccessStatusCode();

            ScenarioContext.Current.Add("CourseInfoToValidate", response.Content.ReadAsAsync<CourseInfoResponse>().Result);
        }
        
        [Then(@"the course should have the following info")]
        public void ThenTheCourseShouldHaveTheFollowingInfo(Table table)
        {
            var courseInfoResponse = ScenarioContext.Current.Get<CourseInfoResponse>("CourseInfoToValidate");
            table.CompareToInstance(courseInfoResponse);
        }

        [When(@"I update '(.*)' course with the following info")]
        public void WhenIUpdateCourseWithTheFollowingInfo(string courseCode, Table table)
        {
            var putUri = ScenarioContext.Current.Get<Uri>(courseCode);
            var saveCourseRequest = table.CreateInstance<SaveCourseRequest>();
            var response =
                ApiFeature.ApiTestHost.Client.PutAsync(putUri.ToString(), saveCourseRequest,
                                                       new JsonMediaTypeFormatter()).Result;
            
            ScenarioContext.Current.Add("ResponseToValidate", response);
        }

        [Then(@"the course '(.*)' should have the following info")]
        public void ThenTheCourseShouldHaveTheFollowingInfo(string courseCode, Table table)
        {
            var getUri = ScenarioContext.Current.Get<Uri>(courseCode);
            var response = ApiFeature.ApiTestHost.Client.GetAsync(getUri.ToString()).Result;
            response.EnsureSuccessStatusCode();

            var courseInfoResponse = response.Content.ReadAsAsync<CourseInfoResponse>().Result;
            table.CompareToInstance(courseInfoResponse);
        }

        [When(@"I publish '(.*)' course with the following info")]
        [Given(@"I publish '(.*)' course with the following info")]
        public void WhenIPublishCourseWithTheFollowingInfo(string courseCode, Table table)
        {
            var resourceUri = ScenarioContext.Current.Get<Uri>(courseCode);
            var publishUri = string.Format("{0}/publish", resourceUri);
            var publishRequest = table.CreateInstance<CoursePublishRequest>();

            var response =
                ApiFeature.ApiTestHost.Client.PutAsync(publishUri, publishRequest, new JsonMediaTypeFormatter()).Result;
            response.EnsureSuccessStatusCode();
        }

        [Then(@"I get '(.*)' response")]
        public void ThenIGetResponse(string status)
        {
            var response = ScenarioContext.Current.Get<HttpResponseMessage>("ResponseToValidate");

            var expectedStatusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), status);

            Assert.That(response.StatusCode.Equals(expectedStatusCode));
        }

        [When(@"I delete '(.*)' course")]
        public void WhenIDeleteCourse(string courseCode)
        {
            var resourceUri = ScenarioContext.Current.Get<Uri>(courseCode);
            var response = ApiFeature.ApiTestHost.Client.DeleteAsync(resourceUri).Result;

            ScenarioContext.Current.Add("ResponseToValidate", response);
        }

    }
}
