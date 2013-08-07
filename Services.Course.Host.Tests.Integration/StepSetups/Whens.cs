using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Host.Tests.Integration.Operations;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace BpeProducts.Services.Course.Host.Tests.Integration.StepSetups
{
    [Binding]
    public class Whens
    {
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
                            select new OutcomeRequest { Description = r["Description"], TenantId = ApiFeature.TenantId }).ToList();

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
                            select new OutcomeRequest { Description = r["Description"], TenantId = ApiFeature.TenantId }).ToList();

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

            var response = PutOperations.AssociateCourseWithPrograms(course, new List<ProgramResource> { program });
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
                var result = PutOperations.OutcomeSupportsLearningOutcome(supportingOutcome.Value, supportedOutcome);
                result.EnsureSuccessStatusCode();
            }
        }
    }
}
