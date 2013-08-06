using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using BpeProducts.Services.Course.Contract;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using System.Linq;

namespace BpeProducts.Services.Course.Host.Tests.Integration
{
    [Binding]
    public class CoursePrerequisitesSteps
    {
        private const string Url = "course";

        [Given(@"the following courses and their published status:")]
        public void GivenTheFollowingCoursesAndTheirPublishedStatus(Table table)
        {
            var addedCourses = new Dictionary<string, Guid> { { "courseName", Guid.NewGuid() } };

            foreach (var row in table.Rows)
            {
                var courseRequest = new SaveCourseRequest
                {
                    Name = row["Name"],
                    Code = row["Code"],
                    Description = row["Description"],
                    TenantId = 999999,
                    OrganizationId = new Guid(table.Rows[0]["OrganizationId"]),
                    PrerequisiteCourseIds = new List<Guid>()
                };

                var request = ApiFeature.ApiTestHost.Client.PostAsJsonAsync(Url, courseRequest).Result;
                request.EnsureSuccessStatusCode();
                var courseInfo = request.Content.ReadAsAsync<CourseInfoResponse>().Result;
                var location = request.Headers.Location;

                addedCourses.Add(courseInfo.Name, courseInfo.Id);

                if (row["IsPublished"] == "Published")
                {
                    var publishRequest = new PublishRequest() { PublishNote = "this is now published" };
                    PublishCourse(publishRequest, location);
                }
                
                ScenarioContext.Current.Add(courseRequest.Name, request.Headers.Location);
                ScenarioContext.Current.Add("addedCourseId_" + courseRequest.Name, courseInfo.Id);
            }
        }

        [When(@"I add the following prerequisites to '(.*)'")]
        public void WhenIAddTheFollowingPrerequisitesTo(string courseName, Table table)
        {
            var preRequisiteList = new List<Guid>();
            
            foreach (var row in table.Rows)
            {
                preRequisiteList.Add(ScenarioContext.Current.Get<Guid>("addedCourseId_" + row["Name"]));
            }

            var preReqRequest = new UpdateCoursePrerequisites
            {
                PrerequisiteIds = preRequisiteList
            };

            var putUrl = string.Format("{0}/prerequisites", ScenarioContext.Current.Get<Uri>(courseName));

            var response = ApiFeature.ApiTestHost.Client.PutAsync(putUrl, preReqRequest, new JsonMediaTypeFormatter()).Result;

            if (ScenarioContext.Current.ContainsKey("ResponseToValidate"))
            {
                ScenarioContext.Current.Remove("ResponseToValidate");
            }
            ScenarioContext.Current.Add("ResponseToValidate", response);

            if (ScenarioContext.Current.ContainsKey("CoursePreReqs"))
            {
                ScenarioContext.Current.Remove("CoursePreReqs");
            }
            ScenarioContext.Current.Add("CoursePreReqs", preRequisiteList);
        }

        [Then(@"the course '(.*)' should have the following prerequisites")]
        public void ThenTheCourseShouldHaveTheFollowingPrerequisites(string courseName, Table table)
        {
            var getUri = ScenarioContext.Current.Get<Uri>(courseName);
            var response = ApiFeature.ApiTestHost.Client.GetAsync(getUri.ToString()).Result;
            response.EnsureSuccessStatusCode();
            var getResponse = response.Content.ReadAsAsync<CourseInfoResponse>().Result;

            var coursePreReqs = ScenarioContext.Current.Get<List<Guid>>("CoursePreReqs");
            var realPreReqs = getResponse.PrerequisiteCourseIds;
            foreach (var preReqId in coursePreReqs)
            {
                Assert.That(realPreReqs.Contains(preReqId));
            }
        }

        private void PublishCourse(PublishRequest publishRequest, Uri location)
        {
            if (publishRequest == null) throw new ArgumentNullException("publishRequest");
            var publishUri = string.Format("{0}/publish", location);
            var publishResponse = ApiFeature.ApiTestHost.Client.PutAsJsonAsync(publishUri, publishRequest).Result;
            publishResponse.EnsureSuccessStatusCode(); 
        }
    }
}
