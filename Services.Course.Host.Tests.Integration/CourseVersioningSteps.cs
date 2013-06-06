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
    }
}
