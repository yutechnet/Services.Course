﻿using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Host.Tests.Integration.Operations;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources;
using NHibernate.Linq;
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
            //TODO: Why not create a course steps file to put common code?
            Givens.CreateCourseTemplate(templateName, table);
        }

        [When(@"I associate the existing learning outcomes to '(.*)' program")]
        public void WhenIAssociateTheExistingLearningOutcomesToProgram(string programName, Table table)
        {
            var program = Resources<ProgramResource>.Get(programName);

            foreach (var row in table.Rows)
            {
                var outcome = Resources<LearningOutcomeResource>.Get(row["Description"]);

                PutOperations.ProgramSupportsLearningOutcome(program, outcome);
            }
        }

        [When(@"I associate the newly created learning outcomes to '(.*)' course")]
        public void WhenIAssociateTheNewlyCreatedLearningOutcomesToCourse(string courseName, Table table)
        {
            var requests = (from r in table.Rows
                            select new OutcomeRequest {Description = r["Description"], TenantId = ApiFeature.TenantId})
                .ToList();

            var course = Resources<CourseResource>.Get(courseName);

            foreach (var request in requests)
            {
                PostOperations.CreateEntityLearningOutcome(request.Description, course, request);
            }
        }

        [When(@"I associate the existing learning outcomes to '(.*)' course")]
        public void WhenIAssociateTheExistingLearningOutcomesToCourse(string courseName, Table table)
        {
            var course = Resources<CourseResource>.Get(courseName);

            foreach (var row in table.Rows)
            {
                var outcome = Resources<LearningOutcomeResource>.Get(row["Description"]);

                PutOperations.CourseSupportsLearningOutcome(course, outcome);
            }
        }

        [When(@"I associate the existing learning outcomes to '(.*)' segment")]
        public void WhenIAssociateTheExistingLearningOutcomesToSegment(string segmentName, Table table)
        {
            var segment = Resources<CourseSegmentResource>.Get(segmentName);

            foreach (var row in table.Rows)
            {
                var outcome = Resources<LearningOutcomeResource>.Get(row["Description"]);

                PutOperations.SegmentSupportsLearningOutcome(segment, outcome);
            }
        }

        [When(@"I associate '(.*)' course with '(.*)' program")]
        public void WhenIAssociateCourseWithProgram(string courseName, string programName)
        {
            var course = Resources<CourseResource>.Get(courseName);
            var program = Resources<ProgramResource>.Get(programName);

            PutOperations.AssociateCourseWithPrograms(course, new List<ProgramResource> {program});
        }

        [When(@"I associate '(.*)' course with the following programs")]
        public void WhenIAssociateCourseWithTheFollowingPrograms(string courseName, Table table)
        {
            var course = Resources<CourseResource>.Get(courseName);
            var programs = (from r in table.Rows select Resources<ProgramResource>.Get(r["Program Name"])).ToList();

            PutOperations.AssociateCourseWithPrograms(course, programs);
        }

        [When(@"I remove '(.*)' course from '(.*)'")]
        public void WhenIRemoveCourseFrom(string courseName, string programName)
        {
            var course = Resources<CourseResource>.Get(courseName);
            var program = Resources<ProgramResource>.Get(programName);

            PutOperations.DisassociateCourseWithPrograms(course, new List<ProgramResource> {program});
        }

        [When(@"the outcome '(.*)' supports the following outcomes")]
        public void WhenOutcomeSupportsTheFollowingOutcomes(string supportingOutcomeName, Table table)
        {
            var supportingOutcome = Resources<LearningOutcomeResource>.Get(supportingOutcomeName);

            foreach (var row in table.Rows)
            {
                var supportedOutcome = Resources<LearningOutcomeResource>.Get(row["Description"]);

                //TODO: This is an issue... supportingOutcome and supoortedOutcome should be flipped
                PutOperations.OutcomeSupportsLearningOutcome(supportedOutcome, supportingOutcome);
            }
        }

        [When(@"the outcome '(.*)' is supported by the following outcomes")]
        public void WhenOutcomeIsSupportedByTheFollowingOutcomes(string supportedOutcomeName, Table table)
        {
            var supportedOutcome = Resources<LearningOutcomeResource>.Get(supportedOutcomeName);

            foreach (var row in table.Rows)
            {
                var supportingOutcome = Resources<LearningOutcomeResource>.Get(row["Description"]);

                //TODO: This is an issue... supportingOutcome and supoortedOutcome should be flipped
                PutOperations.OutcomeSupportsLearningOutcome(supportingOutcome, supportedOutcome);
            }
        }

        [When(@"I disassociate the following learning outcomes from '(.*)' learning outcome")]
        public void WhenIDisassociateTheFollowingLearningOutcomesFromLearningOutcome(string learningOutcomeName, Table table)
        {
            var supportedOutcome = Resources<LearningOutcomeResource>.Get(learningOutcomeName);

            var descriptions = (from o in table.Rows select o["Description"]).ToList();
            var supportingOutcomes = (from o in Resources<LearningOutcomeResource>.Get()
                                      where descriptions.Contains(o.Key)
                                      select o.Value).ToList();

            foreach (var supportingOutcome in supportingOutcomes)
            {
                PutOperations.OutcomeDoesNotSupportLearningOutcome(supportingOutcome, supportedOutcome);
            }
        }

        [When(@"I disassociate the following learning outcomes from '(.*)' program")]
        public void WhenIDisassociateTheFollowingLearningOutcomesFromProgram(string programName, Table table)
        {
            var programResource = Resources<ProgramResource>.Get(programName);

            var learningOutcomeNames = (from o in table.Rows select o["Description"]).ToList();

            foreach (var learningOutcomeName in learningOutcomeNames)
            {
                var learningOutcome = Resources<LearningOutcomeResource>.Get(learningOutcomeName);
                PutOperations.ProgramDoesNotSupportLearningOutcome(programResource, learningOutcome);
            }
        }

        [When(@"I publish the following courses")]
        public void WhenIPublishTheFollowingCourses(Table table)
        {
            foreach (var row in table.Rows)
            {
                var course = Resources<CourseResource>.Get(row["Name"]);

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
                var learningOutcome = Resources<LearningOutcomeResource>.Get(row["Name"]);

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
            var course = Resources<CourseResource>.Get(courseName);

            var prerequisiteIds = (from r in table.Rows select Resources<CourseResource>.Get(r["Name"]).Id).ToList();

            var request = new UpdateCoursePrerequisites
                {
                    PrerequisiteIds = prerequisiteIds
                };

            PutOperations.SetCoursePrerequisites(course, request);
        }

        [When(@"I add the following learning activity to '(.*)' course segment")]
        public void WhenIAddTheFollowingLearningActivityToCourseSegment(string segmentName, Table table)
        {
            var segment = Resources<CourseSegmentResource>.Get(segmentName);

            var request = table.CreateInstance<SaveCourseLearningActivityRequest>();
            PostOperations.CreateCourseLearningActivity(request.Name, segment, request);
        }

        [When(@"I retrieve the course learning activity '(.*)'")]
        public void WhenIRetrieveTheCourseLearningActivity(string activityName)
        {
            var resource = Resources<CourseLearningActivityResource>.Get(activityName);
            GetOperations.GetCourseLearningActivity(resource.ResourceUri);
        }

        [When(@"I remove ""(.*)"" learning activity")]
        public void WhenIRemoveLearningActivity(string activityName)
        {
            var resource = Resources<CourseLearningActivityResource>.Get(activityName);
            DeleteOperations.DeleteResource(resource);
        }

        [When(@"I update '(.*)' learning activity with the following info")]
        public void WhenIUpdateLearningActivityWithTheFollowingInfo(string activityName, Table table)
        {
            var resourse = Resources<CourseLearningActivityResource>.Get(activityName);
            
            var learningActivity = table.CreateInstance<SaveCourseLearningActivityRequest>();

            PutOperations.UpdateCourseLearningActivity(resourse, learningActivity);
        }

        [When(@"I change the '(.*)' learning outcome description to '(.*)'")]
        public void WhenIChangeTheLearningOutcomeDescriptionTo(string learningOutcomeName, string newDescription)
        {
            var resource = Resources<LearningOutcomeResource>.Get(learningOutcomeName);

            var request = new OutcomeRequest
                {
                    Description = newDescription
                };

            PutOperations.UpdateLearningOutcome(resource, request);
        }

        [When(@"I update '(.*)' learning outcome with the following info")]
        public void WhenIUpdateLearningOutcomeWithTheFollowingInfo(string learningOutcomeName, Table table)
        {
            var resource = Resources<LearningOutcomeResource>.Get(learningOutcomeName);
            var request = table.CreateInstance<OutcomeRequest>();

            PutOperations.UpdateLearningOutcome(resource, request);
        }

        [When(@"I get the learning outcome '(.*)'")]
        public void WhenIGetTheLearningOutcome(string learningOutcomeName)
        {
            var resource = Resources<LearningOutcomeResource>.Get(learningOutcomeName);
            GetOperations.GetLearningOutcome(resource.ResourceUri);
        }

        [When(@"I delete the '(.*)' learning outcome")]
        public void WhenIDeleteTheLearningOutcome(string learningOutcomeName)
        {
            var resource = Resources<LearningOutcomeResource>.Get(learningOutcomeName);

            DeleteOperations.DeleteResource(resource);
        }

        [When(@"I retrieve '(.*)' course")]
        public void WhenIRetrieveCourse(string courseName)
        {
            var resource = Resources<CourseResource>.Get(courseName);
            GetOperations.GetCourse(resource.ResourceUri);
        }

        [When(@"I update '(.*)' course with the following info")]
        public void WhenIUpdateCourseWithTheFollowingInfo(string courseName, Table table)
        {
            var course = Resources<CourseResource>.Get(courseName);
            var updateCourseRequest = table.CreateInstance<UpdateCourseRequest>();

            PutOperations.UpdateCourse(course, updateCourseRequest);
        }

        [When(@"I create a new version of '(.*)' course named '(.*)' with the following info")]
        public void WhenICreateANewVersionOfCourseNamedWithTheFollowingInfo(string courseName, string newVersionName, Table table)
        {
            var request = table.CreateInstance<VersionRequest>();

            var course = Resources<CourseResource>.Get(courseName);       
            request.ParentVersionId = course.Id;

            PostOperations.CreateCourseVersion(newVersionName, request);
        }

        [When(@"I update the course segments as follows")]
        public void WhenIUpdateTheCourseSegmentAsFollows(Table table)
        {
            foreach (var row in table.Rows)
            {
                var segmentName = row["Name"];
                var courseSegment = Resources<CourseSegmentResource>.Get(segmentName);

                var request = new SaveCourseSegmentRequest
                {
                    Name = segmentName,
                    Description = row["Description"],
                    Type = row["Type"],
                    DisplayOrder = row.ContainsKey("DisplayOrder") ? int.Parse(row["DisplayOrder"]) : 0
                };

                PutOperations.UpdateCourseSegmentRequest(courseSegment, request);
            }
        }

        [When(@"I add the following content to '(.*)' segment")]
        public void WhenIAddTheFollowingContentToSegment(string segmentName, Table table)
        {
            var resource = Resources<CourseSegmentResource>.Get(segmentName);
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
                var resource = Resources<CourseSegmentResource>.Get(segmentName);
                DeleteOperations.DeleteResource(resource);
            }
        }

        [When(@"I create a new version of '(.*)' outcome named '(.*)' with the following info")]
        public void WhenICreateANewVersionOfWithTheFollowingInfo(string learningOutcomeName, string newOutcomeName, Table table)
        {
            var versionRequest = table.CreateInstance<VersionRequest>();

            var resource = Resources<LearningOutcomeResource>.Get(learningOutcomeName);
            versionRequest.ParentVersionId = resource.Id;

            PostOperations.CreateLearningOutcomeVersion(newOutcomeName, resource, versionRequest);
        }

        [When(@"I create a course without a version")]
        public void WhenICreateACourseWithoutAVersion()
        {
            var versionRequest = new VersionRequest
                {
                    ParentVersionId = Guid.NewGuid(),
                    VersionNumber = null
                };

            PostOperations.CreateCourseVersion("badversion", versionRequest);
        }

        [When(@"I delete '(.*)' course")]
        public void WhenIDeleteCourse(string courseName)
        {
            var resource = Resources<CourseResource>.Get(courseName);

            DeleteOperations.DeleteResource(resource);
        }

        //[When(@"I create a course under organization (.*)")]
        //public void WhenICreateACourseUnderOrganization(string orgObjectName)
        //{
        //    var orgObjectNameId = (Guid)ScenarioContext.Current[orgObjectName];
        //    var saveCourseRequest = new SaveCourseRequest
        //    {
        //        Name = "English 101",
        //        Description = "English",
        //        Code = "ENG101",
        //        CourseType = ECourseType.Traditional,
        //        IsTemplate = false,
        //        OrganizationId = orgObjectNameId
        //    };

        //    PostOperations.CreateCourse(saveCourseRequest.Name, saveCourseRequest);
        //}

        [When(@"I modify the program '(.*)' info to reflect the following")]
        public void WhenIModifyTheProgramInfoToReflectTheFollowing(string programName, Table table)
        {
            var resource = Resources<ProgramResource>.Get(programName);
            var request = table.CreateInstance<UpdateProgramRequest>();

            PutOperations.UpdateResource(resource, request);
        }

        [When(@"I delete the program '(.*)'")]
        public void WhenIDeleteTheProgram(string programName)
        {
            var resource = Resources<ProgramResource>.Get(programName);
            DeleteOperations.DeleteResource(resource);
        }

        [When(@"I get the program '(.*)'")]
        public void WhenIGetTheProgram(string programName)
        {
            var resource = Resources<ProgramResource>.Get(programName);
            GetOperations.GetProgram(resource.ResourceUri);
        }

        [When(@"I attempt to create the following programs")]
        public void WhenIAttemptToCreateTheFollowingPrograms(Table table)
        {
            foreach (var row in table.Rows)
            {
                var saveProgramRequest = new SaveProgramRequest
                {
                    Description = row["Description"],
                    Name = row["Name"],
                    ProgramType = row["ProgramType"],
                    OrganizationId = Account.Givens.Organizations[row["OrganizationName"]].Id
                };

                PostOperations.CreateProgram(saveProgramRequest.Name, saveProgramRequest);
            }
        }

	    [When(@"I get the course templates for organization ""(.*)"" to scenario context name ""(.*)""")]
        public void WhenIGetTheCourseTemplatesForOrganizationToScenarioContextName(string organizationName, string scenarioContextName)
        {
            var organizationId = ScenarioContext.Current[organizationName].As<Guid>();
            List<Guid> courseTemplateIds = GetOperations.GetCourseTemplateIds(organizationId);
            ScenarioContext.Current[scenarioContextName] = courseTemplateIds;
        }

        [When(@"the outcome '(.*)' supports the following learning outcomes asynchronously")]
        public void WhenTheOutcomeSupportsTheFollowingLearningOutcomesAsynchronously(string supportingOutcomeName, Table table)
        {
            var supportingOutcome = Resources<LearningOutcomeResource>.Get(supportingOutcomeName);

            foreach (var row in table.Rows)
            {
                var supportedOutcome = Resources<LearningOutcomeResource>.Get(row["Description"]);

                //TODO: This is an issue... supportingOutcome and supoortedOutcome should be flipped
                PutOperations.OutcomeSupportsLearningOutcome(supportingOutcome, supportedOutcome, true);
            }
        }

    }
}
