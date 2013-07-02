using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text.RegularExpressions;
using BpeProducts.Services.Course.Contract;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace BpeProducts.Services.Course.Host.Tests.Integration
{
    [Binding]
    public class CourseVersioningSteps
    {
        private const int Tenant = 999999;

        [Given(@"I create the following course")]
        public void GivenICreateTheFollowingCourse(Table table)
        {
            var postUri = FeatureContext.Current.Get<string>("CourseLeadingPath");
            var saveCourseRequest = table.CreateInstance<SaveCourseRequest>();
            saveCourseRequest.TenantId = Tenant;

            var response =
                ApiFeature.ApiTestHost.Client.PostAsync(postUri, saveCourseRequest, new JsonMediaTypeFormatter()).Result;
            response.EnsureSuccessStatusCode();

            ScenarioContext.Current.Add(saveCourseRequest.Name, response.Headers.Location);
        }
        
        [When(@"I retrieve '(.*)' course")]
        public void WhenIRetrieveCourse(string courseName)
        {
            var getUri = ScenarioContext.Current.Get<Uri>(courseName);
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
        public void WhenIUpdateCourseWithTheFollowingInfo(string courseName, Table table)
        {
            var putUri = ScenarioContext.Current.Get<Uri>(courseName);
            var saveCourseRequest = table.CreateInstance<SaveCourseRequest>();
            saveCourseRequest.TenantId = Tenant;

            var response = ApiFeature.ApiTestHost.Client.PutAsync(putUri.ToString(), saveCourseRequest,
                                                       new JsonMediaTypeFormatter()).Result;
            
            ScenarioContext.Current.Add("ResponseToValidate", response);
        }

        [Then(@"the course '(.*)' should have the following info")]
        public void ThenTheCourseShouldHaveTheFollowingInfo(string courseName, Table table)
        {
            var getUri = ScenarioContext.Current.Get<Uri>(courseName);
            var response = ApiFeature.ApiTestHost.Client.GetAsync(getUri.ToString()).Result;
            response.EnsureSuccessStatusCode();

            var courseInfoResponse = response.Content.ReadAsAsync<CourseInfoResponse>().Result;
            table.CompareToInstance(courseInfoResponse);
        }

        [When(@"I publish '(.*)' course with the following info")]
        [Given(@"I publish '(.*)' course with the following info")]
        public void WhenIPublishCourseWithTheFollowingInfo(string courseName, Table table)
        {
            var resourceUri = ScenarioContext.Current.Get<Uri>(courseName);
            var publishUri = string.Format("{0}/publish", resourceUri);
            var publishRequest = table.CreateInstance<PublishRequest>();

            var response = ApiFeature.ApiTestHost.Client.PutAsync(publishUri, publishRequest, new JsonMediaTypeFormatter()).Result;
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
        public void WhenIDeleteCourse(string courseName)
        {
            var resourceUri = ScenarioContext.Current.Get<Uri>(courseName);
            var response = ApiFeature.ApiTestHost.Client.DeleteAsync(resourceUri).Result;

            ScenarioContext.Current.Add("ResponseToValidate", response);
        }

        [When(@"I create a new version of '(.*)' course with the following info")]
        public void WhenICreateANewVersionOfWithTheFollowingInfo(string courseName, Table table)
        {
            var versionRequest = table.CreateInstance<VersionRequest>();

            if (courseName.Equals("RandomCourse"))
            {
                versionRequest.ParentVersionId = Guid.NewGuid();
            }
            else
            {
                var resourceUri = ScenarioContext.Current.Get<Uri>(courseName);
                versionRequest.ParentVersionId = Guid.Parse(ExtractGuid(resourceUri.ToString(), 0));
            }

            var postUri = string.Format("{0}/version", FeatureContext.Current.Get<string>("CourseLeadingPath"));

            var response =
                ApiFeature.ApiTestHost.Client.PostAsync(postUri, versionRequest, new JsonMediaTypeFormatter()).Result;

            ScenarioContext.Current[courseName] = response.Headers.Location;
            ScenarioContext.Current["ResponseToValidate"] = response;
        }

        [When(@"I create a course without a version")]
        public void WhenICreateACourseWithoutAVersion()
        {
            var versionRequest = new VersionRequest
                {
                    ParentVersionId = Guid.NewGuid(),
                    VersionNumber = null
                };
            var postUri = string.Format("{0}/version", FeatureContext.Current.Get<string>("CourseLeadingPath"));

            var response =
                ApiFeature.ApiTestHost.Client.PostAsync(postUri, versionRequest, new JsonMediaTypeFormatter()).Result;

            ScenarioContext.Current["ResponseToValidate"] = response;
        }


        private string ExtractGuid(string str, int i)
        {
            var p = @"([a-z0-9]{8}[-][a-z0-9]{4}[-][a-z0-9]{4}[-][a-z0-9]{4}[-][a-z0-9]{12})";
            var mc = Regex.Matches(str, p);

            return mc[i].ToString();
        }

    }
}
