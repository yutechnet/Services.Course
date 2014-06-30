using System.Net.Http;
using System.Net.Http.Formatting;
using BpeProducts.Common.Ioc.Extensions;
using BpeProducts.Common.WebApiTest.Extensions;
using BpeProducts.Services.Asset.Contracts;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Host.Tests.Integration.Operations;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources.Account;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources.Assessment;
using BpeProducts.Common.WebApiTest.Framework;
using Moq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace BpeProducts.Services.Course.Host.Tests.Integration.StepSetups
{
    [Binding]
    public class Givens
    {
        private readonly string _leadingPath;

        public Givens()
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

        [Given(@"I have existing courses with following info:")]
        public void GivenIHaveExistingCoursesWithFollowingInfo(Table table)
        {
            // This is creating a course for us.
            foreach (var row in table.Rows)
            {
                var saveCourseRequest = new SaveCourseRequest
                {
                    Name = ScenarioContext.Current.Get<long>("ticks") + row["Name"],
                    Code = ScenarioContext.Current.Get<long>("ticks") + row["Code"],
                    Description = row["Description"],
                    TenantId = int.Parse(table.Rows[0]["Tenant Id"]),
                    OrganizationId = Resources<OrganizationResource>.Get(table.Rows[0]["OrganizationName"]).Id,
                    CourseType = ECourseType.Traditional,
                    IsTemplate = false
                };

                var response = ApiFeature.CourseTestHost.Client.PostAsync(_leadingPath, saveCourseRequest, new JsonMediaTypeFormatter()).Result;
                response.EnsureSuccessStatusCode();
            }
        }

        [Given(@"I have a course with following info:")]
        public void GivenIHaveACourseWithFollowingInfo(Table table)
        {
            var saveCourseRequest = new SaveCourseRequest
            {
                Name = ScenarioContext.Current.Get<long>("ticks") + table.Rows[0]["Name"],
                Code = ScenarioContext.Current.Get<long>("ticks") + table.Rows[0]["Code"],
                Description = table.Rows[0]["Description"],
                TenantId = int.Parse(table.Rows[0]["Tenant Id"]),
                //OrganizationId = new Guid(table.Rows[0]["OrganizationId"]),
                OrganizationId = Resources<OrganizationResource>.Get(table.Rows[0]["OrganizationName"]).Id,
                CourseType = ECourseType.Traditional,
                IsTemplate = false,
                Credit = decimal.Parse(table.Rows[0].GetValue("Credit", "0")),
                MetaData = table.Rows[0]["MetaData"],
                ExtensionAssets = string.IsNullOrWhiteSpace(table.Rows[0]["ExtensionAssets"]) ? null : Array.ConvertAll(table.Rows[0]["ExtensionAssets"].Split(','), s => new Guid(s)).ToList()
            };

            ScenarioContext.Current.Add("createCourseRequest", saveCourseRequest);
            ScenarioContext.Current.Add("courseName", table.Rows[0]["Name"]);
            ScenarioContext.Current.Add("courseCode", table.Rows[0]["Code"]);
            ScenarioContext.Current.Add("orgId", saveCourseRequest.OrganizationId);
            ScenarioContext.Current.Add("courseType", saveCourseRequest.CourseType);
            ScenarioContext.Current.Add("isTemplate", saveCourseRequest.IsTemplate);
        }

        [Given(@"I have an existing course with following info:")]
        public void GivenIHaveAnExistingCourseWithFollowingInfo(Table table)
        {
            // This is creating a course for us.
            GivenIHaveACourseWithFollowingInfo(table);

            var whens = new Whens();
            whens.WhenISubmitACreationRequest();

            var thens = new Thens();
            thens.ThenIShouldGetASuccessConfirmationMessage();
        }

        [Given(@"I create a course from the template '(.*)' with the following")]
        public void GivenICreateACourseFromTheTemplateWithTheFollowing(string templateName, Table table)
        {
            var whens = new Whens();
            var result = whens.WhenICreateACourseFromTheTemplateWithTheFollowing(templateName, table);
            result.EnsureSuccessStatusCode();
        }

        [Given(@"I have the following courses")]
        [Given(@"I have the following course templates")]
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
                    Credit = decimal.Parse(table.Rows[0].GetValue("Credit", "0")),
                    MetaData = row["MetaData"]
                };

                var assetNames = row["ExtensionAssets"].Split(',').ToList();
                var guidList = new List<Guid>();
                guidList.AddRange(assetNames.Select(a => Resources<AssetResource>.Get(a).Id));
                saveCourseRequest.ExtensionAssets = guidList;

                var result = PostOperations.CreateCourse(saveCourseRequest.Name, saveCourseRequest);
                result.EnsureSuccessStatusCode();
            }
        }

        [Given(@"I delete this course")]
        public void GivenIDeleteThisCourse()
        {
            var response = ScenarioContext.Current.Get<HttpResponseMessage>("createCourseResponse");
            var courseInfoResponse = response.Content.ReadAsAsync<CourseInfoResponse>().Result;
            ScenarioContext.Current.Add("courseId", courseInfoResponse.Id);
            var delSuccess = ApiFeature.CourseTestHost.Client.DeleteAsync(_leadingPath + "/" + courseInfoResponse.Id).Result;
            delSuccess.EnsureSuccessStatusCode();
        }

        [Given(@"I have the following programs")]
        [When(@"I create the following programs")]
        public void GivenIHaveTheFollowingPrograms(Table table)
        {
            foreach (var row in table.Rows)
            {
                var graduationRequirements = "";
                row.TryGetValue("GraduationRequirements", out graduationRequirements);
                var saveProgramRequest = new SaveProgramRequest
                {
                    Description = row["Description"],
                    Name = row["Name"],
                    ProgramType = row["ProgramType"],
                    OrganizationId = Resources<OrganizationResource>.Get(row["OrganizationName"]).Id,
                    GraduationRequirements = graduationRequirements
                };

                var result = PostOperations.CreateProgram(saveProgramRequest.Name, saveProgramRequest);
                //result.EnsureSuccessStatusCode();
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
            /*
            foreach (var row in table.Rows)
            {
                var request = new OutcomeRequest
                {
                    Description = row["Description"],
                };

                var result = PostOperations.CreateLearningOutcome(request.Description, request);
                result.EnsureSuccessStatusCode();
            }
            */
        }

        [Given(@"I add the following course learning activities to '(.*)' course segment")]
        public void GivenIAddTheFollowingCourseLearningActivitiesToCourseSegment(string segmentName, Table table)
        {
            var segment = Resources<CourseSegmentResource>.Get(segmentName);

            foreach (var row in table.Rows)
            {
                var learningActivityRequest = new SaveCourseLearningActivityRequest
                    {
                        Name = row["Name"],
                        IsGradeable = Convert.ToBoolean(row["IsGradeable"]),
                        Weight = Convert.ToInt32(row["Weight"]),
                        MaxPoint = Convert.ToInt32(row["MaxPoint"]),
                        IsExtraCredit = Convert.ToBoolean(row["IsExtraCredit"])
                    };

                var type = row.GetValue("Type", null);
                if(type != null)
                    learningActivityRequest.Type = (CourseLearningActivityType) Enum.Parse(typeof (CourseLearningActivityType), type);

                learningActivityRequest.AssessmentType = row.GetValue("AssessmentType", null);

                if (table.ContainsColumn("ObjectId"))
                {
                    learningActivityRequest.ObjectId = Guid.Parse(row["ObjectId"]);
                }
                if (table.ContainsColumn("Assessment"))
                {
                    var assessmentResource = Resources<AssessmentResource>.Get(row["Assessment"]);
                    learningActivityRequest.AssessmentId = assessmentResource.Id;
                }

                var result = PostOperations.CreateCourseLearningActivity(learningActivityRequest.Name, segment, learningActivityRequest);
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
                ApiFeature.MockAssetClient.Setup(x => x.AddAssetToLibrary("course", It.IsAny<Guid>(), resource.Id))
                          .Returns(new LibraryInfo {OwnerId = Guid.NewGuid()});
                //ApiFeature.MockAssetClient.Setup(x => x.GetAsset(resource.Id))
                //          .Returns(new AssetInfo { IsPublished = row.GetValue("IsPublished", false) });
            }
        }

        [Given(@"Create learning material as the following info")]
        public void GivenCreateLearningMaterialAsTheFollowingInfo(Table table)
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
                    CustomAttribute = row["CustomAttribute"]
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

        [Given(@"Published the following assets")]
        public void GivenPublishedTheFollowingAssets(Table table)
        {
            foreach (var row in table.Rows)
            {
                var name = row["Name"];
                var publishNote = row["PublishNote"];
                var asset = Resources<AssetResource>.Get(name);
                ApiFeature.MockAssetClient.Setup(x => x.PublishAsset(asset.Id, publishNote));
                ApiFeature.MockAssetClient.Setup(x => x.GetAsset(asset.Id)).Returns(new AssetInfo
                {
                    Id = asset.Id,
                    IsPublished = true
                });
            }
        }

        [Given(@"Publish the following courses")]
        public void GivenPublishTheFollowingCourses(Table table)
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
    }
}
