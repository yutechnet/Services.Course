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

            var expectedCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), status);
            Assert.That(courseResource.Response.StatusCode, Is.EqualTo(expectedCode));
        }

        [Then(@"the course '(.*)' includes the following programs:")]
        public void ThenTheCourseIncludesTheFollowingPrograms(string courseName, Table table)
        {
            var resource = Givens.Courses[courseName];

            var course = GetOperations.GetCourse(resource.ResourseUri);
            var expectedProgramIds = (from r in table.Rows select Givens.Programs[r["Program Name"]].Id).ToList();

            CollectionAssert.AreEquivalent(course.ProgramIds, expectedProgramIds);
        }

        [Then(@"'(.*)' program is associated with the following learning outcomes:")]
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
    }
}
