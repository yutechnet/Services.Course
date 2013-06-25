using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using BpeProducts.Services.Course.Contract;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace BpeProducts.Services.Course.Host.Tests.Integration
{
    [Binding]
    public class CourseSegmentSteps
    {
		
		[When(@"I add following course segments to '(.*)':")]
		public void WhenIAddFollowingCourseSegmentsTo(string courseName, Table table)
		{
            
			var segments = table.Rows.Select(row =>new{
                ParentSegment=row["ParentSegment"], 
                Segment=new SaveCourseSegmentRequest
				{
					Name = row["Name"], 
                    Description = row["Description"], 
                    Type = row["CourseType"]
				}}).ToList();

			var courseInfoResponse = ScenarioContext.Current.Get<CourseInfoResponse>(courseName);


            segments.ForEach(s =>
                {
                    HttpResponseMessage response = null;
                    if (string.IsNullOrEmpty(s.ParentSegment))
                    {
                        var postUrl = string.Format("{0}/{1}/segments", FeatureContext.Current.Get<String>("CourseLeadingPath"),
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
			var courseInfoResponse = ScenarioContext.Current.Get<CourseInfoResponse>(courseName);
		    foreach (var row in table.Rows)
		    {
		        var segmentResourceLocation = ScenarioContext.Current.Get<System.Uri>(row["Name"]);
		        var response = ApiFeature.ApiTestHost.Client.GetAsync(segmentResourceLocation).Result;
		        var courseSegment = response.Content.ReadAsAsync<CourseSegment>().Result;

		        if (!string.IsNullOrEmpty(row["ParentSegment"]))
		        {
                    var parentSegmentResourceLocation = ScenarioContext.Current.Get<System.Uri>(row["ParentSegment"]);
		            var parentSegment =
		                ApiFeature.ApiTestHost.Client.GetAsync(parentSegmentResourceLocation)
		                          .Result.Content.ReadAsAsync<CourseSegment>()
		                          .Result;

                    Assert.That(parentSegment.Name, Is.EqualTo(row["ParentSegment"]));
                }

                Assert.That(courseSegment.Description, Is.EqualTo(row["Description"]));
		    }
		}

        [When(@"I update the course segments as following:")]
        public void WhenIUpdateTheCourseSegmentAsFollowing(Table table)
        {
            foreach (var row in table.Rows)
            {
                var courseSegmentName = row["Name"];
                var segmentResourceLocation = ScenarioContext.Current.Get<System.Uri>(courseSegmentName);
                var response = ApiFeature.ApiTestHost.Client.PutAsync<SaveCourseSegmentRequest>(segmentResourceLocation.ToString(),
                    new SaveCourseSegmentRequest
                    {
                        Name = courseSegmentName,
                        Description = row["Description"],
                        Type = row["CourseType"]
                    }, new JsonMediaTypeFormatter()).Result;
                response.EnsureSuccessStatusCode();
            }
        }

        [Then(@"the course segment '(.*)' should have these children segments:")]
        public void ThenTheCourseSegmentShouldHaveTheseChildrenSegments(string parentSegmentName, Table table)
        {
            var parentSegmentResourceLocation = ScenarioContext.Current.Get<System.Uri>(parentSegmentName);
            var parentSegment =
                ApiFeature.ApiTestHost.Client.GetAsync(parentSegmentResourceLocation)
                          .Result.Content.ReadAsAsync<CourseSegment>()
                          .Result;

            Assert.That(parentSegment.ChildrenSegments.Count, Is.EqualTo(table.Rows.Count));
            foreach (var row in table.Rows)
            {
                Assert.That(parentSegment.ChildrenSegments.Any(
                    s => s.Name == row["Name"] && s.Description == row["Description"] && s.Type == row["CourseType"]));
            }
        }

        [When(@"I add the following content to '(.*)' segment")]
        public void WhenIAddTheFollowingContentToSegment(string courseSegmentName, Table table)
        {
            var segmentResourceLocation = ScenarioContext.Current.Get<System.Uri>(courseSegmentName);
            var response = ApiFeature.ApiTestHost.Client.GetAsync(segmentResourceLocation).Result;
            response.EnsureSuccessStatusCode();

            var segmentInfo = response.Content.ReadAsAsync<CourseSegment>().Result;

            var saveCourseSegmentRequest = new SaveCourseSegmentRequest
                {
                    Name = segmentInfo.Name,
                    Type = segmentInfo.Type,
                    Description = segmentInfo.Description,
                    // TenantId = 1,
                    Content = table.CreateSet<ContentRequest>().ToList()
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
            var retrievedContent = response.Content.ReadAsAsync<CourseSegment>().Result.Content;

            Assert.That(savedContent.Count, Is.EqualTo(retrievedContent.Count));
            foreach (var contentRequest in savedContent)
            {
                var content = retrievedContent.First(c => c.Id == contentRequest.Id);
                Assert.That(contentRequest.Type, Is.EqualTo(content.Type));
            }
        }
    }
}
