using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using BpeProducts.Services.Course.Contract;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace BpeProducts.Services.Course.Host.Tests.Integration
{
    [Binding]
    public class CourseSegmentSteps
    {
		
		[When(@"I add following course segments to '(.*)':")]
		public void WhenIAddFollowingCourseSegmentsTo(string courseName, Table table)
		{
			var segments = table.Rows.Select(row => new CourseSegment
				{
					Name = row["Name"], Description = row["Description"], Type = row["Type"], TenantId = 1
				}).ToList();

			var courseInfoResponse = ScenarioContext.Current.Get<CourseInfoResponse>(courseName);
			var saveCourseRequest = new SaveCourseRequest
				{
					Id = courseInfoResponse.Id,
					Code = courseInfoResponse.Code,
					Name = courseInfoResponse.Name,
					Description = courseInfoResponse.Description,
					ProgramIds = courseInfoResponse.ProgramIds,
					Segments = segments,
					TenantId = 1
				};
			var response =
			   ApiFeature.ApiTestHost.Client.PutAsync(FeatureContext.Current.Get<String>("CourseLeadingPath") + "/" + courseInfoResponse.Id, saveCourseRequest,
													  new JsonMediaTypeFormatter()).Result;
			response.EnsureSuccessStatusCode();
		}

      
		[Then(@"the course '(.*)' should have these course segments:")]
		public void ThenTheCourseShouldHaveTheseCourseSegments(string courseName, Table table)
		{
			var courseInfoResponse = ScenarioContext.Current.Get<CourseInfoResponse>(courseName);
			var response =
			   ApiFeature.ApiTestHost.Client.GetAsync(FeatureContext.Current.Get<String>("CourseLeadingPath") + "/" + courseInfoResponse.Id).Result;
			response.EnsureSuccessStatusCode();
			var segments = response.Content.ReadAsAsync<CourseInfoResponse>().Result.Segments;
			Assert.That(segments.Count, Is.EqualTo(table.Rows.Count));
			foreach (var row in table.Rows)
			{
				Assert.That(segments.Any(s=>s.Name==row["Name"]),"segment does not exist");
			}
		}

    }
}
