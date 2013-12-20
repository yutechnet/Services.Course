﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using BpeProducts.Common.WebApiTest.Extensions;
using BpeProducts.Common.WebApiTest.Framework;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Host.Tests.Integration.Operations;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources.Account;
using NHibernate.Linq;
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
        }

        [Then(@"the course '(.*)' includes the following programs")]
        public void ThenTheCourseIncludesTheFollowingPrograms(string courseName, Table table)
        {
            var resource = Resources<CourseResource>.Get(courseName);

            var course = GetOperations.GetCourse(resource);
            var expectedProgramIds = (from r in table.Rows select Resources<ProgramResource>.Get(r["Program Name"]).Id).ToList();

            CollectionAssert.AreEquivalent(course.ProgramIds, expectedProgramIds);
        }

        [Then(@"'(.*)' program is associated with the following learning outcomes")]
        public void ThenProgramIsAssociatedWithTheFollowingLearningOutcomes(string programName, Table table)
        {
            var resource = Resources<ProgramResource>.Get(programName);

            var outcomes = GetOperations.GetEntityLearningOutcomes(new List<Guid> {resource.Id});

            var actualOutcomes = from o in outcomes[resource.Id] select o.Description;
            var expectedOutcomes = (from r in table.Rows select r["Description"]).ToList();

            CollectionAssert.AreEquivalent(expectedOutcomes, actualOutcomes);
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

            table.CompareToInstance(courseLearningActivity);
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

            var expectedLearningActivities =
                (from r in table.Rows select Resources<CourseLearningActivityResource>.Get(r["Name"]).Id).ToList();
            var actualLearningActivities = (from a in segment.CourseLearningActivities select a.Id).ToList();

            CollectionAssert.AreEquivalent(expectedLearningActivities, actualLearningActivities);
        }

        [Then(@"the learning outcome '(.*)' should contain")]
        public void ThenTheLearningOutcomeShouldContain(string learningOutcomeName, Table table)
        {
            var resource = Resources<LearningOutcomeResource>.Get(learningOutcomeName);

            var actual = GetOperations.GetLearningOutcome(resource);
            var expected = table.CreateInstance<OutcomeInfo>();

            Assert.That(expected.Description, Is.EqualTo(actual.Description));
        }

        [Then(@"the course '(.*)' has the following learning outcomes")]
        public void ThenCourseHasTheFollowingLearningOutcomes(string courseName, Table table)
        {
            var resource = Resources<CourseResource>.Get(courseName);
            var course = GetOperations.GetCourse(resource);

            foreach (var row in table.Rows)
            {
                var supportedOutcomeDescription = row["Description"];

                var supporedOutcome = course.SupportedOutcomes.FirstOrDefault(o => o.Description == supportedOutcomeDescription);
                Assert.NotNull(supporedOutcome);

                if (string.IsNullOrEmpty(row["SupportingOutcomes"]))
                    continue;

                var supportingOutcomeDescriptions = row["SupportingOutcomes"].Split(',');

                foreach (var outcomeDescription in supportingOutcomeDescriptions)
                {
                    var supportingOutcome = supporedOutcome.SupportedOutcomes.FirstOrDefault(o => o.Description == outcomeDescription.Trim());
                    Assert.NotNull(supportingOutcome);
                }
            }
        }
   
        [Then(@"the segment '(.*)' includes the following learning outcomes")]
        public void ThenTheSegmentIncludesTheFollowingLearningOutcomes(string segmentName, Table table)
        {
            var resource = Resources<CourseSegmentResource>.Get(segmentName);
            var outcomes = GetOperations.GetSupportedOutcomes(resource);
            table.CompareToSet(outcomes);
        }

        [Then(@"'(.*)' program is associated with the only following learning outcomes")]
        public void ThenProgramIsAssociatedWithTheOnlyFollowingLearningOutcomes(string programName, Table table)
        {
            var resource = Resources<ProgramResource>.Get(programName);

            var actualOutcomes =
                (from o in GetOperations.GetSupportedOutcomes(resource) select o.Description).ToList();
            var expectedOutcomes = (from o in table.Rows select o["Description"]).ToList();

            CollectionAssert.AreEquivalent(expectedOutcomes, actualOutcomes);
        }

        [Then(@"learning outcome '(.*)' is supported by the following learning outcomes")]
        public void ThenLearningOutcomeIsSupportedByTheFollowingLearningOutcomes(string learningOutcomeName, Table table)
        {
            var resource = Resources<LearningOutcomeResource>.Get(learningOutcomeName);

            var actualOutcomes = (from o in GetOperations.GetSupportedOutcomes(resource) select o.Description).ToList();
            var expectedOutcomes = (from o in table.Rows select o["Description"]).ToList();

            CollectionAssert.AreEquivalent(expectedOutcomes, actualOutcomes);
        }

        [Then(@"I get the entity learning outcomes as follows")]
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
                        resource = Resources<ProgramResource>.Get(entityName);
                        break;
                    case "Course":
                        resource = Resources<CourseResource>.Get(entityName);
                        break;
                    case "Segment":
                        resource = Resources<CourseSegmentResource>.Get(entityName);
                        break;
                }

                if (resource == null)
                    throw new Exception("No recourse found for entity type " + entityType);

                var expectedOutcomes = (from o in row["LearningOutcomes"].Split(new[] {','})
                                        where !String.IsNullOrWhiteSpace(o)
                                        select Resources<LearningOutcomeResource>.Get(o.Trim()).Id).ToList();
                expectedEntityOutcomes.Add(resource, expectedOutcomes);
            }

            var entityIdsToGet = (from e in expectedEntityOutcomes.Keys select e.Id).ToList();
            var actualEntityOutcomes = GetOperations.GetEntityLearningOutcomes(entityIdsToGet);

            foreach (var expectedEntity in expectedEntityOutcomes)
            {
                if (expectedEntity.Value.Count == 0) continue;

                List<OutcomeInfo> outcomes;

                if (actualEntityOutcomes.TryGetValue(expectedEntity.Key.Id, out outcomes))
                {
                    var actualEntityOutcomeIds = (from o in outcomes select o.Id).ToList();
                    CollectionAssert.AreEquivalent(expectedEntity.Value.ToList(), actualEntityOutcomeIds);
                }
                else
                {
                    Assert.That(actualEntityOutcomes.Keys.Contains(expectedEntity.Key.Id));
                }
            }
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
                    Assert.That(actualSegment.ParentSegmentId, Is.EqualTo(Guid.Empty));
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

        [Then(@"the course '(.*)' should have the following learning outcomes")]
        public void ThenTheCourseShouldHaveTheFollowingLearningOutcomes(string courseName, Table table)
        {
            var resource = Resources<CourseResource>.Get(courseName);
            var courseInfo = GetOperations.GetCourse(resource);

            var expected = (from r in table.Rows select r["Description"]).ToList();
            var actual = (from o in courseInfo.SupportedOutcomes select o.Description).ToList();

            CollectionAssert.AreEquivalent(expected, actual);
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
            }
        }


        [Then(@"the learning outcome '(.*)' supports the following learning outcomes")]
        public void ThenTheLearningOutcomeSupportsTheFollowingLearningOutcomes(string learningOutcomeName, Table table)
        {
            foreach (var row in table.Rows)
            {
                var resource = Resources<LearningOutcomeResource>.Get(row["Description"]);

                var actualOutcomes = (from o in GetOperations.GetSupportedOutcomes(resource) select o.Description).ToList();

                Assert.That(actualOutcomes.Count, Is.EqualTo(1));
                Assert.That(actualOutcomes[0], Is.EqualTo(learningOutcomeName));
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

        [Then(@"the learning activity '(.*)' should have the following rubrics")]
        public void ThenTheLearningActivityShouldHaveTheFollowingRubrics(string learningActivityName, Table table)
        {
            var resource = Resources<CourseLearningActivityResource>.Get(learningActivityName);
            var actual = GetOperations.GetCourseLearningActivity(resource);

			Assert.That(actual.CourseRubrics.Count, Is.EqualTo(table.RowCount));
			foreach (var row in table.Rows)
			{
				var title = row["Title"];
				var rubricId = Resources<RubricResource>.Get(title);
				Assert.That(actual.CourseRubrics.Any(x => x.RubricId == rubricId.Id));
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
            var materialResource = Resources<LearningMaterialResource>.Get(materialName);
            var response = GetOperations.GetCourseLearningMaterial(materialResource);
            foreach (var row in table.Rows)
            {
                var segmentName = row["CourseSegment"];
                var courseSegment = Resources<CourseSegmentResource>.Get(segmentName);
                var assetName = row["Asset"];
                var asset = Resources<AssetResource>.Get(assetName);
                Assert.That(response.AssetId, Is.EqualTo(asset.Id));
                Assert.That(response.CourseSegmentId, Is.EqualTo(courseSegment.Id));
                Assert.That(response.Instruction, Is.EqualTo(row["Instruction"]));
                Assert.That(response.IsRequired, Is.EqualTo(bool.Parse(row["IsRequired"])));
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
            }
        }

        [Then(@"The course '(.*)' has following learning material")]
        public void ThenTheCourseHasFollowingLearningMaterial(string courseName, Table table)
        {
            var courseResource = Resources<CourseResource>.Get(courseName);
            var response = GetOperations.GetCourse(courseResource);
            foreach (var row in table.Rows)
            {
                var segmentName = row["CourseSegment"];
                var assetName = row["Asset"];
                var asset = Resources<AssetResource>.Get(assetName);
                var segmentResponse = response.Segments.First(p => p.Name == segmentName);
                Assert.That(segmentResponse.LearningMaterials.Count, Is.EqualTo(1));
                Assert.That(segmentResponse.LearningMaterials[0].AssetId, Is.EqualTo(asset.Id));
                Assert.That(segmentResponse.LearningMaterials[0].CourseSegmentId, Is.EqualTo(segmentResponse.Id));
                Assert.That(segmentResponse.LearningMaterials[0].Instruction, Is.EqualTo(row["Instruction"]));
                Assert.That(segmentResponse.LearningMaterials[0].IsRequired, Is.EqualTo(bool.Parse(row["IsRequired"])));
            }
        }

        [Then(@"The asset '(.*)' is published")]
        public void ThenTheAssetIsPublished(string assetName)
        {
            var asset = Resources<AssetResource>.Get(assetName);
            var response = ApiFeature.MockAssetClient.Object.GetAsset(asset.Id);
            Assert.That(response.IsPublished, Is.EqualTo(true));
        }

    }
}

