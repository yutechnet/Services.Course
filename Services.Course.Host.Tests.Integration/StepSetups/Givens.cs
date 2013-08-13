using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Host.Tests.Integration.Operations;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

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

        public static IDictionary<string, CourseLearningActivityResource> CourseLearningActivities
        {
            get
            {
                return ScenarioContext.Current.Get<IDictionary<string, CourseLearningActivityResource>>("CourseLearningActivities");
            }
        }

        public Givens()
        {
            ScenarioContext.Current.Add("Courses", new Dictionary<string, CourseResource>());
            ScenarioContext.Current.Add("Programs", new Dictionary<string, ProgramResource>());
            ScenarioContext.Current.Add("Segments", new Dictionary<string, CourseSegmentResource>());
            ScenarioContext.Current.Add("LearningOutcomes", new Dictionary<string, LearningOutcomeResource>());
            ScenarioContext.Current.Add("CourseLearningActivities", new Dictionary<string, CourseLearningActivityResource>());
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

                PostOperations.CreateProgram(saveProgramRequest.Name, saveProgramRequest);
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

                var request = new SaveCourseRequest
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

                PostOperations.CreateCourse(request.Name, request);
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
                var parentSegmentName = row["ParentSegment"];

                if (string.IsNullOrWhiteSpace(parentSegmentName))
                {
                    PostOperations.CreateSegment(request.Name, course, request);
                }
                else
                {
                    var parentSegment = Segments[parentSegmentName];
                    PostOperations.CreateSegment(request.Name, course, parentSegment, request);
                }
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

                PostOperations.CreateLearningOutcome(request.Description, request);
            }
        }

        [Given(@"I add the following course learning activities to '(.*)' course segment")]
        public void GivenIAddTheFollowingCourseLearningActivitiesToCourseSegment(string segmentName, Table table)
        {
            var learningActivityRequests = table.CreateSet<SaveCourseLearningActivityRequest>();
            var segment = Segments[segmentName];

            foreach (var request in learningActivityRequests)
            {
                request.TenantId = ApiFeature.TenantId;

                PostOperations.CreateCourseLearningActivity(request.Name, segment, request);
            }
        }

        [Given(@"I associate the newly created learning outcomes to '(.*)' program")]
        public void WhenIAssociateTheNewlyCreatedLearningOutcomesToProgram(string programName, Table table)
        {
            var requests = table.CreateSet<OutcomeRequest>();
            var program = Programs[programName];

            foreach (var request in requests)
            {
                request.TenantId = ApiFeature.TenantId;
                PostOperations.CreateEntityLearningOutcome(request.Description, "program", program.Id, request);
            }
        }
    }
}
