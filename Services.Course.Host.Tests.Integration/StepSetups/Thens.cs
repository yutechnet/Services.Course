using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Host.Tests.Integration.Operations;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace BpeProducts.Services.Course.Host.Tests.Integration.StepSetups
{
    [Binding]
    public class Thens
    {
        [Then(@"the course '(.*)' should have the following info:")]
        public void ThenTheCourseShouldHaveTheFollowingInfo(string courseName, Table table)
        {
            var courseResource = Givens.Courses[courseName];
            table.CompareToInstance(courseResource.Dto);
        }

        [Then(@"I should get a '(.*)' status for course '(.*)'")]
        public void ThenIShouldGetAStatus(string status, string courseName)
        {
            var courseResource = Givens.Courses[courseName];

            var expectedCode = (HttpStatusCode) Enum.Parse(typeof (HttpStatusCode), status);
            Assert.That(courseResource.Response.StatusCode, Is.EqualTo(expectedCode));
        }

        [Then(@"I get the following responses")]
        public void ThenIGetTheFollowingResponses(Table table)
        {
            var j = 0;
            for (var i = table.Rows.Count; i > 0; i--)
            {
                var expected = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), table.Rows[j]["StatusCode"]);
                var actual = Whens.ResponseMessages.ElementAt(Whens.ResponseMessages.Count - i).StatusCode;

                Assert.That(actual, Is.EqualTo(expected));
                j++;
            }
        }

        [Then(@"the course '(.*)' includes the following programs:")]
        public void ThenTheCourseIncludesTheFollowingPrograms(string courseName, Table table)
        {
            var resource = Givens.Courses[courseName];

            var course = GetOperations.GetCourse(resource.ResourseUri);
            var expectedProgramIds = (from r in table.Rows select Givens.Programs[r["Program Name"]].Id).ToList();

            CollectionAssert.AreEquivalent(course.ProgramIds, expectedProgramIds);
        }

        [Then(@"'(.*)' program is associated with the following learning outcomes")]
        public void ThenProgramIsAssociatedWithTheFollowingLearningOutcomes(string programName, Table table)
        {
            var resource = Givens.Programs[programName];

            var outcomes = GetOperations.GetEntityLearningOutcomes(new List<Guid> {resource.Id});

            var actualOutcomes = from o in outcomes[resource.Id] select o.Description;
            var expectedOutcomes = (from r in table.Rows select r["Description"]).ToList();

            CollectionAssert.AreEquivalent(expectedOutcomes, actualOutcomes);
        }

        [Then(@"the course '(.*)' includes '(.*)' program association")]
        public void ThenTheCourseIncludesProgramAssociation(string courseName, string programName)
        {
            var courseResource = Givens.Courses[courseName];
            var course = GetOperations.GetCourse(courseResource.ResourseUri);

            var programResourse = Givens.Programs[programName];

            Assert.That(course.ProgramIds.Count, Is.EqualTo(1));
            CollectionAssert.Contains(course.ProgramIds, programResourse.Id);
        }

        [Then(@"the course '(.*)' includes the following program information:")]
        public void ThenTheCourseIncludesTheFollowingProgramInformation(string courseName, Table table)
        {
            var courseResource = Givens.Courses[courseName];
            var course = GetOperations.GetCourse(courseResource.ResourseUri);

            var programIds = (from r in table.Rows select Givens.Programs[r["Program Name"]].Id).ToList();

            CollectionAssert.AreEquivalent(course.ProgramIds, programIds);
        }

        [Then(@"the course '(.*)' should have the following prerequisites")]
        public void ThenTheCourseShouldHaveTheFollowingPrerequisites(string courseName, Table table)
        {
            var courseResource = Givens.Courses[courseName];
            var course = GetOperations.GetCourse(courseResource.ResourseUri);

            var prereqIds = (from r in table.Rows select Givens.Courses[r["Name"]].Id).ToList();

            CollectionAssert.AreEquivalent(prereqIds, course.PrerequisiteCourseIds);
        }

        [Then(@"I get '(.*)' response")]
        public void ThenIGetResponse(string status)
        {
            var response = Whens.ResponseMessages.Last();

            var expectedStatusCode = (HttpStatusCode) Enum.Parse(typeof (HttpStatusCode), status);

            Assert.That(response.StatusCode.Equals(expectedStatusCode));
        }

        [Then(@"my course learning activity '(.*)' contains the following")]
        public void ThenMyCourseLearningActivityContainsTheFollowing(string learningActivityName, Table table)
        {
            var resource = Givens.CourseLearningActivities[learningActivityName];

            var courseLearningActivity = GetOperations.GetCourseLearningActivity(resource.ResourceUri);

            table.CompareToInstance(courseLearningActivity);
        }

        [Then(@"the program '(.*)' include the following course information:")]
        public void ThenTheProgramIncludeTheFollowingCourseInformation(string programName, Table table)
        {
            var programResource = Givens.Programs[programName];
            var program = GetOperations.GetProgram(programResource.ResourceUri);

            var courseIds = (from r in table.Rows select Givens.Courses[r["Course Name"]].Id).ToList();

            CollectionAssert.AreEquivalent(program.Courses.Select(c => c.Id).ToList(), courseIds);
        }

        [Then(@"the learning outcome '(.*)' should contain")]
        public void ThenTheLearningOutcomeShouldContain(string learningOutcomeName, Table table)
        {
            var resource = Givens.LearningOutcomes[learningOutcomeName];

            var actual = GetOperations.GetLearningOutcome(resource.ResourceUri);
            var expected = table.CreateInstance<OutcomeInfo>();

            Assert.That(expected.Description, Is.EqualTo(actual.Description));
        }

        [Then(@"the course '(.*)' includes the following learning outcomes:")]
        public void ThenTheCourseIncludesTheFollowingLearningOutcomes(string courseName, Table table)
        {
            var course = Givens.Courses[courseName];
            var response = ApiFeature.ApiTestHost.Client.GetAsync(course.ResourseUri + "/" + "supports").Result;
            var outcomes = response.Content.ReadAsAsync<List<OutcomeInfo>>().Result;
            table.CompareToSet(outcomes);
        }

        [Then(@"'(.*)' program is associated with the only following learning outcomes")]
        public void ThenProgramIsAssociatedWithTheOnlyFollowingLearningOutcomes(string programName, Table table)
        {
            var resource = Givens.Programs[programName];

            var actualOutcomes = (from o in GetOperations.GetSupportedOutcomes(resource.ResourceUri) select o.Description).ToList();
            var expectedOutcomes = (from o in table.Rows select o["Description"]).ToList();

            CollectionAssert.AreEquivalent(expectedOutcomes, actualOutcomes);
        }

        [Then(@"learning outcome '(.*)' has the following learning outcomes")]
        public void ThenHasTheFollowingLearningOutcomes(string learningOutcomeName, Table table)
        {
            var resource = Givens.LearningOutcomes[learningOutcomeName];

            var actualOutcomes = (from o in GetOperations.GetSupportedOutcomes(resource.ResourceUri) select o.Description).ToList();
            var expectedOutcomes = (from o in table.Rows select o["Description"]).ToList();

            CollectionAssert.AreEquivalent(expectedOutcomes, actualOutcomes);
        }
    }
}
