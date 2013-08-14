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
        [Given(@"I add following course segments to '(.*)':")]
        [When(@"I add following course segments to '(.*)':")]
        public void WhenIAddFollowingCourseSegmentsTo(string courseName, Table table)
        {
            var segments = table.Rows.Select(row => new
                {
                    ParentSegment = row["ParentSegment"],
                    Segment = new SaveCourseSegmentRequest
                        {
                            Name = row["Name"],
                            Description = row["Description"],
                            Type = row["Type"],
                            TenantId = 999999,
                            DisplayOrder = row.ContainsKey("DisplayOrder") ? int.Parse(row["DisplayOrder"]) : 0
                        }
                }).ToList();

            var resourceUri = Givens.Courses[courseName].ResourceUri;
            var response = ApiFeature.ApiTestHost.Client.GetAsync(resourceUri.ToString()).Result;
            response.EnsureSuccessStatusCode();

            var courseInfoResponse = response.Content.ReadAsAsync<CourseInfoResponse>().Result;

            segments.ForEach(s =>
                {
                    response = null;
                    if (string.IsNullOrEmpty(s.ParentSegment))
                    {
                        var postUrl = string.Format("{0}/{1}/segments",
                                                    FeatureContext.Current.Get<String>("CourseLeadingPath"),
                                                    courseInfoResponse.Id);
                        response =
                            ApiFeature.ApiTestHost.Client.PostAsync(postUrl, s.Segment,
                                                                    new JsonMediaTypeFormatter()).Result;
                    }
                    else
                    {
                        var postUrl = ScenarioContext.Current.Get<System.Uri>(s.ParentSegment);
                        response =
                            ApiFeature.ApiTestHost.Client.PostAsync(postUrl.ToString() + "/segments", s.Segment,
                                                                    new JsonMediaTypeFormatter()).Result;
                    }
                    response.EnsureSuccessStatusCode();

                    ScenarioContext.Current.Add(s.Segment.Name, response.Headers.Location);
                });
        }

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


        [When(@"I update the course segments as following:")]
        public void WhenIUpdateTheCourseSegmentAsFollowing(Table table)
        {
            foreach (var row in table.Rows)
            {
                var courseSegmentName = row["Name"];
                var segmentResourceLocation = ScenarioContext.Current.Get<System.Uri>(courseSegmentName);
                var response =
                    ApiFeature.ApiTestHost.Client.PutAsync<SaveCourseSegmentRequest>(segmentResourceLocation.ToString(),
                                                                                     new SaveCourseSegmentRequest
                                                                                         {
                                                                                             Name = courseSegmentName,
                                                                                             Description =
                                                                                                 row["Description"],
                                                                                             Type = row["Type"],
                                                                                             DisplayOrder = row.ContainsKey("DisplayOrder") ? int.Parse(row["DisplayOrder"]) : 0
                                                                                         }, new JsonMediaTypeFormatter())
                              .Result;
                response.EnsureSuccessStatusCode();
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

        [When(@"I add the following content to '(.*)' segment")]
        public void WhenIAddTheFollowingContentToSegment(string courseSegmentName, Table table)
        {
            var segmentResourceLocation = ScenarioContext.Current.Get<System.Uri>(courseSegmentName);
            var response = ApiFeature.ApiTestHost.Client.GetAsync(segmentResourceLocation).Result;
            response.EnsureSuccessStatusCode();

            var segmentInfo = response.Content.ReadAsAsync<CourseSegmentInfo>().Result;

            var saveCourseSegmentRequest = new SaveCourseSegmentRequest
                {
                    Name = segmentInfo.Name,
                    Type = segmentInfo.Type,
                    Description = segmentInfo.Description,
                    // TenantId = 1,
                    Content =
                        table.Rows.Select(row => new Content {Id = Guid.Parse(row["Id"]), Type = row["Type"]}).ToList()
                };

            response =
                ApiFeature.ApiTestHost.Client.PutAsync(segmentResourceLocation.ToString(), saveCourseSegmentRequest,
                                                       new JsonMediaTypeFormatter()).Result;
            response.EnsureSuccessStatusCode();

            ScenarioContext.Current.Add("SaveSegmentRequest", saveCourseSegmentRequest);
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

        [Then(@"The course '(.*)' segments retrieved match the display order entered:")]
        public void ThenTheCourseSegmentsRetrievedMatchTheDisplayOrderEntered(string courseName, Table table)
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

                Assert.That(courseSegment.DisplayOrder, Is.EqualTo(Convert.ToInt32(row["DisplayOrder"])));
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