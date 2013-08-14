using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Host.Tests.Integration.StepSetups;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace BpeProducts.Services.Course.Host.Tests.Integration
{
    [Binding]
    public class CourseSegmentSteps
    {
        [Then(@"the course '(.*)' should have these course segments:")]
        public void ThenTheCourseShouldHaveTheseCourseSegments(string courseName, Table table)
        {
            int rootLevelSegmentCount = 0;
            var resourceUri = Givens.Courses[courseName].ResourceUri;
            var response = ApiFeature.ApiTestHost.Client.GetAsync(resourceUri.ToString()).Result;
            response.EnsureSuccessStatusCode();

            var courseInfoResponse = response.Content.ReadAsAsync<CourseInfoResponse>().Result;

            foreach (var row in table.Rows)
            {
                var segmentResourceLocation = ScenarioContext.Current.Get<System.Uri>(row["Name"]);
                response = ApiFeature.ApiTestHost.Client.GetAsync(segmentResourceLocation).Result;
                var courseSegment = response.Content.ReadAsAsync<CourseSegmentInfo>().Result;

                if (!string.IsNullOrEmpty(row["ParentSegment"]))
                {
                    var parentSegmentResourceLocation = ScenarioContext.Current.Get<System.Uri>(row["ParentSegment"]);
                    var parentSegment =
                        ApiFeature.ApiTestHost.Client.GetAsync(parentSegmentResourceLocation)
                                  .Result.Content.ReadAsAsync<CourseSegmentInfo>()
                                  .Result;

                    Assert.That(parentSegment.Name, Is.EqualTo(row["ParentSegment"]));
                }
                else
                {
                    rootLevelSegmentCount++;
                }

                Assert.That(courseSegment.Description, Is.EqualTo(row["Description"]));
            }

            Assert.That(courseInfoResponse.Segments.Count, Is.EqualTo(rootLevelSegmentCount));
        }

        [Then(@"the course '(.*)' should have this course segment tree:")]
        public void ThenTheCourseShouldHaveThisCourseSegmentsTree(string courseName, Table table)
        {
            var resourceUri = Givens.Courses[courseName].ResourceUri;
            var response = ApiFeature.ApiTestHost.Client.GetAsync(resourceUri.ToString()).Result;
            response.EnsureSuccessStatusCode();

            var courseInfoResponse = response.Content.ReadAsAsync<CourseInfoResponse>().Result;
            var index = new Dictionary<string, CourseSegmentInfo>();

            IndexNodes(courseInfoResponse.Segments, ref index);

            foreach (var row in table.Rows)
            {
                var courseSegment = index[row["Name"]];
                Assert.That(courseSegment.Description, Is.EqualTo(row["Description"]));
                Assert.That(courseSegment.ChildSegments.Count, Is.EqualTo(Convert.ToInt32(row["ChildCount"])));
            }
        }

        [Then(@"the course segment '(.*)' should have these children segments:")]
        public void ThenTheCourseSegmentShouldHaveTheseChildrenSegments(string parentSegmentName, Table table)
        {
            var parentSegmentResourceLocation = ScenarioContext.Current.Get<System.Uri>(parentSegmentName);
            var parentSegment =
                ApiFeature.ApiTestHost.Client.GetAsync(parentSegmentResourceLocation)
                          .Result.Content.ReadAsAsync<CourseSegmentInfo>()
                          .Result;

            Assert.That(parentSegment.ChildSegments.Count, Is.EqualTo(table.Rows.Count));
            foreach (var row in table.Rows)
            {
                Assert.That(parentSegment.ChildSegments.Any(
                    s => s.Name == row["Name"] && s.Description == row["Description"] && s.Type == row["Type"]));
            }
        }

        [Then(@"the course segment '(.*)' should have this content")]
        public void ThenTheCourseSegmentShouldHaveThisContent(string courseSegmentName)
        {
            var segmentResourceLocation = ScenarioContext.Current.Get<System.Uri>(courseSegmentName);
            var response = ApiFeature.ApiTestHost.Client.GetAsync(segmentResourceLocation).Result;
            response.EnsureSuccessStatusCode();

            var savedContent = ScenarioContext.Current.Get<SaveCourseSegmentRequest>("SaveSegmentRequest").Content;
            var retrievedContent = response.Content.ReadAsAsync<CourseSegmentInfo>().Result.Content;

            Assert.That(savedContent.Count, Is.EqualTo(retrievedContent.Count));
            foreach (var contentRequest in savedContent)
            {
                var content = retrievedContent.First(c => c.Id == contentRequest.Id);
                Assert.That(contentRequest.Type, Is.EqualTo(content.Type));
            }
        }

        #region Helpers

        public void IndexNodes(IList<CourseSegmentInfo> segmentInfos, ref Dictionary<string, CourseSegmentInfo> index)
        {
            foreach (var courseSegmentInfo in segmentInfos)
            {
                index[courseSegmentInfo.Name] = courseSegmentInfo;
                if (courseSegmentInfo.ChildSegments.Count > 0)
                {
                    IndexNodes(courseSegmentInfo.ChildSegments, ref index);
                }
            }
        }

        #endregion
    }
}