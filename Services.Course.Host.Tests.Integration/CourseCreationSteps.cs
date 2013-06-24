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
    public class CourseCreationSteps
    {
        private readonly string _leadingPath;

        public CourseCreationSteps()
        {
            var targetUri = new Uri(ConfigurationManager.AppSettings["TestHostBaseAddress"]);
            if (!targetUri.Host.Equals("localhost"))
            {
                _leadingPath = targetUri.PathAndQuery + "/courses";
            }
            else
            {
                _leadingPath = "/courses";
            }
        }

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
                Name = ScenarioContext.Current.Get<long>("ticks") + table.Rows[0]["Name"],
                Code = ScenarioContext.Current.Get<long>("ticks") + table.Rows[0]["Code"],
                Description = table.Rows[0]["Description"],
                TenantId = 1,
                OrganizationId = new Guid(table.Rows[0]["OrganizationId"])
                
            };

            ScenarioContext.Current.Add("createCourseRequest", saveCourseRequest);
            ScenarioContext.Current.Add("courseName", table.Rows[0]["Name"]);
            ScenarioContext.Current.Add("courseCode", table.Rows[0]["Code"]);
            ScenarioContext.Current.Add("orgId", saveCourseRequest.OrganizationId);
        }

        [When(@"I submit a creation request")]
        public void WhenISubmitACreationRequest()
        {
            var saveCourseRequest = ScenarioContext.Current.Get<SaveCourseRequest>("createCourseRequest");
            var response = ApiFeature.ApiTestHost.Client.PostAsync(_leadingPath, saveCourseRequest, new JsonMediaTypeFormatter()).Result;

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
                Name = ScenarioContext.Current.Get<long>("ticks") + table.Rows[0]["Name"],
                Code = ScenarioContext.Current.Get<long>("ticks") + table.Rows[0]["Code"],
                Description = table.Rows[0]["Description"],
                TenantId = 1,
                OrganizationId = new Guid(table.Rows[0]["OrganizationId"])
            };

            ScenarioContext.Current.Add("editCourseRequest", editCourseRequest);
            var response = ScenarioContext.Current.Get<HttpResponseMessage>("createCourseResponse");
            var courseInfoResponse = response.Content.ReadAsAsync<CourseInfoResponse>().Result;

            var result = ApiFeature.ApiTestHost.Client.PutAsync(_leadingPath + "/" + courseInfoResponse.Id, editCourseRequest, new JsonMediaTypeFormatter()).Result;
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
            var result = ApiFeature.ApiTestHost.Client.GetAsync(_leadingPath + "?name=" + ScenarioContext.Current.Get<long>("ticks") + courseName).Result;

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
            var result = ApiFeature.ApiTestHost.Client.GetAsync(_leadingPath + "?name=" + ScenarioContext.Current.Get<long>("ticks") + courseName).Result;
            result.EnsureSuccessStatusCode();
        }

        [Then(@"I can retrieve the course by course code")]
        public void ThenICanRetrieveTheCourseByCourseCode()
        {
            var courseCode = ScenarioContext.Current.Get<string>("courseCode");
            var result = ApiFeature.ApiTestHost.Client.GetAsync(_leadingPath + "?code=" + ScenarioContext.Current.Get<long>("ticks") + courseCode).Result;
            result.EnsureSuccessStatusCode();
        }


        [Then(@"my course info is changed")]
        public void ThenMyCourseInfoIsChanged()
        {
            var courseId = ScenarioContext.Current.Get<Guid>("courseId");
            var response = ApiFeature.ApiTestHost.Client.GetAsync(_leadingPath + "/" + courseId).Result;
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
            var expected = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), "NotFound");
            Assert.That(response.StatusCode, Is.EqualTo(expected));
        }

        [Then(@"the organization id is returned as part of the request")]
        public void ThenTheOrganizationIdIsReturnedAsPartOfTheRequest()
        {
            var orgId = ScenarioContext.Current.Get<Guid>("orgId");
            var response = ScenarioContext.Current.Get<HttpResponseMessage>("createCourseResponse");
            var courseInfoResponse = response.Content.ReadAsAsync<CourseInfoResponse>().Result;

            Assert.That(courseInfoResponse.OrganizationId, Is.Not.Null);
            Assert.That(courseInfoResponse.OrganizationId, Is.EqualTo(orgId));
        }

        [Given(@"I have an existing course with following info:")]
        public void GivenIHaveAnExistingCourseWithFollowingInfo(Table table)
        {
            // This is creating a course for us.
            GivenIHaveACourseWithFollowingInfo(table);
            WhenISubmitACreationRequest();
            ThenIShouldGetASuccessConfirmationMessage();
        }

        [Given(@"I have existing courses with following info:")]
        public void GivenIHaveExistingCoursesWithFollowingInfo(Table table)
        {
            // This is creating a course for us.
            foreach (var row in table.Rows)
            {
                var saveCourseRequest = new SaveCourseRequest
                {
                    Name = ScenarioContext.Current.Get<long>("ticks") + row["Name"],
                    Code = ScenarioContext.Current.Get<long>("ticks") + row["Code"],
                    Description = row["Description"],
                    TenantId = 1,
                    OrganizationId = new Guid(table.Rows[0]["OrganizationId"])
                };

                var response = ApiFeature.ApiTestHost.Client.PostAsync(_leadingPath, saveCourseRequest, new JsonMediaTypeFormatter()).Result;
                response.EnsureSuccessStatusCode();

            }
        }

        [Given(@"I delete this course")]
        public void GivenIDeleteThisCourse()
        {
            var response = ScenarioContext.Current.Get<HttpResponseMessage>("createCourseResponse");
            var courseInfoResponse = response.Content.ReadAsAsync<CourseInfoResponse>().Result;
            ScenarioContext.Current.Add("courseId", courseInfoResponse.Id);
            var delSuccess = ApiFeature.ApiTestHost.Client.DeleteAsync(_leadingPath + "/" + courseInfoResponse.Id).Result;
            delSuccess.EnsureSuccessStatusCode();
        }

        [Then(@"my course no longer exists")]
        public void ThenMyCourseNoLongerExists()
        {
            var courseId = ScenarioContext.Current.Get<Guid>("courseId");
            var getResponse = ApiFeature.ApiTestHost.Client.GetAsync(_leadingPath + "/" + courseId).Result;
            Assert.That(getResponse.StatusCode.Equals(HttpStatusCode.NotFound));
        }

        [When(@"I create a new course with (.*), (.*), (.*)")]
        public void WhenICreateANewCourseWith(string name, string code, string description)
        {
            var saveCourseRequest = new SaveCourseRequest
            {
                Name = string.IsNullOrEmpty(name) ? name : ScenarioContext.Current.Get<long>("ticks") + name,
                Code = string.IsNullOrEmpty(code) ? code : ScenarioContext.Current.Get<long>("ticks") + code,
                Description = description,
                TenantId = 1,
                OrganizationId = Guid.NewGuid()
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

        [Then(@"the course name counts are as follows:")]
        public void ThenTheCourseNameCountsAreAsFollows(Table table)
        {
            foreach (var row in table.Rows)
            {
                var operation = row["Operation"];
                var argument = row["Argument"];
                var count = int.Parse(row["Count"]);
                var result = ApiFeature.ApiTestHost.Client.GetAsync(_leadingPath + ConstructODataQueryString(operation, argument)).Result;//+ ScenarioContext.Current.Get<long>("ticks")).Result;
                var getResponse = result.Content.ReadAsAsync<IEnumerable<CourseInfoResponse>>().Result;
                var responseList = new List<CourseInfoResponse>(getResponse);
                Assert.That(responseList.Count, Is.EqualTo(count));
            }
        }

        private string ConstructODataQueryString(string operation, string argument)
        {
            string queryString;

            if (operation.ToLower() == "startswith")
            {
                queryString = String.Format("?$filter={1}(Name, '{0}')",
                                                    String.IsNullOrWhiteSpace(argument)
                                                        ? ""
                                                        : ScenarioContext.Current.Get<long>("ticks") + argument,
                                                    operation);
            }
            else if (operation.ToLower() == "eq")
            {
                queryString = String.Format("?$filter=Name eq '{0}'",
                                                    String.IsNullOrWhiteSpace(argument)
                                                        ? ""
                                                        : ScenarioContext.Current.Get<long>("ticks") + argument);
            }
            else
            {
                throw new InvalidOperationException(string.Format("Unknown operation: {0}", operation));
            }

            return queryString;
        }

        [Then(@"the course count is atleast '(.*)' when search term is '(.*)'")]
        public void ThenTheCourseCountIsAtleastWhenSearchTermIs(int count, string searchPhrase)
        {
            var startsWithQuery = String.IsNullOrWhiteSpace(searchPhrase) ? "" : String.Format("?$filter=startswith(Name, '{0}')", ScenarioContext.Current.Get<long>("ticks") + searchPhrase);
            var result = ApiFeature.ApiTestHost.Client.GetAsync(_leadingPath + startsWithQuery).Result;//+ ScenarioContext.Current.Get<long>("ticks")).Result;
            var getResponse = result.Content.ReadAsAsync<IEnumerable<CourseInfoResponse>>().Result;
            var responseList = new List<CourseInfoResponse>(getResponse);
            Assert.That(responseList.Count, Is.AtLeast(count));
        }
    }
}
