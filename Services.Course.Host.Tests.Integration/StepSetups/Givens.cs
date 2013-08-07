using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Formatting;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Host.Tests.Integration.Operations;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources;
using TechTalk.SpecFlow;

namespace BpeProducts.Services.Course.Host.Tests.Integration.StepSetups
{
    [Binding]
    public class Givens
    {
        public static IDictionary<string, CourseResource> Courses
        {
            get
            {
                return ScenarioContext.Current.Get<IDictionary<string, CourseResource>>("Courses");
            }
        }

        public static IDictionary<string, ProgramResource> Programs
        {
            get
            {
                return ScenarioContext.Current.Get<IDictionary<string, ProgramResource>>("Programs");
            }
        }

        public static IDictionary<string, CourseSegmentResource> Segments
        {
            get
            {
                return ScenarioContext.Current.Get<IDictionary<string, CourseSegmentResource>>("Segments");
            }
        }

        public static IDictionary<string, LearningOutcomeResource> LearningOutcomes
        {
            get
            {
                return ScenarioContext.Current.Get<IDictionary<string, LearningOutcomeResource>>("LearningOutcomes");
            }
        }

        public Givens()
        {
            ScenarioContext.Current.Add("Courses", new Dictionary<string, CourseResource>());
            ScenarioContext.Current.Add("Programs", new Dictionary<string, ProgramResource>());
            ScenarioContext.Current.Add("Segments", new Dictionary<string, CourseSegmentResource>());
            ScenarioContext.Current.Add("LearningOutcomes", new Dictionary<string, LearningOutcomeResource>());
        }

        [Given(@"I have the following programs")]
        public void GivenIHaveTheFollowingPrograms(Table table)
        {
            foreach (var row in table.Rows)
            {
                var saveProgramRequest = new SaveProgramRequest
                {
                    Description = row["Description"],
                    Name = row["Name"],
                    TenantId = ApiFeature.TenantId,
                    ProgramType = "BA",
                    OrganizationId = Guid.NewGuid()
                };

                var program = PostOperations.CreateProgram(saveProgramRequest);

                var resourceId = row["Name"];
                Programs.Add(resourceId, program);
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
                    TenantId = ApiFeature.TenantId,
                    OrganizationId = new Guid(table.Rows[0]["OrganizationId"]),
                    PrerequisiteCourseIds = new List<Guid>(),
                    CourseType = row.TryGetValue("CourseType", out type) ? (ECourseType)Enum.Parse(typeof(ECourseType), type) : ECourseType.Traditional,
                    IsTemplate = row.TryGetValue("IsTemplate", out isTemplate) && bool.Parse(isTemplate),
                };

                var course = PostOperations.CreateCourse(saveCourseRequest);

                var resourceId = row["Name"];
                Courses.Add(resourceId, course);
            }
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
                    TenantId = ApiFeature.TenantId,
                };

                var course = Courses[courseName];
                
                var parentSegmentName = row["ParentSegmentName"];
                CourseSegmentResource segment = null;
                if (string.IsNullOrWhiteSpace(parentSegmentName))
                {
                    segment = PostOperations.CreateSegment(course, request);
                }
                else
                {
                    var parentSegment = Segments[parentSegmentName];
                    segment = PostOperations.CreateSegment(course, parentSegment, request);
                }

                var resourceId = row["Name"];
                Segments.Add(resourceId, segment);
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
                    TenantId = ApiFeature.TenantId,
                };

                var outcome = PostOperations.CreateLearningOutcome(request);

                var resourceId = row["Description"];
                LearningOutcomes.Add(resourceId, outcome);
            }
        }
    }
}
