using BpeProducts.Common.WebApiTest.Extensions;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Host.Tests.Integration.Operations;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources.Account;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using BpeProducts.Common.WebApiTest.Framework;
using Moq;

namespace BpeProducts.Services.Course.Host.Tests.Integration.StepSetups
{
    [Binding]
    public class Givens
    {
        [Given(@"I have the following programs")]
        public void GivenIHaveTheFollowingPrograms(Table table)
        {
            foreach (var row in table.Rows)
            {
                var saveProgramRequest = new SaveProgramRequest
                {
                    Description = row["Description"],
                    Name = row["Name"],
                    ProgramType = row["ProgramType"],
                    OrganizationId = Resources<OrganizationResource>.Get(row["OrganizationName"]).Id,
                };

                var result = PostOperations.CreateProgram(saveProgramRequest.Name, saveProgramRequest);
                result.EnsureSuccessStatusCode();
            }
        }

        [Given(@"I have the following course templates")]
        [Given(@"I have the following courses")]
        public void GivenIHaveTheFollowingCourses(Table table)
        {
            foreach (var row in table.Rows)
            {
                string type;
                string isTemplate;

                var saveCourseRequest = new SaveCourseRequest
                {
                    Code = row["Code"],
                    Description = row["Description"],
                    Name = row["Name"],
                    OrganizationId = Resources<OrganizationResource>.Get(row["OrganizationName"]).Id,
                    PrerequisiteCourseIds = new List<Guid>(),
                    CourseType = row.TryGetValue("CourseType", out type) ? (ECourseType)Enum.Parse(typeof(ECourseType), type) : ECourseType.Traditional,
                    IsTemplate = row.TryGetValue("IsTemplate", out isTemplate) && bool.Parse(isTemplate),
                    Credit = decimal.Parse(table.Rows[0].GetValue("Credit", "0"))
                };

                var result = PostOperations.CreateCourse(saveCourseRequest.Name, saveCourseRequest);
                result.EnsureSuccessStatusCode();
            }
        }

        [Given(@"I create a course template ""(.*)"" for organization ""(.*)""")]
        public void GivenICreateACourseTemplateForOrganization(string courseName, string organizationName)
        {
            var saveCourseRequest = new SaveCourseRequest
                {
                    Name = courseName,
                    OrganizationId = (Guid)ScenarioContext.Current[organizationName],
                    Code = "TestCode",
                    Description = "TestDescription",
                    CourseType = ECourseType.Traditional,
                    IsTemplate = true,
                    PrerequisiteCourseIds = new List<Guid>()
                };

            var result = PostOperations.CreateCourse(courseName, saveCourseRequest);
            result.EnsureSuccessStatusCode();
        }

        [Given(@"I have the following course segments for '(.*)'")]
        public void GivenIHaveTheFollowingCourseSegmentsFor(string courseName, Table table)
        {
            foreach (var row in table.Rows)
            {
                var request = new SaveCourseSegmentRequest
                    {
                        Description = row["Description"],
                        Name = row["Name"],
                        Type = row["Type"],
                        DisplayOrder = row.ContainsKey("DisplayOrder") ? int.Parse(row["DisplayOrder"]) : 0,

                    };
                var activeDate = int.Parse(table.Rows[0].GetValue("ActiveDate", "0"));
                var inactiveDate = int.Parse(table.Rows[0].GetValue("InactiveDate", "0"));
                if (activeDate != 0) { request.ActiveDate = activeDate; }
                if (inactiveDate != 0) { request.InactiveDate = inactiveDate; }
                var course = Resources<CourseResource>.Get(courseName);
                var parentSegmentName = row["ParentSegment"];

                if (!string.IsNullOrWhiteSpace(parentSegmentName))
                {
                    var parentSegment = Resources<CourseSegmentResource>.Get(parentSegmentName);
                    request.ParentSegmentId = parentSegment.Id;
                }

                var result = PostOperations.CreateSegment(request.Name, course, request);
                result.EnsureSuccessStatusCode();
            }
        }

        [Given(@"I have the following learning outcomes")]
        public void GivenTheFollowingLearningOutcomesExist(Table table)
        {
            foreach (var row in table.Rows)
            {
                var request = new OutcomeRequest
                {
                    Description = row["Description"],
                };

                var result = PostOperations.CreateLearningOutcome(request.Description, request);
                result.EnsureSuccessStatusCode();
            }
        }

        [Given(@"I add the following course learning activities to '(.*)' course segment")]
        public void GivenIAddTheFollowingCourseLearningActivitiesToCourseSegment(string segmentName, Table table)
        {
            var learningActivityRequests = table.CreateSet<SaveCourseLearningActivityRequest>();
            var segment = Resources<CourseSegmentResource>.Get(segmentName);

            foreach (var request in learningActivityRequests)
            {
                var result = PostOperations.CreateCourseLearningActivity(request.Name, segment, request);
                result.EnsureSuccessStatusCode();
            }
        }

        [Given(@"I associate the newly created learning outcomes to '(.*)' program")]
        public void GivenIAssociateTheNewlyCreatedLearningOutcomesToProgram(string programName, Table table)
        {
            var requests = table.CreateSet<OutcomeRequest>();
            var resource = Resources<ProgramResource>.Get(programName);

            foreach (var request in requests)
            {
                var result = PostOperations.CreateEntityLearningOutcome(request.Description, resource, request);
                result.EnsureSuccessStatusCode();
            }
        }

        [Given(@"I associate the newly created learning outcomes to '(.*)' course")]
        public void GivenIAssociateTheNewlyCreatedLearningOutcomesToCourse(string courseName, Table table)
        {
            var requests = table.CreateSet<OutcomeRequest>();
            var resource = Resources<CourseResource>.Get(courseName);

            foreach (var request in requests)
            {
                request.TenantId = ApiFeature.TenantId;
                var result = PostOperations.CreateEntityLearningOutcome(request.Description, resource, request);
                result.EnsureSuccessStatusCode();
            }
        }

        [Given(@"I associate the newly created learning outcomes to '(.*)' segment")]
        public void GivenIAssociateTheNewlyCreatedLearningOutcomesToSegment(string segmentName, Table table)
        {
            var requests = table.CreateSet<OutcomeRequest>();
            var resource = Resources<CourseSegmentResource>.Get(segmentName);

            foreach (var request in requests)
            {
                request.TenantId = ApiFeature.TenantId;
                var result = PostOperations.CreateEntityLearningOutcome(request.Description, resource, request);
                result.EnsureSuccessStatusCode();
            }
        }

        [Given(@"I associate '(.*)' course with the following programs")]
        public void GivenIAssociateCourseWithTheFollowingPrograms(string courseName, Table table)
        {
            var course = Resources<CourseResource>.Get(courseName);
            var programs = (from r in table.Rows select Resources<ProgramResource>.Get(r["Program Name"])).ToList();

            var response = PutOperations.AssociateCourseWithPrograms(course, programs);
            response.EnsureSuccessStatusCode();
        }

        [Given(@"I create a course from the template '(.*)' with the following")]
        public void GivenICreateACourseFromTheTemplateWithTheFollowing(string templateName, Table table)
        {
            //TODO: Why not create a course steps file to put common code?
            var result = CreateCourseTemplate(templateName, table);
            result.EnsureSuccessStatusCode();
        }

        [Given(@"I have the following assets")]
        public void GivenIHaveTheFollowingAssets(Table table)
        {
            foreach (var row in table.Rows)
            {
                var name = row["Name"];
                var resource = new AssetResource
                    {
                        Id = Guid.NewGuid(),
                    };

                Resources<AssetResource>.Add(name, resource);
            }
        }

        public static HttpResponseMessage CreateCourseTemplate(string templateName, Table table)
        {
            var template = Resources<CourseResource>.Get(templateName);
            var courseRequest = table.CreateInstance<SaveCourseRequest>();
            courseRequest.OrganizationId = Resources<OrganizationResource>.Get(table.Rows[0]["OrganizationName"]).Id;
            courseRequest.TemplateCourseId = template.Id;

            var result = PostOperations.CreateCourse(courseRequest.Name, courseRequest);
            return result;
        }
    }
}
