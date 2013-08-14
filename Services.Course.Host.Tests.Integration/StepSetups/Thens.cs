using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Host.Tests.Integration.Operations;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace BpeProducts.Services.Course.Host.Tests.Integration.StepSetups
{
    [Binding]
    public class Thens
    {
        [Then(@"the course '(.*)' should have the following info")]
        public void ThenTheCourseShouldHaveTheFollowingInfo(string courseName, Table table)
        {
            var courseResource = Givens.Courses[courseName];

            var actual = GetOperations.GetCourse(courseResource.ResourceUri);

            table.CompareToInstance(actual);
        }

        [Then(@"I should get a '(.*)' status for course '(.*)'")]
        public void ThenIShouldGetAStatus(string status, string courseName)
        {
            var courseResource = Givens.Courses[courseName];

            var expectedCode = (HttpStatusCode) Enum.Parse(typeof (HttpStatusCode), status);
            Assert.That(courseResource.Response.StatusCode, Is.EqualTo(expectedCode));
        }

        [Then(@"the course '(.*)' includes the following programs")]
        public void ThenTheCourseIncludesTheFollowingPrograms(string courseName, Table table)
        {
            var resource = Givens.Courses[courseName];

            var course = GetOperations.GetCourse(resource.ResourceUri);
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
            var course = GetOperations.GetCourse(courseResource.ResourceUri);

            var programResourse = Givens.Programs[programName];

            Assert.That(course.ProgramIds.Count, Is.EqualTo(1));
            CollectionAssert.Contains(course.ProgramIds, programResourse.Id);
        }

        [Then(@"the course '(.*)' includes the following program information:")]
        public void ThenTheCourseIncludesTheFollowingProgramInformation(string courseName, Table table)
        {
            var courseResource = Givens.Courses[courseName];
            var course = GetOperations.GetCourse(courseResource.ResourceUri);

            var programIds = (from r in table.Rows select Givens.Programs[r["Program Name"]].Id).ToList();

            CollectionAssert.AreEquivalent(course.ProgramIds, programIds);
        }

        [Then(@"the course '(.*)' should have the following prerequisites")]
        public void ThenTheCourseShouldHaveTheFollowingPrerequisites(string courseName, Table table)
        {
            var courseResource = Givens.Courses[courseName];
            var course = GetOperations.GetCourse(courseResource.ResourceUri);

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

        [Then(@"I get the following responses")]
        public void ThenIGetTheFollowingResponses(Table table)
        {
            for (var i = 0; i < table.Rows.Count; i++)
            {
                var result = Whens.ResponseMessages[Whens.ResponseMessages.Count - table.Rows.Count + i];
                var statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), table.Rows[i]["StatusCode"]); 

                Assert.That(result.StatusCode, Is.EqualTo(statusCode));
            }
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

        [Then(@"the segment '(.*)' should have the following learning activities:")]
        public void ThenTheSegmentShouldHaveTheFollowingLearningActivities(string segmentName, Table table)
        {
            var resource = Givens.Segments[segmentName];
            var segment = GetOperations.GetSegment(resource.ResourseUri);

            var expectedLearningActivities =
                (from r in table.Rows select Givens.CourseLearningActivities[r["Name"]].Id).ToList();
            var actualLearningActivities = (from a in segment.CourseLearningActivities select a.Id).ToList();

            CollectionAssert.AreEquivalent(expectedLearningActivities, actualLearningActivities);
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
            var response = ApiFeature.ApiTestHost.Client.GetAsync(course.ResourceUri + "/" + "supports").Result;
            var outcomes = response.Content.ReadAsAsync<List<OutcomeInfo>>().Result;
            table.CompareToSet(outcomes);
        }

        [Then(@"'(.*)' program is associated with the only following learning outcomes")]
        public void ThenProgramIsAssociatedWithTheOnlyFollowingLearningOutcomes(string programName, Table table)
        {
            var resource = Givens.Programs[programName];

            var actualOutcomes =
                (from o in GetOperations.GetSupportedOutcomes(resource.ResourceUri) select o.Description).ToList();
            var expectedOutcomes = (from o in table.Rows select o["Description"]).ToList();

            CollectionAssert.AreEquivalent(expectedOutcomes, actualOutcomes);
        }

        [Then(@"learning outcome '(.*)' has the following learning outcomes")]
        public void ThenHasTheFollowingLearningOutcomes(string learningOutcomeName, Table table)
        {
            var resource = Givens.LearningOutcomes[learningOutcomeName];

            var actualOutcomes =
                (from o in GetOperations.GetSupportedOutcomes(resource.ResourceUri) select o.Description).ToList();
            var expectedOutcomes = (from o in table.Rows select o["Description"]).ToList();

            CollectionAssert.AreEquivalent(expectedOutcomes, actualOutcomes);
        }

        [Then(@"I get the entity learning outcomes as follows:")]
        public void ThenIGetTheEntityLearningOutcomesAsFollows(Table table)
        {
            var expectedEntityOutcomes = new Dictionary<IResource, IList<Guid>>();
            foreach (var row in table.Rows)
            {
                var entityType = row["EntityType"];
                var entityName = row["EntityName"];
                IResource resource = null;

                switch (entityType)
                {
                    case "Program":
                        resource = Givens.Programs[entityName];
                        break;
                    case "Course":
                        resource = Givens.Courses[entityName];
                        break;
                    case "Segment":
                        resource = Givens.Segments[entityName];
                        break;
                }

                if (resource == null)
                    throw new Exception("No recourse found for entity type " + entityType);

                var expectedOutcomes = (from o in row["LearningOutcomes"].Split(new[] {','})
                                        where !string.IsNullOrWhiteSpace(o)
                                        select Givens.LearningOutcomes[o.Trim()].Id).ToList();
                expectedEntityOutcomes.Add(resource, expectedOutcomes);
            }

            var entityIdsToGet = (from e in expectedEntityOutcomes.Keys select e.Id).ToList();
            var actualEntityOutcomes = GetOperations.GetEntityLearningOutcomes(entityIdsToGet);

            foreach (var expectedEntity in expectedEntityOutcomes)
            {
                List<OutcomeInfo> outcomes;

                if (actualEntityOutcomes.TryGetValue(expectedEntity.Key.Id, out outcomes))
                {
                    var actualEntityOutcomeIds = (from o in outcomes select o.Id).ToList();
                    CollectionAssert.AreEquivalent(expectedEntity.Value.ToList(), actualEntityOutcomeIds);
                }
                else
                {
                    Assert.That(!actualEntityOutcomes.Keys.Contains(expectedEntity.Key.Id));
                }
            }
        }
    }
}

