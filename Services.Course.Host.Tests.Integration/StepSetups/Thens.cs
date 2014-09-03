using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using BpeProducts.Common.WebApiTest.Extensions;
using BpeProducts.Common.WebApiTest.Framework;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Host.Tests.Integration.Operations;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources.Account;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources.Assessment;
using BpeProducts.Services.Section.Contracts;
using Moq;
using NHibernate.Linq;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace BpeProducts.Services.Course.Host.Tests.Integration.StepSetups
{
    [Binding]
    public class Thens
    {
        private readonly string _leadingPath;

        public Thens()
        {
            var targetUri = new Uri(ApiFeature.BaseAddress);
            if (!targetUri.Host.Equals("localhost"))
            {
                _leadingPath = targetUri.PathAndQuery + "/course";
            }
            else
            {
                _leadingPath = "/course";
            }
        }

        [Then(@"the course count is atleast '(.*)' when search term is '(.*)'")]
        public void ThenTheCourseCountIsAtleastWhenSearchTermIs(int count, string searchPhrase)
        {
            var startsWithQuery = String.IsNullOrWhiteSpace(searchPhrase) ? "" : String.Format("?$filter=startswith(Name, '{0}')", ScenarioContext.Current.Get<long>("ticks") + searchPhrase);
            var result = ApiFeature.CourseTestHost.Client.GetAsync(_leadingPath + startsWithQuery).Result;
            var getResponse = result.Content.ReadAsAsync<IEnumerable<CourseInfoResponse>>().Result;
            var responseList = new List<CourseInfoResponse>(getResponse);
            Assert.That(responseList.Count, Is.AtLeast(count));
        }

        [Then(@"the course name counts are as follows:")]
        public void ThenTheCourseNameCountsAreAsFollows(Table table)
        {
            foreach (var row in table.Rows)
            {
                var operation = row["Operation"];
                var argument = row["Argument"];
                var count = int.Parse(row["Count"]);
                var result = ApiFeature.CourseTestHost.Client.GetAsync(_leadingPath + ConstructODataQueryString(operation, argument, "Name")).Result;
                var getResponse = result.Content.ReadAsAsync<IEnumerable<CourseInfoResponse>>().Result;
                var responseList = new List<CourseInfoResponse>(getResponse);
                Assert.That(responseList.Count, Is.EqualTo(count));
            }
        }

        private string ConstructODataQueryString(string operation, string argument, string fieldName)
        {
            string queryString;

            if (operation.ToLower() == "startswith")
            {
                queryString = String.Format("?$filter={1}({2}, '{0}')",
                                                    String.IsNullOrWhiteSpace(argument)
                                                        ? ""
                                                        : ScenarioContext.Current.Get<long>("ticks") + argument,
                                                    operation, fieldName);
            }
            else if (operation.ToLower() == "eq")
            {
                queryString = String.Format("?$filter={1} eq '{0}'",
                                                    String.IsNullOrWhiteSpace(argument)
                                                        ? ""
                                                        : ScenarioContext.Current.Get<long>("ticks") + argument, fieldName);
            }
            else
            {
                throw new InvalidOperationException(string.Format("Unknown operation: {0}", operation));
            }

            return queryString;
        }

        [Then(@"my course no longer exists")]
        public void ThenMyCourseNoLongerExists()
        {
            var courseName = ScenarioContext.Current["courseName"].ToString();
            var courseRescource = Resources<CourseResource>.Get(courseName);
            var getResponse = ApiFeature.CourseTestHost.Client.GetAsync(_leadingPath + "/" + courseRescource.Id).Result;
            Assert.That(getResponse.StatusCode.Equals(HttpStatusCode.NotFound));
        }

        [Then(@"I can retrieve the course by course name")]
        public void ThenICanRetrieveTheCourseByCourseName()
        {
            var courseName = ScenarioContext.Current.Get<string>("courseName");
            var result = ApiFeature.CourseTestHost.Client.GetAsync(_leadingPath + "?name=" + ScenarioContext.Current.Get<long>("ticks") + courseName).Result;
            result.EnsureSuccessStatusCode();
        }

        [Then(@"I can retrieve the course by course code")]
        public void ThenICanRetrieveTheCourseByCourseCode()
        {
            var courseCode = ScenarioContext.Current.Get<string>("courseCode");
            var result = ApiFeature.CourseTestHost.Client.GetAsync(_leadingPath + "?code=" + ScenarioContext.Current.Get<long>("ticks") + courseCode).Result;
            result.EnsureSuccessStatusCode();
        }

        [Then(@"my course info is changed")]
        public void ThenMyCourseInfoIsChanged()
        {
            var courseName = ScenarioContext.Current["courseName"].ToString();
            var courseRescource = Resources<CourseResource>.Get(courseName);
            var courseInfo = GetOperations.GetCourse(courseRescource);
            var originalRequest = ScenarioContext.Current.Get<UpdateCourseRequest>("editCourseRequest");
            Assert.AreEqual(originalRequest.Name, courseInfo.Name);
            Assert.AreEqual(originalRequest.Code, courseInfo.Code);
            Assert.AreEqual(originalRequest.Description, courseInfo.Description);
            Assert.AreEqual(originalRequest.MetaData, courseInfo.MetaData);
            Assert.AreEqual(originalRequest.CorrelationId, courseInfo.CorrelationId);
            CollectionAssert.AreEquivalent(originalRequest.ExtensionAssets, courseInfo.ExtensionAssets);
        }

        [Then(@"I should get a not found message returned")]
        public void ThenIShouldGetANotFoundMessageReturned()
        {
            var response = ScenarioContext.Current.Get<HttpResponseMessage>("getCourseName");
            var expected = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), "NotFound");
            Assert.That(response.StatusCode, Is.EqualTo(expected));
        }

        [Then(@"the organization id is returned as part of the request")]
        public void ThenTheOrganizationIdIsReturnedAsPartOfTheRequest()
        {
            var orgId = ScenarioContext.Current.Get<Guid>("orgId");
            var courseName = ScenarioContext.Current["courseName"].ToString();
            var courseRescource = Resources<CourseResource>.Get(courseName);
            var courseInfoResponse = GetOperations.GetCourse(courseRescource);

            Assert.That(courseInfoResponse.OrganizationId, Is.Not.Null);
            Assert.That(courseInfoResponse.OrganizationId, Is.EqualTo(orgId));
        }

        [Then(@"the template course id is returned as part of the request")]
        public void ThenTheTemplateCourseIdIsReturnedAsPartOfTheRequest()
        {
            var templateId = ScenarioContext.Current.Get<Guid>("templateId");
            var courseName = ScenarioContext.Current["courseName"].ToString();
            var courseRescource = Resources<CourseResource>.Get(courseName);
            var courseInfoResponse = GetOperations.GetCourse(courseRescource);

            Assert.That(courseInfoResponse.TemplateCourseId, Is.Not.Null);
            Assert.That(courseInfoResponse.TemplateCourseId, Is.EqualTo(templateId));
        }

        [Then(@"the organization '(.*)' has the following programs")]
        public void ThenTheOrganizationHasTheFollowingPrograms(string orgName, Table table)
        {
            var orgResource = Resources<OrganizationResource>.Get(orgName);
            var queryString = string.Format("?$filter=OrganizationId+eq+'{0}'", orgResource.Id);
            var programs = GetOperations.SearchPrograms(queryString);
        }

        [Then(@"the organization with '(.*)' id has the following programs")]
        public void ThenTheOrganizationWithIdHasTheFollowingPrograms(Guid orgId, Table table)
        {
            var queryString = string.Format("?$filter=OrganizationId+eq+'{0}'", orgId.ToString());
            var programs = GetOperations.SearchPrograms(queryString);

            //table.CompareToSet(programs);
        }

        [Then(@"the course '(.*)' should have the following info")]
        public void ThenTheCourseShouldHaveTheFollowingInfo(string courseName, Table table)
        {
            var resource = Resources<CourseResource>.Get(courseName);

            var actual = GetOperations.GetCourse(resource);


            var orgName = table.GetValue("OrganizationName", String.Empty);
          
            if (String.IsNullOrEmpty(orgName)==false)
            {
                var newRows = new Dictionary<string, string>
                    {
                        {"Field", "OrganizationId"},
                        {"Value", Resources<OrganizationResource>.Get(orgName).Id.ToString()}
                    };
                 table.ReplaceRow("Field","OrganizationName",newRows);
           }
           
            table.CompareToInstance(actual);

            if (actual.PublishDate.HasValue)
            {
                Assert.That(actual.PublishDate, Is.InRange(DateTime.Now.AddMinutes(-5), DateTime.Now));
            }
        }

        [Then(@"the course '(.*)' includes the following programs")]
        public void ThenTheCourseIncludesTheFollowingPrograms(string courseName, Table table)
        {
            var resource = Resources<CourseResource>.Get(courseName);

            var course = GetOperations.GetCourse(resource);
            var expectedProgramIds = (from r in table.Rows select Resources<ProgramResource>.Get(r["Program Name"]).Id).ToList();

            CollectionAssert.AreEquivalent(course.ProgramIds, expectedProgramIds);
        }

        [Then(@"the course '(.*)' includes '(.*)' program association")]
        public void ThenTheCourseIncludesProgramAssociation(string courseName, string programName)
        {
            var courseResource = Resources<CourseResource>.Get(courseName);
            var course = GetOperations.GetCourse(courseResource);

            var programResourse = Resources<ProgramResource>.Get(programName);

            Assert.That(course.ProgramIds.Count, Is.EqualTo(1));
            CollectionAssert.Contains(course.ProgramIds, programResourse.Id);
        }

        [Then(@"the course '(.*)' includes the following program information")]
        public void ThenTheCourseIncludesTheFollowingProgramInformation(string courseName, Table table)
        {
            var courseResource = Resources<CourseResource>.Get(courseName);
            var course = GetOperations.GetCourse(courseResource);

            var programIds = (from r in table.Rows select Resources<ProgramResource>.Get(r["Program Name"]).Id).ToList();

            CollectionAssert.AreEquivalent(course.ProgramIds, programIds);
        }

        [Then(@"the course '(.*)' should have the following prerequisites")]
        public void ThenTheCourseShouldHaveTheFollowingPrerequisites(string courseName, Table table)
        {
            var courseResource = Resources<CourseResource>.Get(courseName);
            var course = GetOperations.GetCourse(courseResource);

            var prereqIds = (from r in table.Rows select Resources<CourseResource>.Get(r["Name"]).Id).ToList();

            CollectionAssert.AreEquivalent(prereqIds, course.PrerequisiteCourseIds);
        }
      
        [Then(@"I get '(.*)' response")]
        public void ThenIGetResponse(string status)
        {
            var response = Responses.Last;

            var expectedStatusCode = (HttpStatusCode) Enum.Parse(typeof (HttpStatusCode), status);

            Assert.That(response.StatusCode, Is.EqualTo(expectedStatusCode));
        }

        //TODO: Refactor this code to use "I get '(.*)' response"
        [Then(@"I should get the status code (.*)")]
        public void ThenIShouldGetA(string status)
        {
            var response = ScenarioContext.Current.Get<HttpResponseMessage>("responseToValidate");

            var expectedStatusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), status);

            Assert.That(response.StatusCode.Equals(expectedStatusCode));
        }

        [Then(@"The course '(.*)' should have the template named '(.*)'")]
        public void ThenTheCourseShouldHaveTheTemplateNamed(string courseName, string templateName)
        {
            var courseResource = Resources<CourseResource>.Get(courseName);
            var course = GetOperations.GetCourse(courseResource);
            var templateResource = Resources<CourseResource>.Get(templateName);
            Assert.That(course.TemplateCourseId, Is.EqualTo(templateResource.Id));
        }


        [Then(@"I get the following responses")]
        public void ThenIGetTheFollowingResponses(Table table)
        {
            for (var i = 0; i < table.Rows.Count; i++)
            {
                var result = Responses.All[Responses.All.Count - table.Rows.Count + i];
                var statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), table.Rows[i]["StatusCode"]); 

                Assert.That(result.StatusCode, Is.EqualTo(statusCode));
            }
        }

        [Then(@"my course learning activity '(.*)' contains the following")]
        public void ThenMyCourseLearningActivityContainsTheFollowing(string learningActivityName, Table table)
        {
            var resource = Resources<CourseLearningActivityResource>.Get(learningActivityName);

            var courseLearningActivity = GetOperations.GetCourseLearningActivity(resource);

            var assessmentRow = table.Rows.FirstOrDefault(r => r["Field"] == "Assessment");
            if (assessmentRow != null)
            {
                var assessmentResource = Resources<AssessmentResource>.Get(assessmentRow["Value"]);
                assessmentRow["Field"] = "AssessmentId";
                assessmentRow["Value"] = assessmentResource.Id.ToString();
            }

            table.CompareToInstance(courseLearningActivity);
        }

        [When(@"I get course learning activity '(.*)'")]
        public void WhenIGetCourseLearningActivity(string learningActivityName)
        {
            var resource = Resources<CourseLearningActivityResource>.Get(learningActivityName);
            GetOperations.GetCourseLearningActivity(resource);
        }

        [When(@"I get course learning material '(.*)'")]
        public void WhenIGetCourseLearningMaterial(string learningMaterialName)
        {
            var resource = Resources<LearningMaterialResource>.Get(learningMaterialName);
            GetOperations.GetCourseLearningMaterial(resource);
        }

        [Then(@"the program '(.*)' include the following course information")]
        public void ThenTheProgramIncludeTheFollowingCourseInformation(string programName, Table table)
        {
            var programResource = Resources<ProgramResource>.Get(programName);

            var program = GetOperations.GetProgram(programResource);

            var courseIds = (from r in table.Rows select Resources<CourseResource>.Get(r["Course Name"]).Id).ToList();

            CollectionAssert.AreEquivalent(program.Courses.Select(c => c.Id).ToList(), courseIds);
        }

        [Then(@"the segment '(.*)' should have the following learning activities")]
        public void ThenTheSegmentShouldHaveTheFollowingLearningActivities(string segmentName, Table table)
        {
            var resource = Resources<CourseSegmentResource>.Get(segmentName);
            var segment = GetOperations.GetSegment(resource);

            if (table.ContainsColumn("Assessment"))
            {
                foreach (var row in table.Rows)
                {
                    if (!string.IsNullOrEmpty(row["Assessment"]))
                    {
                        var assessmentResource = Resources<AssessmentResource>.Get(row["Assessment"]);
                        row["Assessment"] = assessmentResource.Id.ToString();
                    }
                    else
                    {
                        row["Assessment"] = null;
                    }
                }
                table.RenameColumn("Assessment", "AssessmentId");
            }
            table.CompareToSet(segment.CourseLearningActivities);

            //var expectedLearningActivities =
            //    (from r in table.Rows select Resources<CourseLearningActivityResource>.Get(r["Name"]).Id).ToList();
            //var actualLearningActivities = (from a in segment.CourseLearningActivities select a.Id).ToList();

            //CollectionAssert.AreEquivalent(expectedLearningActivities, actualLearningActivities);
        }

        [Then(@"the course '(.*)' should have these course segments")]
        public void ThenTheCourseShouldHaveTheseCourseSegments(string courseName, Table table)
        {
            var resource = Resources<CourseResource>.Get(courseName);
            var courseInfo = GetOperations.GetCourse(resource);

            var index = courseInfo.Segments.FlattenTree(c => c.ChildSegments).ToList();

            var traversed = new List<string>();
            foreach (var row in table.Rows)
            {
                var segmentName = row["Name"];

                if(traversed.Contains(segmentName))
                    Assert.Fail("Duplicate segment '{0}' in table", segmentName);
                else 
                    traversed.Add(segmentName);
                
                var actualSegment = index.First(x => x.Name == segmentName);

                var parentSegmentName = row["ParentSegment"];
                if (String.IsNullOrEmpty(parentSegmentName))
                {
                    Assert.That(actualSegment.ParentSegmentId, Is.EqualTo(null));
                }
                else
                {
                    var parentSegment = index.First(x => x.Name == parentSegmentName);

                    Assert.That(actualSegment.ParentSegmentId, Is.EqualTo(parentSegment.Id));
                    Assert.That(parentSegment.ChildSegments.Count(cs => cs.Id == actualSegment.Id), Is.EqualTo(1));
                }

                Assert.That(actualSegment.Description, Is.EqualTo(row["Description"]));
                Assert.That(actualSegment.Type, Is.EqualTo(row["Type"]));
            }

            Assert.That(index.Count, Is.EqualTo(table.Rows.Count));
        }

        [Then(@"The course '(.*)' segments retrieved match the display order entered")]
        public void ThenTheCourseSegmentsRetrievedMatchTheDisplayOrderEntered(string courseName, Table table)
        {
            var courseInfo = GetOperations.GetCourse(Resources<CourseResource>.Get(courseName));

            var index = courseInfo.Segments.FlattenTree(c => c.ChildSegments).ToList();

            foreach (var row in table.Rows)
            {
                var courseSegment = index.First(x => x.Name == row["Name"]);

                Assert.That(courseSegment.DisplayOrder, Is.EqualTo(Convert.ToInt32(row["DisplayOrder"])));
            }
        }

        [Then(@"the course segment '(.*)' should have these children segments")]
        public void ThenTheCourseSegmentShouldHaveTheseChildrenSegments(string parentSegmentName, Table table)
        {
            var parentSegmentResource = Resources<CourseSegmentResource>.Get(parentSegmentName);
            var parentSegment = GetOperations.GetSegment(parentSegmentResource);

            Assert.That(parentSegment.ChildSegments.Count, Is.EqualTo(table.Rows.Count));
            foreach (var row in table.Rows)
            {
                Assert.That(parentSegment.ChildSegments.Any(
                    s => s.Name == row["Name"] && s.Description == row["Description"] && s.Type == row["Type"]));
            }
        }

        [Then(@"the course template Ids in ""(.*)"" are:")]
        public void ThenTheCourseTemplateIdsInAre(string scenarioContextName, Table table)
        {
            var actualIds = ScenarioContext.Current[scenarioContextName].As<List<string>>();
            table.CompareToSet(actualIds);
        }

        [Then(@"the program '(.*)' contains")]
        public void ThenTheProgramContains(string programName, Table table)
        {
            var resource = Resources<ProgramResource>.Get(programName);
            var program = GetOperations.GetProgram(resource);

            table.CompareToInstance(program);
        }

        [Then(@"I have the following programs")]
        public void ThenIHaveTheFollowingPrograms(Table table)
        {
            var expectedPrograms = table.CreateSet<ProgramResponse>().OrderBy(p => p.Name);
            var actualPrograms = GetOperations.GetAllPrograms();
            var isValidatePublishStatus = table.ContainsColumn("IsPublished");

            foreach (var expectedProgram in expectedPrograms)
            {
                var expectedId = Resources<ProgramResource>.Get(expectedProgram.Name).Id;
                var actualProgram = actualPrograms.First(p => p.Id == expectedId);
                var expectedOrgId = expectedProgram.OrganizationId == Guid.Empty
                                        ? Resources<OrganizationResource>.Get("Default").Id
                                        : expectedProgram.OrganizationId;

                Assert.That(actualProgram.Description, Is.EqualTo(expectedProgram.Description));
                Assert.That(actualProgram.OrganizationId, Is.EqualTo(expectedOrgId));
                Assert.That(actualProgram.ProgramType, Is.EqualTo(expectedProgram.ProgramType));
                if (isValidatePublishStatus)
                {
                    Assert.That(actualProgram.IsPublished, Is.EqualTo(expectedProgram.IsPublished));
                    Assert.That(actualProgram.PublishNote, Is.EqualTo(expectedProgram.PublishNote));
                    Assert.That(actualProgram.VersionNumber, Is.EqualTo(expectedProgram.VersionNumber));
                }
            }
        }


        [Then(@"the course '(.*)' should have these course segments in the following order")]
        public void ThenTheCourseShouldHaveTheseCourseSegmentsInTheFollowingOrder(string courseName, Table table)
        {
            var resource = Resources<CourseResource>.Get(courseName);
            var courseInfo = GetOperations.GetCourse(resource);

            var courseSegments = courseInfo.Segments;
            var uncollapsedSegments = new List<CourseSegmentInfo>();

            var index = courseInfo.Segments.FlattenTree(c => c.ChildSegments).ToList();

            foreach (var segment in courseSegments)
            {
                uncollapsedSegments.Add(segment);
                if (segment.ChildSegments.Count > 0)
                {
                    foreach (var childSegment in segment.ChildSegments)
                    {
                        uncollapsedSegments.Add(childSegment);
                        if (segment.ChildSegments.Count > 0)
                        {
                            uncollapsedSegments.AddRange(childSegment.ChildSegments);
                        }
                    }
                }
            }

            Assert.That(table.RowCount, Is.EqualTo(uncollapsedSegments.Count));

            for (int i = 0; i < uncollapsedSegments.Count; i++)
            {
                Assert.That(table.Rows[i]["Name"], Is.EqualTo(uncollapsedSegments[i].Name));
                if (table.Rows[i]["ParentSegment"] != string.Empty)
                {
                    Assert.That(uncollapsedSegments[i].ParentSegmentId, Is.Not.Null);
                    Assert.That(index.First(x => x.Id == uncollapsedSegments[i].ParentSegmentId).Name, Is.EqualTo(table.Rows[i]["ParentSegment"]));
                }
            }
        }

        [Then(@"published courses for orgniazation '(.*)' contains the following courses")]
        public void ThenPublishedCoursesForOrgniazationContainsTheFollowingCourses(string organization, Table table)
        {
            var orgResource = Resources<OrganizationResource>.Get(organization);
            var courses = GetOperations.GetPublishedCourses(orgResource).ToList();

            foreach (var row in table.Rows)
            {
                var courseResource = Resources<CourseResource>.Get(row["Name"]);
                Assert.That(courses.Any(c => c.Id == courseResource.Id), Is.True);
            }
        }

        [Then(@"published courses for orgniazation '(.*)' does not contain the following courses")]
        public void ThenPublishedCoursesForOrgniazationDoesNotContainTheFollowingCourses(string organization, Table table)
        {
            var orgResource = Resources<OrganizationResource>.Get(organization);
            var courses = GetOperations.GetPublishedCourses(orgResource).ToList();

            foreach (var row in table.Rows)
            {
                var courseResource = Resources<CourseResource>.Get(row["Name"]);
                Assert.That(courses.Count(c => c.Id == courseResource.Id), Is.EqualTo(0));
            }
        }

        [Then(@"The '(.*)' learning material has the following info")]
        public void ThenTheLearningMaterialHasTheFollowingInfo(string materialName, Table table)
        {
            var isCourseSegmentLearningMaterial = table.ContainsColumn("CourseSegment");
            var materialResource = Resources<LearningMaterialResource>.Get(materialName);
            var response = GetOperations.GetCourseLearningMaterial(materialResource);
            foreach (var row in table.Rows)
            {
                var assetName = row["Asset"];
                var asset = Resources<AssetResource>.Get(assetName);
                Assert.That(response.AssetId, Is.EqualTo(asset.Id));
                Assert.That(response.Instruction, Is.EqualTo(row["Instruction"]));
                Assert.That(response.IsRequired, Is.EqualTo(bool.Parse(row["IsRequired"])));
                if (isCourseSegmentLearningMaterial)
                {
                    var segmentName = row["CourseSegment"];
                    var courseSegment = Resources<CourseSegmentResource>.Get(segmentName);
                    Assert.That(response.CourseSegmentId, Is.EqualTo(courseSegment.Id));
                }
                else
                {
                    var courseName = row["Course"];
                    var course = Resources<CourseResource>.Get(courseName);
                    Assert.That(response.CourseId, Is.EqualTo(course.Id));
                }

            }
        }

        [Then(@"The following learning materials have the following info")]
        public void ThenTheFollowingLearningMaterialsHaveTheFollowingInfo(Table table)
        {
            foreach (var row in table.Rows)
            {
                var materialName = row["LearningMaterial"];
                var materialResource = Resources<LearningMaterialResource>.Get(materialName);
                var response = GetOperations.GetCourseLearningMaterial(materialResource);
                var segmentName = row["CourseSegment"];
                var courseSegment = Resources<CourseSegmentResource>.Get(segmentName);
                var assetName = row["Asset"];
                var asset = Resources<AssetResource>.Get(assetName);
                Assert.That(response.AssetId, Is.EqualTo(asset.Id));
                Assert.That(response.CourseSegmentId, Is.EqualTo(courseSegment.Id));
                Assert.That(response.Instruction, Is.EqualTo(row["Instruction"]));
                Assert.That(response.IsRequired, Is.EqualTo(bool.Parse(row["IsRequired"])));
                Assert.That(response.MetaData, Is.EqualTo(row["MetaData"]));
            }
        }

        [Then(@"The course '(.*)' has following learning material")]
        public void ThenTheCourseHasFollowingLearningMaterial(string courseName, Table table)
        {
            var courseResource = Resources<CourseResource>.Get(courseName);
            var course = GetOperations.GetCourse(courseResource);
            foreach (var row in table.Rows)
            {
                var segmentName = row["CourseSegment"];
                var isCourseLearningMaterial = segmentName.Equals(string.Empty);
                var assetName = row["Asset"];
                var asset = Resources<AssetResource>.Get(assetName);
                var parentCourseResource = Resources<CourseResource>.Get(row["ParentCourse"]);
                var parentCourse = GetOperations.GetCourse(parentCourseResource);
                var segment = new CourseSegmentInfo();
                LearningMaterialInfo parentCourseLearningMaterial;
                LearningMaterialInfo learningMaterial;
                if (isCourseLearningMaterial)
                {
                    learningMaterial = course.LearningMaterials.First(l => l.AssetId == asset.Id);
                    parentCourseLearningMaterial = parentCourse.LearningMaterials.First(l => l.AssetId == asset.Id);
                    Assert.That(course.LearningMaterials.Count, Is.EqualTo(parentCourse.LearningMaterials.Count));

                }
                else
                {
                    var segments = course.Segments.FlattenTree(c => c.ChildSegments).ToList();
                    segment = segments.First(s => s.Name == segmentName);
                    learningMaterial = segment.LearningMaterials.First(l => l.AssetId == asset.Id);
                    var parentCourseSegments = parentCourse.Segments.FlattenTree(c => c.ChildSegments).ToList();
                    var parentCourseSegment = parentCourseSegments.First(s => s.Name == segmentName);
                    parentCourseLearningMaterial = parentCourseSegment.LearningMaterials.First(l => l.AssetId == asset.Id);
                    Assert.That(segment.LearningMaterials.Count, Is.EqualTo(parentCourseSegment.LearningMaterials.Count));
                }
                Assert.That(learningMaterial.Id, Is.Not.EqualTo(parentCourseLearningMaterial.Id));
                Assert.That(learningMaterial.AssetId, Is.EqualTo(asset.Id));
                if (isCourseLearningMaterial)
                {
                    Assert.That(learningMaterial.CourseSegmentId, Is.EqualTo(null));
                }
                else
                {
                    Assert.That(learningMaterial.CourseSegmentId, Is.EqualTo(segment.Id));
                    Assert.That(learningMaterial.CourseId, Is.EqualTo(course.Id));
                }
                Assert.That(learningMaterial.Instruction, Is.EqualTo(row["Instruction"]));
                Assert.That(learningMaterial.IsRequired, Is.EqualTo(bool.Parse(row["IsRequired"])));
            }
        }

        [Then(@"The asset '(.*)' is published")]
        public void ThenTheAssetIsPublished(string assetName)
        {
            var asset = Resources<AssetResource>.Get(assetName);
            var response = ApiFeature.MockAssetClient.Object.GetAsset(asset.Id);
            Assert.That(response.IsPublished, Is.EqualTo(true));
        }

        [Then(@"the course '(.*)' should have the following learning outcomes")]
        public void ThenTheCourseShouldHaveTheFollowingLearningOutcomes(string courseName, Table table)
        {
            var courseResource = Resources<CourseResource>.Get(courseName);
            foreach (var row in table.Rows)
            {
                var outcomeInfo = Resources<IOutcomeInfoResource>.Get(row["Description"]);
                Assert.That(outcomeInfo, Is.Not.Null);
                Assert.That(outcomeInfo.Title, Is.EqualTo(row["Title"]));
                var supportedOutcomes = outcomeInfo.SupportedOutcomes;
                Assert.That(supportedOutcomes.Count, Is.EqualTo(1));
                foreach (var outcome in supportedOutcomes)
                {
                    Assert.That(outcome.Id,Is.EqualTo(courseResource.Id));
                }
            }
        }

        [Then(@"the course '(.*)' should have the following reference info")]
        public void ThenTheCourseShouldHaveTheFollowingReferenceInfo(string courseName, Table table)
        {
            var resource = Resources<CourseResource>.Get(courseName);
            var actual = GetOperations.GetCourse(resource);
            
            //var extensionAssets = string.IsNullOrWhiteSpace(table.GetValue("ExtensionAssets", null)) ? null : Array.ConvertAll(table.GetValue("ExtensionAssets", null).Split(','), s => new Guid(s)).ToList();
            
            var assetNames = table.Rows.Single(r => r["Field"] == "ExtensionAssets")["Value"].Split(',').ToList();
            var guidList = new List<Guid>();
            guidList.AddRange(assetNames.Select(a => Resources<AssetResource>.Get(a).Id));
            var extensionAssets = guidList;

            CollectionAssert.AreEquivalent(extensionAssets, actual.ExtensionAssets);
        }

        [Then(@"The course '(.*)' has '(.*)' course learning material")]
        public void ThenTheCourseHasCourseLearningMaterial(string courseName, int count)
        {
            var courseResource = Resources<CourseResource>.Get(courseName);
            var course = GetOperations.GetCourse(courseResource);
            Assert.That(course.LearningMaterials.Count, Is.EqualTo(count));
        }

     
        [Then(@"TemplateCourseId should be null in the course '(.*)'")]
        public void ThenTemplateCourseIdShouldBeNullInTheCourse(string courseName)
        {
            var courseResource = Resources<CourseResource>.Get(courseName);
            var course = GetOperations.GetCourse(courseResource);
            Assert.That(course.TemplateCourseId, Is.Null);
        }

        [Then(@"The course '(.*)' has following CorrelationId")]
        public void ThenTheCourseHasFollowingCorrelationId(string courseName, Table table)
        {
            var courseResource = Resources<CourseResource>.Get(courseName);
            var course = GetOperations.GetCourse(courseResource);
            var expectCorrelationId = ScenarioContext.Current.Get<long>("ticks") + table.Rows[0]["CorrelationId"];
            Assert.That(course.CorrelationId, Is.EqualTo(expectCorrelationId));
        }

        [Then(@"the course correlationId counts are as follows:")]
        public void ThenTheCourseCorrelationIdCountsAreAsFollows(Table table)
        {
            foreach (var row in table.Rows)
            {
                var operation = row["Operation"];
                var argument = row["Argument"];
                var count = int.Parse(row["Count"]);
                var result = ApiFeature.CourseTestHost.Client.GetAsync(_leadingPath + ConstructODataQueryString(operation, argument, "CorrelationId")).Result;
                var getResponse = result.Content.ReadAsAsync<IEnumerable<CourseInfoResponse>>().Result;
                var responseList = new List<CourseInfoResponse>(getResponse);
                Assert.That(responseList.Count, Is.EqualTo(count));
            }
        }

    }
}

