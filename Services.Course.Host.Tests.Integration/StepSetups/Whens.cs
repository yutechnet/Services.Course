using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Host.Tests.Integration.Operations;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
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

        [When(@"I create a course from the template '(.*)' with the following")]
        public void WhenICreateACourseFromTheTemplateWithTheFollowing(string templateName, Table table)
        {
            var template = Givens.Courses[templateName];
            var courseRequest = table.CreateInstance<SaveCourseRequest>();
            courseRequest.TemplateCourseId = template.Id;

            PostOperations.CreateCourse(courseRequest.Name, courseRequest);
        }

        [When(@"I associate the existing learning outcomes to '(.*)' program")]
        public void WhenIAssociateTheExistingLearningOutcomesToProgram(string programName, Table table)
        {
            var program = Givens.Programs[programName];

            foreach (var row in table.Rows)
            {
                var outcome = Givens.LearningOutcomes[row["Description"]];

                PutOperations.ProgramSupportsLearningOutcome(program, outcome);
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
                PostOperations.CreateEntityLearningOutcome(request.Description, course, request);
            }
        }

        [When(@"I associate the existing learning outcomes to '(.*)' course")]
        public void WhenIAssociateTheExistingLearningOutcomesToCourse(string courseName, Table table)
        {
            var course = Givens.Courses[courseName];

            foreach (var row in table.Rows)
            {
                var outcome = Givens.LearningOutcomes[row["Description"]];

                PutOperations.CourseSupportsLearningOutcome(course, outcome);
            }
        }

        [When(@"I associate the existing learning outcomes to '(.*)' segment")]
        public void WhenIAssociateTheExistingLearningOutcomesToSegment(string segmentName, Table table)
        {
            var segment = Givens.Segments[segmentName];

            foreach (var row in table.Rows)
            {
                var outcome = Givens.LearningOutcomes[row["Description"]];

                PutOperations.SegmentSupportsLearningOutcome(segment, outcome);
            }
        }

        [When(@"I associate '(.*)' course with '(.*)' program")]
        public void WhenIAssociateCourseWithProgram(string courseName, string programName)
        {
            var course = Givens.Courses[courseName];
            var program = Givens.Programs[programName];

            course.Dto = GetOperations.GetCourse(course.ResourceUri);

            PutOperations.AssociateCourseWithPrograms(course, new List<ProgramResource> {program});
        }

        [When(@"I associate '(.*)' course with the following programs")]
        public void WhenIAssociateCourseWithTheFollowingPrograms(string courseName, Table table)
        {
            var course = Givens.Courses[courseName];
            var programs = (from r in table.Rows select Givens.Programs[r["Program Name"]]).ToList();

            course.Dto = GetOperations.GetCourse(course.ResourceUri);

            PutOperations.AssociateCourseWithPrograms(course, programs);
        }

        [When(@"I remove '(.*)' course from '(.*)'")]
        public void WhenIRemoveCourseFrom(string courseName, string programName)
        {
            var course = Givens.Courses[courseName];
            var program = Givens.Programs[programName];

            course.Dto = GetOperations.GetCourse(course.ResourceUri);

            PutOperations.DisassociateCourseWithPrograms(course, new List<ProgramResource> {program});
        }

        [When(@"the outcome '(.*)' supports the following outcomes")]
        public void WhenOutcomeSupportsTheFollowingOutcomes(string supportingOutcomeName, Table table)
        {
            var supportingOutcome = Givens.LearningOutcomes[supportingOutcomeName];

            foreach (var row in table.Rows)
            {
                var supportedOutcome = Givens.LearningOutcomes[row["Description"]];

                //TODO: This is an issue... supportingOutcome and supoortedOutcome should be flipped
                PutOperations.OutcomeSupportsLearningOutcome(supportedOutcome, supportingOutcome);
            }
        }

        [When(@"the outcome '(.*)' is supported by the following outcomes")]
        public void WhenOutcomeIsSupportedByTheFollowingOutcomes(string supportedOutcomeName, Table table)
        {
            var supportedOutcome = Givens.LearningOutcomes[supportedOutcomeName];

            foreach (var row in table.Rows)
            {
                var supportingOutcome = Givens.LearningOutcomes[row["Description"]];

                //TODO: This is an issue... supportingOutcome and supoortedOutcome should be flipped
                PutOperations.OutcomeSupportsLearningOutcome(supportingOutcome, supportedOutcome);
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
                PutOperations.OutcomeDoesNotSupportLearningOutcome(supportingOutcome.Value, supportedOutcome);
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
                PutOperations.ProgramDoesNotSupportLearningOutcome(programResource, learningOutcome);
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

                PutOperations.PublishCourse(course, request);
            }
        }

        [When(@"I publish the following learning outcomes")]
        public void WhenIPublishTheFollowingLearningOutcomes(Table table)
        {
            foreach (var row in table.Rows)
            {
                var learningOutcome = Givens.LearningOutcomes[row["Name"]];

                var request = new PublishRequest
                {
                    PublishNote = row["Note"]
                };

                PutOperations.PublishLearningOutcome(learningOutcome, request);
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

            PutOperations.SetCoursePrerequisites(course, request);
        }

        [When(@"I add the following learning activity to '(.*)' course segment")]
        public void WhenIAddTheFollowingLearningActivityToCourseSegment(string segmentName, Table table)
        {
            var segment = Givens.Segments[segmentName];

            var request = table.CreateInstance<SaveCourseLearningActivityRequest>();
            PostOperations.CreateCourseLearningActivity(request.Name, segment, request);
        }

        [When(@"I retrieve the course learning activity '(.*)'")]
        public void WhenIRetrieveTheCourseLearningActivity(string activityName)
        {
            var resource = Givens.CourseLearningActivities[activityName];
            GetOperations.GetCourseLearningActivity(resource.ResourceUri);
        }

        [When(@"I remove ""(.*)"" learning activity")]
        public void WhenIRemoveLearningActivity(string activityName)
        {
            var resource = Givens.CourseLearningActivities[activityName];
            DeleteOperations.DeleteResource(resource);
        }

        [When(@"I update '(.*)' learning activity with the following info")]
        public void WhenIUpdateLearningActivityWithTheFollowingInfo(string activityName, Table table)
        {
            var resourse = Givens.CourseLearningActivities[activityName];
            
            var learningActivity = table.CreateInstance<SaveCourseLearningActivityRequest>();

            PutOperations.UpdateCourseLearningActivity(resourse, learningActivity);
        }

        [When(@"I change the '(.*)' learning outcome description to '(.*)'")]
        public void WhenIChangeTheLearningOutcomeDescriptionTo(string leaningOutcomeName, string newDescription)
        {
            var resource = Givens.LearningOutcomes[leaningOutcomeName];

            var request = new OutcomeRequest
                {
                    Description = newDescription
                };

            PutOperations.UpdateLearningOutcome(resource, request);
        }

        [When(@"I update '(.*)' learning outcome with the following info")]
        public void WhenIUpdateLearningOutcomeWithTheFollowingInfo(string leaningOutcomeName, Table table)
        {
            var resource = Givens.LearningOutcomes[leaningOutcomeName];
            var request = table.CreateInstance<OutcomeRequest>();

            PutOperations.UpdateLearningOutcome(resource, request);
        }

        [When(@"I get the learning outcome '(.*)'")]
        public void WhenIGetTheLearningOutcome(string leaningOutcomeName)
        {
            var resource = Givens.LearningOutcomes[leaningOutcomeName];
            GetOperations.GetLearningOutcome(resource.ResourceUri);
        }

        [When(@"I delete the '(.*)' learning outcome")]
        public void WhenIDeleteTheLearningOutcome(string leaningOutcomeName)
        {
            var resource = Givens.LearningOutcomes[leaningOutcomeName];

            DeleteOperations.DeleteResource(resource);
        }

        [When(@"I retrieve '(.*)' course")]
        public void WhenIRetrieveCourse(string courseName)
        {
            var resource = Givens.Courses[courseName];
            GetOperations.GetCourse(resource.ResourceUri);
        }

        [When(@"I update '(.*)' course with the following info")]
        public void WhenIUpdateCourseWithTheFollowingInfo(string courseName, Table table)
        {
            var course = Givens.Courses[courseName];
            var updateCourseRequest = table.CreateInstance<UpdateCourseRequest>();

            PutOperations.UpdateCourse(course, updateCourseRequest);
        }

        [When(@"I create a new version of '(.*)' course named '(.*)' with the following info")]
        public void WhenICreateANewVersionOfCourseNamedWithTheFollowingInfo(string courseName, string newVersionName, Table table)
        {
            var request = table.CreateInstance<VersionRequest>();

            var course = Givens.Courses[courseName];         
            request.ParentVersionId = course.Id;

            PostOperations.CreateCourseVersion(newVersionName, request);
        }

        [When(@"I update the course segments as follows")]
        public void WhenIUpdateTheCourseSegmentAsFollows(Table table)
        {
            foreach (var row in table.Rows)
            {
                var courseSegmentName = row["Name"];
                var courseSegment = Givens.Segments[courseSegmentName];

                var request = new SaveCourseSegmentRequest
                {
                    Name = courseSegmentName,
                    Description = row["Description"],
                    Type = row["Type"],
                    DisplayOrder = row.ContainsKey("DisplayOrder") ? int.Parse(row["DisplayOrder"]) : 0
                };

                PutOperations.UpdateCourseSegmentRequest(courseSegment, request);
            }
        }

        [When(@"I add the following content to '(.*)' segment")]
        public void WhenIAddTheFollowingContentToSegment(string courseSegmentName, Table table)
        {
            var resource = Givens.Segments[courseSegmentName];
            var courseSegment = GetOperations.GetSegment(resource.ResourceUri);

            var request = new SaveCourseSegmentRequest
            {
                Name = courseSegment.Name,
                Type = courseSegment.Type,
                Description = courseSegment.Description,
                TenantId = ApiFeature.TenantId,
                Content = table.Rows.Select(row => new Content { Id = Guid.Parse(row["Id"]), Type = row["Type"] }).ToList()
            };

            PutOperations.UpdateCourseSegmentRequest(resource, request);
        }

        [When(@"I delete the following segments")]
        public void WhenIDeleteTheFollowingSegments(Table table)
        {
            var segmentNames = (from r in table.Rows select r["Name"]).ToList();

            foreach (var segmentName in segmentNames)
            {
                var resource = Givens.Segments[segmentName];
                DeleteOperations.DeleteResource(resource);
            }
        }

        [When(@"I create a new version of '(.*)' outcome named '(.*)' with the following info")]
        public void WhenICreateANewVersionOfWithTheFollowingInfo(string outcomeName, string newOutcomeName, Table table)
        {
            var versionRequest = table.CreateInstance<VersionRequest>();

            var resource = Givens.LearningOutcomes[outcomeName];
            versionRequest.ParentVersionId = resource.Id;

            PostOperations.CreateLearningOutcomeVersion(newOutcomeName, resource, versionRequest);
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

        [When(@"I delete '(.*)' course")]
        public void WhenIDeleteCourse(string courseName)
        {
            var resourceUri = Givens.Courses[courseName].ResourceUri;
            var response = ApiFeature.ApiTestHost.Client.DeleteAsync(resourceUri).Result;

            ResponseMessages.Add(response);
        }

        [When(@"I create a course under organization (.*)")]
        public void WhenICreateACourseUnderOrganization(string orgObjectName)
        {
            var orgObjectNameId = (Guid)ScenarioContext.Current[orgObjectName];
            var saveCourseRequest = new SaveCourseRequest
            {
                Name = "English 101",
                Description = "English",
                Code = "ENG101",
                CourseType = ECourseType.Traditional,
                IsTemplate = false,
                TenantId = 999999,
                OrganizationId = orgObjectNameId
            };

            var postUri = FeatureContext.Current.Get<string>("CourseLeadingPath");
            var httpResponseMessage = ApiFeature.ApiTestHost.Client.PostAsJsonAsync(postUri, saveCourseRequest).Result;

            ScenarioContext.Current["httpResponseMessage"] = httpResponseMessage;     
        }
    }
}
