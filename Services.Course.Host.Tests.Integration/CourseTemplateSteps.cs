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
    public class CourseTemplateSteps
    {

        [Given(@"I have the following course template")]
        public void GivenIHaveTheFollowingCourseTemplate(Table table)
        {
            var postUri = FeatureContext.Current.Get<string>("CourseLeadingPath");
            var saveCourseRequest = table.CreateInstance<SaveCourseRequest>();
            var response = ApiFeature.ApiTestHost.Client.PostAsync(postUri, saveCourseRequest, new JsonMediaTypeFormatter()).Result;
            response.EnsureSuccessStatusCode();

            var courseInfo = response.Content.ReadAsAsync<CourseInfoResponse>().Result;
            var courseId = courseInfo.Id;

            ScenarioContext.Current.Add(saveCourseRequest.Name, response.Headers.Location);
            ScenarioContext.Current.Add("CourseId", courseId );
            ScenarioContext.Current.Add("CourseInfo", courseInfo);
        }

        [When(@"I create a course from the template '(.*)' with the following:")]
        public void WhenICreateACourseFromTheTemplateWithTheFollowing(string courseName, Table table)
        {
            var courseId = ScenarioContext.Current.Get<Guid>("CourseId");
            var resourceUri = ScenarioContext.Current.Get<Uri>(courseName);
            var coureRequest = table.CreateInstance<SaveCourseRequest>();
            coureRequest.TemplateCourseId = courseId;

            var response = ApiFeature.ApiTestHost.Client.PostAsync(resourceUri.ToString(), coureRequest,new JsonMediaTypeFormatter()).Result;

            var courseResponse = response.Content.ReadAsAsync<CourseInfoResponse>().Result;
            ScenarioContext.Current.Add("ResponseToValidate", response);
            ScenarioContext.Current.Add("CourseResponse", courseResponse);
        }

        [When(@"I create a course from the template with the following:")]
        public void WhenICreateACourseFromTheTemplateWithTheFollowing(Table table)
        {
            var postUri = FeatureContext.Current.Get<string>("CourseLeadingPath");
            var courseId = ScenarioContext.Current.Get<Guid>("CourseId");
            var courseInfo = ScenarioContext.Current.Get<CourseInfoResponse>("CourseInfo");
            var coureRequest = table.CreateInstance<SaveCourseRequest>();
            coureRequest.TemplateCourseId = courseId;

            var response = ApiFeature.ApiTestHost.Client.PostAsync(postUri + "/" + courseInfo.Id, coureRequest, new JsonMediaTypeFormatter()).Result;

            var courseResponse = response.Content.ReadAsAsync<CourseInfoResponse>().Result;
            ScenarioContext.Current.Add("ResponseToValidate", response);
            ScenarioContext.Current.Add("CourseResponse", courseResponse);
        }


        [Then(@"the course should have the following info:")]
        public void ThenTheCourseShouldHaveTheFollowingInfo(Table table)
        {
            var courseResponse = ScenarioContext.Current.Get<CourseInfoResponse>("CourseResponse");
            table.CompareToInstance(courseResponse);
        }

        [Then(@"I should get a '(.*)' status")]
        public void ThenIShouldGetAStatus(string status)
        {
            var courseResponse = ScenarioContext.Current.Get<HttpResponseMessage>("ResponseToValidate");
            var expectedCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), status);
            Assert.That(courseResponse.StatusCode, Is.EqualTo(expectedCode));
        }

    }
}
