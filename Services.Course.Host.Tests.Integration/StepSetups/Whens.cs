using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Text.RegularExpressions;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Host.Tests.Integration.Operations;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace BpeProducts.Services.Course.Host.Tests.Integration.StepSetups
{
    [Binding]
    public class Whens
    {
        public static IList<HttpResponseMessage> ResponseMessages
        {
            get { return ScenarioContext.Current.Get<IList<HttpResponseMessage>>("Responses"); }
        }

        public Whens()
        {
            ScenarioContext.Current.Add("Responses", new List<HttpResponseMessage>());
        }

        [When(@"I create a course from the template '(.*)' with the following")]
        public void WhenICreateACourseFromTheTemplateWithTheFollowing(string templateName, Table table)
        {
            var template = Givens.Courses[templateName];
            var courseRequest = table.CreateInstance<SaveCourseRequest>();
            courseRequest.TemplateCourseId = template.Id;

            var resourse = PostOperations.CreateCourse(courseRequest);

            Givens.Courses.Add(resourse.Dto.Name, resourse);
        }

        [When(@"I associate the newly created learning outcomes to '(.*)' program")]
        public void WhenIAssociateTheNewlyCreatedLearningOutcomesToProgram(string programName, Table table)
        {
            var requests = (from r in table.Rows
                            select new OutcomeRequest {Description = r["Description"], TenantId = ApiFeature.TenantId})
                .ToList();

            var program = Givens.Programs[programName];

            foreach (var request in requests)
            {
                var resource = PostOperations.CreateEntityLearningOutcome("program", program.Id, request);
                resource.Response.EnsureSuccessStatusCode();
            }
        }

        [When(@"I associate the existing learning outcomes to '(.*)' program")]
        public void WhenIAssociateTheExistingLearningOutcomesToProgram(string programName, Table table)
        {
            var program = Givens.Programs[programName];

            foreach (var row in table.Rows)
            {
                var outcome = Givens.LearningOutcomes[row["Description"]];

                var response = PutOperations.ProgramSupportsLearningOutcome(program, outcome);
                response.EnsureSuccessStatusCode();
            }
        }

        [When(@"I associate the newly created learning outcomes to '(.*)' course")]
        public void WhenIAssociateTheNewlyCreatedLearningOutcomesToCourse(string courseName, Table table)
        {
            var requests = (from r in table.Rows
                            select new OutcomeRequest {Description = r["Description"], TenantId = ApiFeature.TenantId})
                .ToList();

            var course = Givens.Courses[courseName];

            foreach (var request in requests)
            {
                var resource = PostOperations.CreateEntityLearningOutcome("course", course.Id, request);
                resource.Response.EnsureSuccessStatusCode();
            }
        }

        [When(@"I associate the existing learning outcomes to '(.*)' course")]
        public void WhenIAssociateTheExistingLearningOutcomesToCourse(string courseName, Table table)
        {
            var course = Givens.Courses[courseName];

            foreach (var row in table.Rows)
            {
                var outcome = Givens.LearningOutcomes[row["Description"]];

                var response = PutOperations.CourseSupportsLearningOutcome(course, outcome);
                response.EnsureSuccessStatusCode();
            }
        }

        [When(@"I associate '(.*)' course with '(.*)' program")]
        public void WhenIAssociateCourseWithProgram(string courseName, string programName)
        {
            var course = Givens.Courses[courseName];
            var program = Givens.Programs[programName];

            course.Dto = GetOperations.GetCourse(course.ResourseUri);

            var response = PutOperations.AssociateCourseWithPrograms(course, new List<ProgramResource> {program});
            response.EnsureSuccessStatusCode();
        }

        [When(@"I associate '(.*)' course with the following programs")]
        public void WhenIAssociateCourseWithTheFollowingPrograms(string courseName, Table table)
        {
            var course = Givens.Courses[courseName];
            var programs = (from r in table.Rows select Givens.Programs[r["Program Name"]]).ToList();

            course.Dto = GetOperations.GetCourse(course.ResourseUri);

            var response = PutOperations.AssociateCourseWithPrograms(course, programs);
            response.EnsureSuccessStatusCode();
        }

        [When(@"I remove '(.*)' course from '(.*)'")]
        public void WhenIRemoveCourseFrom(string courseName, string programName)
        {
            var course = Givens.Courses[courseName];
            var program = Givens.Programs[programName];

            course.Dto = GetOperations.GetCourse(course.ResourseUri);

            var response = PutOperations.DisassociateCourseWithPrograms(course, new List<ProgramResource> {program});
            response.EnsureSuccessStatusCode();
        }

        [When(@"I assoicate the following outcomes to outcome '(.*)'")]
        public void WhenIAssoicateTheFollowingOutcomesToOutcome(string outcomeName, Table table)
        {
            var supportedOutcome = Givens.LearningOutcomes[outcomeName];

            var descriptions = (from o in table.Rows select o["Description"]).ToList();
            var supportingOutcomes = (from o in Givens.LearningOutcomes
                                      where descriptions.Contains(o.Value.SaveRequest.Description)
                                      select o).ToList();

            foreach (var supportingOutcome in supportingOutcomes)
            {
                var response = PutOperations.OutcomeSupportsLearningOutcome(supportingOutcome.Value, supportedOutcome);
                response.EnsureSuccessStatusCode();
            }
        }

        [When(@"I publish the following courses")]
        public void WhenIPublishTheFollowingCourses(Table table)
        {
            foreach (var row in table.Rows)
            {
                var course = Givens.Courses[row["Name"]];

                var request = new PublishRequest
                    {
                        PublishNote = row["Note"]
                    };

                var response = PutOperations.PublishCourse(course, request);
                response.EnsureSuccessStatusCode();
            }
        }

        [When(@"I add the following prerequisites to '(.*)'")]
        public void WhenIAddTheFollowingPrerequisitesTo(string courseName, Table table)
        {
            var course = Givens.Courses[courseName];

            var prerequisiteIds = (from r in table.Rows select Givens.Courses[r["Name"]].Id).ToList();

            var request = new UpdateCoursePrerequisites
                {
                    PrerequisiteIds = prerequisiteIds
                };

            var response = PutOperations.SetCoursePrerequisites(course, request);
            ResponseMessages.Add(response);
        }

        //TODO: Refactor
        [When(@"I remove ""(.*)"" learning activity")]
        public void WhenIRemoveLearningActivity(string activityName)
        {
            const string learningActivityUrl = "/learningactivity/";
            var requestUri = ScenarioContext.Current.Get<Uri>("Week1");

            var activity = ScenarioContext.Current.Get<CourseLearningActivityResponse>(activityName);
            var response =
                ApiFeature.ApiTestHost.Client.DeleteAsync(requestUri.ToString() + learningActivityUrl + activity.Id)
                          .Result;
            if (!response.IsSuccessStatusCode)
            {
                var responseBody = response.Content.ReadAsStringAsync().Result;
                throw new Exception(responseBody);
            }

            var getResponse =
                ApiFeature.ApiTestHost.Client.GetAsync(requestUri + learningActivityUrl + activity.Id).Result;
            ScenarioContext.Current.Add("ResponseToValidate", getResponse);
            ResponseMessages.Add(getResponse);
        }

        //TODO: Refactor
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
                var resourceUri = Givens.Courses[courseName].ResourseUri;
                versionRequest.ParentVersionId = Guid.Parse(resourceUri.Segments[resourceUri.Segments.Length - 1]);
            }

            var postUri = string.Format("{0}/version", FeatureContext.Current.Get<string>("CourseLeadingPath"));

            var response =
                ApiFeature.ApiTestHost.Client.PostAsync(postUri, versionRequest, new JsonMediaTypeFormatter()).Result;

            ResponseMessages.Add(response);

            ScenarioContext.Current[courseName] = response.Headers.Location;
            ScenarioContext.Current["ResponseToValidate"] = response;
            ResponseMessages.Add(response);
        }

        //TODO: Refactor
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
            ResponseMessages.Add(response);
        }

        //TODO: Refactor
        [When(@"I retrieve '(.*)' course")]
        public void WhenIRetrieveCourse(string courseName)
        {
            var getUri = Givens.Courses[courseName].ResourseUri;
            var response = ApiFeature.ApiTestHost.Client.GetAsync(getUri).Result;
            response.EnsureSuccessStatusCode();

            ScenarioContext.Current.Add("CourseInfoToValidate",
                                        response.Content.ReadAsAsync<CourseInfoResponse>().Result);
            ResponseMessages.Add(response);
        }

        //TODO: Refactor
        [When(@"I update '(.*)' course with the following info")]
        public void WhenIUpdateCourseWithTheFollowingInfo(string courseName, Table table)
        {
            var putUri = Givens.Courses[courseName].ResourseUri;
            var saveCourseRequest = table.CreateInstance<SaveCourseRequest>();
            saveCourseRequest.TenantId = ApiFeature.TenantId;

            var response = ApiFeature.ApiTestHost.Client.PutAsync(putUri.ToString(), saveCourseRequest,
                                                                  new JsonMediaTypeFormatter()).Result;

            ScenarioContext.Current.Add("ResponseToValidate", response);
        }

        //TODO: Refactor
        [When(@"I delete '(.*)' course")]
        public void WhenIDeleteCourse(string courseName)
        {
            var resourceUri = Givens.Courses[courseName].ResourseUri;
            var response = ApiFeature.ApiTestHost.Client.DeleteAsync(resourceUri).Result;

            ScenarioContext.Current.Add("ResponseToValidate", response);
            ResponseMessages.Add(response);
        }

        [When(@"I create a learning outcome with the description '(.*)'")]
        [Given(@"I have a learning outcome with the description '(.*)'")]
        public void WhenICreateALearningOutcomeWithTheDescription(string description)
        {
            var outcomeRequest = new OutcomeRequest {Description = description, TenantId = ApiFeature.TenantId};
            var postUrl = FeatureContext.Current.Get<String>("OutcomeLeadingPath");
            var response =
                ApiFeature.ApiTestHost.Client.PostAsync(postUrl, outcomeRequest, new JsonMediaTypeFormatter()).Result;
            response.EnsureSuccessStatusCode();

            ScenarioContext.Current.Add("learningoutcomeResourceUrl", response.Headers.Location);
        }

        [When(@"I change the description to '(.*)'")]
        public void WhenIChangeTheDescriptionTo(string description)
        {
            var outcomeRequest = new OutcomeRequest {Description = description, TenantId = ApiFeature.TenantId};
            var putUrl = ScenarioContext.Current.Get<Uri>("learningoutcomeResourceUrl");
            var response =
                ApiFeature.ApiTestHost.Client.PutAsync(putUrl.ToString(), outcomeRequest, new JsonMediaTypeFormatter())
                          .Result;
            response.EnsureSuccessStatusCode();
        }

        [When(@"I delete this learning outcome")]
        public void WhenIDeleteThisLearningOutcome()
        {
            var deleteUrl = ScenarioContext.Current.Get<Uri>("learningoutcomeResourceUrl");
            var response = ApiFeature.ApiTestHost.Client.DeleteAsync(deleteUrl).Result;
            response.EnsureSuccessStatusCode();
        }

        [Then(@"the learning outcome should be with the description '(.*)'")]
        public void ThenTheLearningOutcomeShouldBeWithTheDescription(string description)
        {
            var getUrl = ScenarioContext.Current.Get<Uri>("learningoutcomeResourceUrl");
            var response = ApiFeature.ApiTestHost.Client.GetAsync(getUrl.ToString()).Result;
            response.EnsureSuccessStatusCode();

            var outcomeResponse = response.Content.ReadAsAsync<OutcomeInfo>().Result;

            Assert.That(outcomeResponse.Description, Is.EqualTo(description));
        }

        [Then(@"the learning outcome shoud no longer exist")]
        public void ThenTheLearningOutcomeShoudNoLongerExist()
        {
            var getUrl = ScenarioContext.Current.Get<Uri>("learningoutcomeResourceUrl");
            var response = ApiFeature.ApiTestHost.Client.GetAsync(getUrl.ToString()).Result;
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [When(@"I disassociate the following learning outcomes from '(.*)' program:")]
        public void WhenIDisassociateTheFollowingLearningOutcomesFromProgram(string programName, Table table)
        {
            var outcomeUrls = ScenarioContext.Current.Get<Dictionary<string, Uri>>("learningoutcomeResourceUrls");
            foreach (var item in table.Rows)
            {
                var deleteUrl = outcomeUrls[item["Description"]];
                var response = ApiFeature.ApiTestHost.Client.DeleteAsync(deleteUrl.ToString()).Result;
                ScenarioContext.Current.Add("Delete" + item["Description"], response);
                response.EnsureSuccessStatusCode();
            }
        }

        [Then(@"'(.*)' program is associated with the only following learning outcomes:")]
        public void ThenProgramIsAssociatedWithTheOnlyFollowingLearningOutcomes(string programName, Table table)
        {
            //get the program and its outcome and assert on count and desc
            // PUT /program/programid/outcome
            ProgramResponse program = null;

            var programs = ScenarioContext.Current.Get<IList<ProgramResponse>>("programs");
            program = programs.First(p => p.Name.Equals(programName));


            var getUrl = string.Format("{0}/{1}/supports",
                                       FeatureContext.Current.Get<String>("ProgramLeadingPath"), program.Id);

            var response = ApiFeature.ApiTestHost.Client.GetAsync(getUrl).Result;
            response.EnsureSuccessStatusCode();
            var outcomes = response.Content.ReadAsAsync<List<OutcomeInfo>>().Result;
            Assert.That(outcomes.Count, Is.EqualTo(table.Rows.Count));
            foreach (var row in table.Rows)
            {
                Assert.That(outcomes.Any(o => o.Description == row["Description"]));
            }
        }

        [Then(@"disassociating the following from '(.*)' program should result:")]
        public void ThenDisassociatingTheFollowingFromProgramShouldResult(string p0, Table table)
        {
            var outcomeUrls = ScenarioContext.Current.Get<Dictionary<string, Uri>>("learningoutcomeResourceUrls");
            String deleteUrl = "";
            foreach (var item in table.Rows)
            {

                if (outcomeUrls.ContainsKey(item["Description"]))
                {
                    deleteUrl = outcomeUrls[item["Description"]].ToString();
                }
                else
                {
                    deleteUrl = deleteUrl.Substring(0, deleteUrl.LastIndexOf("/") + 1) + Guid.NewGuid().ToString();
                        //craft a fake
                }
                var response = ApiFeature.ApiTestHost.Client.DeleteAsync(deleteUrl).Result;

                var expectedStatusCode =
                    (HttpStatusCode) Enum.Parse(typeof (HttpStatusCode), item["Disassociation Response"]);

                Assert.That(response.StatusCode.Equals(expectedStatusCode));

            }
        }

        private string ExtractGuid(string str, int i)
        {
            var p = @"([a-z0-9]{8}[-][a-z0-9]{4}[-][a-z0-9]{4}[-][a-z0-9]{4}[-][a-z0-9]{12})";
            var mc = Regex.Matches(str, p);

            return mc[i].ToString();
        }

        [Then(@"'(.*)' has the following learning outcomes:")]
        public void ThenHasTheFollowingLearningOutcomes(string outcomeDescription, Table table)
        {
            var parentOutcomeResourceUri = ScenarioContext.Current.Get<Uri>(outcomeDescription);
            var parentOutcomeId = ExtractGuid(parentOutcomeResourceUri.ToString(), 1);

            var getUri = string.Format("{0}/{1}/supports", FeatureContext.Current.Get<string>("OutcomeLeadingPath"),
                                       parentOutcomeId);

            var response = ApiFeature.ApiTestHost.Client.GetAsync(getUri).Result;
            response.EnsureSuccessStatusCode();

            var childOutcomes = response.Content.ReadAsAsync<IList<OutcomeInfo>>().Result;
            table.CompareToSet(childOutcomes);
        }

        [When(@"I disassociate '(.*)' from '(.*)'")]
        public void WhenIDisassociateFrom(string childOutcome, string parentOutcome)
        {
            var parentOutcomeResourceUri = ScenarioContext.Current.Get<Uri>(parentOutcome);
            var parentOutcomeId = ExtractGuid(parentOutcomeResourceUri.ToString(), 1);
            var childOutcomeResourceUri = ScenarioContext.Current.Get<Uri>(childOutcome);
            var childOutcomeId = ExtractGuid(childOutcomeResourceUri.ToString(), 1);

            var deleteUri = string.Format("{0}/{1}/supports/{2}",
                                          FeatureContext.Current.Get<string>("OutcomeLeadingPath"), childOutcomeId,
                                          parentOutcomeId);

            var response = ApiFeature.ApiTestHost.Client.DeleteAsync(deleteUri).Result;
            response.EnsureSuccessStatusCode();
        }

        [Then(@"the course '(.*)' includes the following learning outcomes:")]
        public void ThenTheCourseIncludesTheFollowingLearningOutcomes(string courseName, Table table)
        {
            var course = Givens.Courses[courseName];
            var response = ApiFeature.ApiTestHost.Client.GetAsync(course.ResourseUri + "/" + "supports").Result;
            var outcomes = response.Content.ReadAsAsync<List<OutcomeInfo>>().Result;
            table.CompareToSet(outcomes);
        }
    }
}
