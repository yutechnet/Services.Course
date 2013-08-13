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
                ResponseMessages.Add(response);
            }
        }

        [When(@"I associate the existing learning outcomes to '(.*)' segment")]
        public void WhenIAssociateTheExistingLearningOutcomesToSegment(string segmentName, Table table)
        {
            var segment = Givens.Segments[segmentName];

            foreach (var row in table.Rows)
            {
                var outcome = Givens.LearningOutcomes[row["Description"]];

                var response = PutOperations.SegmentSupportsLearningOutcome(segment, outcome);
                ResponseMessages.Add(response);
            }
        }

        [When(@"I associate '(.*)' course with '(.*)' program")]
        public void WhenIAssociateCourseWithProgram(string courseName, string programName)
        {
            var course = Givens.Courses[courseName];
            var program = Givens.Programs[programName];

            course.Dto = GetOperations.GetCourse(course.ResourseUri);

            var response = PutOperations.AssociateCourseWithPrograms(course, new List<ProgramResource> {program});
            ResponseMessages.Add(response);
        }

        [When(@"I associate '(.*)' course with the following programs")]
        public void WhenIAssociateCourseWithTheFollowingPrograms(string courseName, Table table)
        {
            var course = Givens.Courses[courseName];
            var programs = (from r in table.Rows select Givens.Programs[r["Program Name"]]).ToList();

            course.Dto = GetOperations.GetCourse(course.ResourseUri);

            var response = PutOperations.AssociateCourseWithPrograms(course, programs);
            ResponseMessages.Add(response);
        }

        [When(@"I remove '(.*)' course from '(.*)'")]
        public void WhenIRemoveCourseFrom(string courseName, string programName)
        {
            var course = Givens.Courses[courseName];
            var program = Givens.Programs[programName];

            course.Dto = GetOperations.GetCourse(course.ResourseUri);

            var response = PutOperations.DisassociateCourseWithPrograms(course, new List<ProgramResource> {program});
            ResponseMessages.Add(response);
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
                //TODO: This is an issue... supportingOutcome and supoortedOutcome should be flipped
                var response = PutOperations.OutcomeSupportsLearningOutcome(supportingOutcome.Value, supportedOutcome);
                ResponseMessages.Add(response);
            }
        }

        [When(@"I disassociate the following learning outcomes from '(.*)' learning outcome")]
        public void WhenIDisassociateTheFollowingLearningOutcomesFromLearningOutcome(string learningOutcomeName, Table table)
        {
            var supportedOutcome = Givens.LearningOutcomes[learningOutcomeName];

            var descriptions = (from o in table.Rows select o["Description"]).ToList();
            var supportingOutcomes = (from o in Givens.LearningOutcomes
                                      where descriptions.Contains(o.Value.SaveRequest.Description)
                                      select o).ToList();

            foreach (var supportingOutcome in supportingOutcomes)
            {
                var response = PutOperations.OutcomeDoesNotSupportLearningOutcome(supportingOutcome.Value, supportedOutcome);
                ResponseMessages.Add(response);
            }
        }

        [When(@"I disassociate the following learning outcomes from '(.*)' program")]
        public void WhenIDisassociateTheFollowingLearningOutcomesFromProgram(string programName, Table table)
        {
            var programResource = Givens.Programs[programName];

            var learningOutcomeNames = (from o in table.Rows select o["Description"]).ToList();

            foreach (var learningOutcomeName in learningOutcomeNames)
            {
                var learningOutcome = Givens.LearningOutcomes[learningOutcomeName];
                var response = PutOperations.ProgramDoesNotSupportLearningOutcome(programResource, learningOutcome);
                ResponseMessages.Add(response);
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

        [When(@"I add the following learning activity to '(.*)' course segment")]
        public void WhenIAddTheFollowingLearningActivityToCourseSegment(string segmentName, Table table)
        {
            var segment = Givens.Segments[segmentName];

            var request = table.CreateInstance<SaveCourseLearningActivityRequest>();
            var resource = PostOperations.CreateCourseLearningActivity(segment, request);

            Givens.CourseLearningActivities.Add(request.Name, resource);
            ResponseMessages.Add(resource.Response);
        }

        [When(@"I retrieve the course learning activity '(.*)'")]
        public void WhenIRetrieveTheCourseLearningActivity(string activityName)
        {
            var resource = Givens.CourseLearningActivities[activityName];
            var response = ApiFeature.ApiTestHost.Client.GetAsync(resource.ResourceUri).Result;

            ResponseMessages.Add(response);
        }

        [When(@"I remove ""(.*)"" learning activity")]
        public void WhenIRemoveLearningActivity(string activityName)
        {
            var resource = Givens.CourseLearningActivities[activityName];

            var response = DeleteOperations.CourseLearningActivity(resource);
            ResponseMessages.Add(response);
        }

        [When(@"I update '(.*)' learning activity with the following info")]
        public void WhenIUpdateLearningActivityWithTheFollowingInfo(string activityName, Table table)
        {
            var resourse = Givens.CourseLearningActivities[activityName];
            
            var learningActivity = table.CreateInstance<SaveCourseLearningActivityRequest>();

            var response = PutOperations.UpdateCourseLearningActivity(resourse, learningActivity);
            ResponseMessages.Add(response);
        }

        [When(@"I change the '(.*)' learning outcome description to '(.*)'")]
        public void WhenIChangeTheLearningOutcomeDescriptionTo(string leaningOutcomeName, string newDescription)
        {
            var resource = Givens.LearningOutcomes[leaningOutcomeName];

            var request = new OutcomeRequest
                {
                    Description = newDescription
                };

            var response = PutOperations.UpdateLearningOutcome(resource, request);
            ResponseMessages.Add(response);
        }

        [When(@"I get the learning outcome '(.*)'")]
        public void WhenIGetTheLearningOutcome(string leaningOutcomeName)
        {
            var resource = Givens.LearningOutcomes[leaningOutcomeName];

            var response = ApiFeature.ApiTestHost.Client.GetAsync(resource.ResourceUri.ToString()).Result;
            ResponseMessages.Add(response);
        }

        [When(@"I delete the '(.*)' learning outcome")]
        public void WhenIDeleteTheLearningOutcome(string leaningOutcomeName)
        {
            var resource = Givens.LearningOutcomes[leaningOutcomeName];

            var response = DeleteOperations.LearningOutcome(resource);
            ResponseMessages.Add(response);
        }

        [When(@"I retrieve '(.*)' course")]
        public void WhenIRetrieveCourse(string courseName)
        {
            var getUri = Givens.Courses[courseName].ResourseUri;
            var response = ApiFeature.ApiTestHost.Client.GetAsync(getUri).Result;

            ResponseMessages.Add(response);
        }

        [When(@"I update '(.*)' course with the following info")]
        public void WhenIUpdateCourseWithTheFollowingInfo(string courseName, Table table)
        {
            var course = Givens.Courses[courseName];
            var updateCourseRequest = table.CreateInstance<UpdateCourseRequest>();

            var response = PutOperations.UpdateCourse(course, updateCourseRequest);

            ResponseMessages.Add(response);
        }

        [When(@"I create a new version of '(.*)' course named '(.*)' with the following info")]
        public void WhenICreateANewVersionOfCourseNamedWithTheFollowingInfo(string courseName, string newVersionName, Table table)
        {
            var request = table.CreateInstance<VersionRequest>();

            var course = Givens.Courses[courseName];         
            request.ParentVersionId = course.Id;

            var resource = PostOperations.CreateCourseVersion(request);

            Givens.Courses.Add(newVersionName, resource);
            ResponseMessages.Add(resource.Response);
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

            var response = ApiFeature.ApiTestHost.Client.PostAsync(postUri, versionRequest, new JsonMediaTypeFormatter()).Result;

            ScenarioContext.Current["ResponseToValidate"] = response;
            ResponseMessages.Add(response);
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
    }
}
