using System.Net.Http.Formatting;
using BpeProducts.Common.Exceptions;
using BpeProducts.Common.WebApiTest.Extensions;
using BpeProducts.Services.Asset.Contracts;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Host.Tests.Integration.Operations;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources.Account;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources.Assessment;
using BpeProducts.Services.Section.Contracts;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using BpeProducts.Common.WebApiTest.Framework;
using CourseInfoResponse = BpeProducts.Services.Course.Contract.CourseInfoResponse;
using PublishRequest = BpeProducts.Services.Course.Contract.PublishRequest;

namespace BpeProducts.Services.Course.Host.Tests.Integration.StepSetups
{
    [Binding]
    public class Whens
    {
        private readonly string _leadingPath;

        public Whens()
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

        [BeforeScenario]
        public void BeforeScenario()
        {
            ScenarioContext.Current.Add("ticks", DateTime.Now.Ticks);
        }

        [When(@"I create the following course using a previous course template:")]
        public void WhenICreateTheFollowingCourseUsingAPreviousCourseTemplate(Table table)
        {
            var course = ScenarioContext.Current.Get<SaveCourseRequest>("createCourseRequest");
            var courseWithTemplateId = new SaveCourseRequest
            {
                Name = table.Rows[0]["Name"],
                Code = table.Rows[0]["Code"],
                Description = table.Rows[0]["Description"],
                TenantId = course.TenantId,
                CourseType = ECourseType.Traditional,
                IsTemplate = false
            };

            ScenarioContext.Current.Add("courseTemplate", courseWithTemplateId);

            var response = ApiFeature.CourseTestHost.Client.PostAsync(_leadingPath, courseWithTemplateId, new JsonMediaTypeFormatter()).Result;
            response.EnsureSuccessStatusCode();

            ScenarioContext.Current.Add("ResponseToValidate", response);
        }

        [When(@"I create a new course with (.*), (.*), (.*), (.*)")]
        public void WhenICreateANewCourseWith(string name, string code, string description, string organizationName)
        {
            var saveCourseRequest = new SaveCourseRequest
            {
                Name = string.IsNullOrEmpty(name) ? name : ScenarioContext.Current.Get<long>("ticks") + name,
                Code = string.IsNullOrEmpty(code) ? code : ScenarioContext.Current.Get<long>("ticks") + code,
                Description = description,
                TenantId = 999999,
                OrganizationId = Resources<OrganizationResource>.Get(organizationName).Id,
                CourseType = ECourseType.Traditional,
                IsTemplate = false
            };

            if (ScenarioContext.Current.ContainsKey("createCourseRequest"))
            {
                ScenarioContext.Current.Remove("createCourseRequest");
            }
            ScenarioContext.Current.Add("createCourseRequest", saveCourseRequest);
        }

        [When(@"I request a course name that does not exist")]
        public void WhenIRequestACourseNameThatDoesNotExist()
        {
            var courseName = "someCoureName";
            var result = ApiFeature.CourseTestHost.Client.GetAsync(_leadingPath + "?name=" + ScenarioContext.Current.Get<long>("ticks") + courseName).Result;

            ScenarioContext.Current.Add("getCourseName", result);
        }

        [When(@"I change the info to reflect the following:")]
        public void WhenIChangeTheInfoToReflectTheFollowing(Table table)
        {
            var editCourseRequest = new SaveCourseRequest
            {
                Name = ScenarioContext.Current.Get<long>("ticks") + table.Rows[0]["Name"],
                Code = ScenarioContext.Current.Get<long>("ticks") + table.Rows[0]["Code"],
                Description = table.Rows[0]["Description"],
                TenantId = int.Parse(table.Rows[0]["Tenant Id"]),
                OrganizationId = Resources<OrganizationResource>.Get(table.Rows[0]["OrganizationName"]).Id,
                CourseType = ECourseType.Traditional,
                IsTemplate = false,
                Credit = decimal.Parse(table.Rows[0].GetValue("Credit", "0"))
            };

            ScenarioContext.Current.Add("editCourseRequest", editCourseRequest);
            var response = ScenarioContext.Current.Get<HttpResponseMessage>("createCourseResponse");
            var courseInfoResponse = response.Content.ReadAsAsync<CourseInfoResponse>().Result;

            var result = ApiFeature.CourseTestHost.Client.PutAsync(_leadingPath + "/" + courseInfoResponse.Id, editCourseRequest, new JsonMediaTypeFormatter()).Result;
            ScenarioContext.Current.Add("editCourseResponse", result);
            ScenarioContext.Current.Add("courseId", courseInfoResponse.Id);

            // this is the response to ensure the success code
            if (ScenarioContext.Current.ContainsKey("responseToValidate"))
            {
                ScenarioContext.Current.Remove("responseToValidate");
            }
            ScenarioContext.Current.Add("responseToValidate", result);
        }

        [When(@"I submit a creation request")]
        public void WhenISubmitACreationRequest()
        {
            var saveCourseRequest = ScenarioContext.Current.Get<SaveCourseRequest>("createCourseRequest");
            var response = ApiFeature.CourseTestHost.Client.PostAsync(_leadingPath, saveCourseRequest, new JsonMediaTypeFormatter()).Result;

            if (ScenarioContext.Current.ContainsKey("createCourseResponse"))
            {
                ScenarioContext.Current.Remove("createCourseResponse");
            }
            ScenarioContext.Current.Add("createCourseResponse", response);

            // this is the response to ensure the success code
            if (ScenarioContext.Current.ContainsKey("responseToValidate"))
            {
                ScenarioContext.Current.Remove("responseToValidate");
            }
            ScenarioContext.Current.Add("responseToValidate", response);
        }

        [When(@"I create a course from the template '(.*)' with the following")]
        public HttpResponseMessage WhenICreateACourseFromTheTemplateWithTheFollowing(string templateName, Table table)
        {
           return CreateCourseTemplate(templateName, table);
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

            PutOperations.AssociateCourseWithPrograms(course, new List<ProgramResource> { program });
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

            PutOperations.DisassociateCourseWithPrograms(course, new List<ProgramResource> { program });
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
            var supportingOutcomes = (from o in Resources<LearningOutcomeResource>.All
                                      where descriptions.Contains(o.Key)
                                      select o.Value).ToList();

            foreach (var supportingOutcome in supportingOutcomes)
            {
                DeleteOperations.OutcomeDoesNotSupportLearningOutcome(supportingOutcome, supportedOutcome);
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
                DeleteOperations.ProgramDoesNotSupportLearningOutcome(programResource, learningOutcome);
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
            var assessmentRow=table.Rows.SingleOrDefault(r => r["Field"] == "Assessment");
            if (assessmentRow != null)
            {
                var assessment = Resources<AssessmentResource>.Get(assessmentRow["Value"]);
                request.AssessmentId = assessment.Id;
            }
            PostOperations.CreateCourseLearningActivity(request.Name, segment, request);
        }

        [When(@"I retrieve the course learning activity '(.*)'")]
        public void WhenIRetrieveTheCourseLearningActivity(string activityName)
        {
            var resource = Resources<CourseLearningActivityResource>.Get(activityName);
            GetOperations.GetCourseLearningActivity(resource);
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
            var assessmentRow = table.Rows.SingleOrDefault(r => r["Field"] == "Assessment");
            if (assessmentRow != null)
            {
                if (assessmentRow["Value"] == "Non-Existing")
                {
                    learningActivity.AssessmentId = Guid.NewGuid();
                    ApiFeature.MockAssessmentClient.Setup(a => a.GetAssessment(learningActivity.AssessmentId))
                              .Throws(new NotFoundException("assessment not found"));
                }
                else
                {
                    var assessment = Resources<AssessmentResource>.Get(assessmentRow["Value"]);
                    learningActivity.AssessmentId = assessment.Id;
                }
            }

            PutOperations.UpdateCourseLearningActivity(resourse, learningActivity);
        }

        [When(@"I view '(.*)' course")]
        [When(@"I retrieve '(.*)' course")]
        public void WhenIRetrieveCourse(string courseName)
        {
            var resource = Resources<CourseResource>.Get(courseName);
            GetOperations.GetCourse(resource);
        }

        [When(@"I update '(.*)' course with the following info")]
        public void WhenIUpdateCourseWithTheFollowingInfo(string courseName, Table table)
        {
            var course = Resources<CourseResource>.Get(courseName);
            var updateCourseRequest = table.CreateInstance<UpdateCourseRequest>();

            PutOperations.UpdateCourse(course, updateCourseRequest);
        }

        [When(@"Create a new version of '(.*)' course named '(.*)' with the following info")]
        [When(@"I create a new version of '(.*)' course named '(.*)' with the following info")]
        public void WhenICreateANewVersionOfCourseNamedWithTheFollowingInfo(string courseName, string newVersionName, Table table)
        {
            var request = table.CreateInstance<Contract.VersionRequest>();

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

        [When(@"I create a course without a version")]
        public void WhenICreateACourseWithoutAVersion()
        {
            var versionRequest = new Contract.VersionRequest
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
            GetOperations.GetProgram(resource);
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
                    OrganizationId = Resources<OrganizationResource>.Get(row["OrganizationName"]).Id
                };

                PostOperations.CreateProgram(saveProgramRequest.Name, saveProgramRequest);
            }
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

        [When(@"I perform a bulk update for '(.*)' with the following order")]
        public void WhenIPerformABulkUpdateForWithTheFollowingOrder(string courseName, Table table)
        {
            // Get Math 101 request from dictionary
            var course = Resources<CourseResource>.Get(courseName);
            var courseInfo = GetOperations.GetCourse(course);
            var allSegments = courseInfo.Segments.FlattenTree(c => c.ChildSegments).ToList();

            var requestDictionary = new Dictionary<Guid, UpdateCourseSegmentRequest>();
            foreach (var row in table.Rows)
            {
                var courseSegmentInfo = allSegments.First(s => s.Name == row["Name"]);

                var request = new UpdateCourseSegmentRequest
                    {
                        Id = courseSegmentInfo.Id,
                        Name = courseSegmentInfo.Name,
                        Type = courseSegmentInfo.Type,
                        Description = courseSegmentInfo.Description
                    };

                requestDictionary.Add(courseSegmentInfo.Id, request);

                if (!string.IsNullOrEmpty(row["ParentSegment"]))
                {
                    var parentSegmentInfo = allSegments.First(s => s.Name == row["ParentSegment"]);
                    var parentSegmentInTheDictionary = requestDictionary[parentSegmentInfo.Id];

                    request.ParentSegmentId = parentSegmentInfo.Id;
                    parentSegmentInTheDictionary.ChildrenSegments.Add(request);
                }
            }

            // Now we have the dictionary, which has all the segments, with children segments associated with the parent segments
            // Get the top-level course segments
            var topSegments = requestDictionary.Values.Where(x => x.ParentSegmentId == null).ToList();

            PutOperations.UpdateBulkCourseSegments(course, topSegments);
        }

        [When(@"I create the course")]
        public void WhenICreateTheCourse(Table table)
        {
            var request = table.CreateInstance<SaveCourseRequest>();
            var organization = Resources<OrganizationResource>.Get(table.GetValue("OrganizationName", null));
            request.OrganizationId = organization.Id;

            PostOperations.CreateCourse(request.Name, request);
        }

        [When(@"I get the course '(.*)'")]
        public void WhenIGetTheCourse(string courseName)
        {
            var resource = Resources<CourseResource>.Get(courseName);
            GetOperations.GetCourse(resource);
        }

        [When(@"I create the following sections")]
        public void WhenICreateTheFollowingSections(Table table)
        {
            foreach (var row in table.Rows)
            {
                var course = Resources<CourseResource>.Get(row["CourseName"]);
                var uri = new Uri("http://mockedlocation/");

                var request = new CourseSectionRequest
                {
                    SectionServiceUri = uri,
                    Name = row["Name"],
                    CourseCode = row["CourseCode"],
                    SectionCode = row["SectionCode"],
                    StartDate = row.GetValue("StartDate", DateTime.MinValue),
                    EndDate = row.GetValue<DateTime?>("EndDate", null),
					OrganizationId = Guid.NewGuid()
                };

                PostOperations.CreateSection(request.Name, course, request);
            }
        }

        [When(@"the section service returns '(.*)'")]
        public void WhenTheSectionServiceReturns(string statusCode)
        {
            var uri = new Uri("http://mockedlocation/");
            var code = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), statusCode, true);

            var response = new HttpResponseMessage(code);
            response.Headers.Location = new Uri(uri, Guid.NewGuid().ToString());
            ApiFeature.MockSectionClient.Setup(s => s.CreateSection(It.IsAny<Uri>(), It.IsAny<CreateSectionRequest>())).Returns(response);
            ApiFeature.MockSectionClient.Setup(s => s.CreateSection(It.IsAny<CreateSectionRequest>())).Returns(response);
        }

        [When(@"Create learning material as the following info")]
        public void WhenCreateLearningMaterialAsTheFollowingInfo(Table table)
        {
            var isCourseSegmentLearningMaterial = table.ContainsColumn("CourseSegment");

            foreach (var row in table.Rows)
            {
                var assetName = row["Asset"];
                var asset = Resources<AssetResource>.Get(assetName);
                var learningMaterial = row["LearningMaterial"];
                var request = new LearningMaterialRequest
                {
                    AssetId = asset.Id,
                    Instruction = row["Instruction"],
                    IsRequired = bool.Parse(row["IsRequired"]),
                    CustomAttribute  = row["CustomAttribute"]
                };

                if (isCourseSegmentLearningMaterial)
                {
                    var segmentName = row["CourseSegment"];
                    var courseSegment = Resources<CourseSegmentResource>.Get(segmentName);
                    PostOperations.CreateCourseSegmentLearningMaterial(learningMaterial, courseSegment, request);
                }
                else
                {
                    var courseName = row["Course"];
                    var course = Resources<CourseResource>.Get(courseName);
                    PostOperations.CreateCourseLearningMaterial(learningMaterial, course, request);
                }
            }
        }

        [When(@"Update '(.*)' learning material as the following info")]
        public void WhenUpdateLearningMaterialAsTheFollowingInfo(string materialName, Table table)
        {
            var learningMaterialResource = Resources<LearningMaterialResource>.Get(materialName);
            foreach (var row in table.Rows)
            {
                var assetName = row["Asset"];
                var asset = Resources<AssetResource>.Get(assetName);
                var request = new UpdateLearningMaterialRequest
                {
                    AssetId = asset.Id,
                    Instruction = row["Instruction"],
                    IsRequired = bool.Parse(row["IsRequired"])

                };

                PutOperations.UpdateLearningMaterial(learningMaterialResource, request);
            }
        }

        [When(@"I remove '(.*)' learning material")]
        public void WhenIRemoveLearningMaterial(string materialName)
        {
            var resource = Resources<LearningMaterialResource>.Get(materialName);
            DeleteOperations.DeleteResource(resource);
        }

        [When(@"I retrieve the learning material '(.*)'")]
        public void WhenIRetrieveTheLearningMaterial(string materialName)
        {
            var resource = Resources<LearningMaterialResource>.Get(materialName);
            GetOperations.GetCourseLearningMaterial(resource);
        }

        [When(@"Create section as following")]
        public void WhenCreateSectionAsFollowing(Table table)
        {
            foreach (var row in table.Rows)
            {
                var course = Resources<CourseResource>.Get(row["CourseName"]);
                var uri = new Uri("http://mockedlocation/");

                var request = new CourseSectionRequest
                {
                    SectionServiceUri = uri,
                    Name = row["Name"],
                    CourseCode = row["Code"],
                    SectionCode = row["Code"],
                    StartDate = row.GetValue("StartDate", DateTime.MinValue),
                    EndDate = row.GetValue<DateTime?>("EndDate", null),
                    OrganizationId = Resources<OrganizationResource>.Get(row["OrganizationName"]).Id
                };

                PostOperations.CreateSection(request.Name, course, request);
            }
        }

        [When(@"Create a course from the template '(.*)' with the following")]
        public void WhenCreateACourseFromTheTemplateWithTheFollowing(string templateName, Table table)
        {
            var template = Resources<CourseResource>.Get(templateName);
            foreach (var row in table.Rows)
            {
                string isTemplate;

                var request = new CreateCourseFromTemplateRequest
                {
                    Code = row["Code"],
                    Description = row["Description"],
                    Name = row["Name"],
                    OrganizationId = Resources<OrganizationResource>.Get(row["OrganizationName"]).Id,
                    IsTemplate = row.TryGetValue("IsTemplate", out isTemplate) && bool.Parse(isTemplate),
                    TemplateCourseId = template.Id
                };

                var result = PostOperations.CreateCourseFromTemplate(request.Name, request);
                result.EnsureSuccessStatusCode();
            }
        }

        [When(@"Publish the following courses")]
        public void WhenPublishTheFollowingCourses(Table table)
        {
            foreach (var row in table.Rows)
            {
                var course = Resources<CourseResource>.Get(row["CourseName"]);

                var request = new Contract.PublishRequest
                {
                    PublishNote = row["Note"]
                };

                PutOperations.PublishCourse(course, request);
            }
        }

        [When(@"Publish the following courses with '(.*)' asset does not publish")]
        public void WhenPublishTheFollowingCoursesWithAssetDoesNotPublishing(string assetName, Table table)
        {
            foreach (var row in table.Rows)
            {
                var course = Resources<CourseResource>.Get(row["CourseName"]);
                var asset = Resources<AssetResource>.Get(assetName);
                var request = new Contract.PublishRequest
                {
                    PublishNote = row["Note"]
                };
                ApiFeature.MockAssetClient.Setup(x => x.PublishAsset(asset.Id, row["Note"]));
                ApiFeature.MockAssetClient.Setup(x => x.GetAsset(asset.Id)).Returns(new AssetInfo
                {
                    Id = asset.Id,
                    IsPublished = true
                });
                PutOperations.PublishCourse(course, request);
            }
        }

        private static HttpResponseMessage CreateCourseTemplate(string templateName, Table table)
        {
            var template = Resources<CourseResource>.Get(templateName);
            var courseRequest = table.CreateInstance<CreateCourseFromTemplateRequest>();
            var organizationName = table.Rows[0]["OrganizationName"];
            courseRequest.OrganizationId = Resources<OrganizationResource>.GetId(organizationName);
            courseRequest.TemplateCourseId = template.Id;

            var result = PostOperations.CreateCourseFromTemplate(courseRequest.Name, courseRequest);
            return result;
        }
    }
}
