using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text.RegularExpressions;
using BpeProducts.Services.Course.Contract;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace BpeProducts.Services.Course.Host.Tests.Integration
{
    [Binding]
    public class CourseSegmentLearningOutcomeSteps
    {
        private const int TenantId = 999999;

        [Given(@"the following course exists")]
        public void GivenTheFollowingCourseExists(Table table)
        {
            var postUri = FeatureContext.Current.Get<string>("CourseLeadingPath");
            var courseSaveRequest = table.CreateInstance<SaveCourseRequest>();
            courseSaveRequest.TenantId = TenantId;

            var response = ApiFeature.ApiTestHost.Client.PostAsync(postUri, courseSaveRequest, new JsonMediaTypeFormatter()).Result;
            response.EnsureSuccessStatusCode();

            ScenarioContext.Current.Add(courseSaveRequest.Name, response.Headers.Location);
        }


        [Given(@"'(.*)' course has the following learning outcomes:")]
        public void GivenCourseHasTheFollowingLearningOutcomes(string courseName, Table table)
        {
            var courseResourceUri = ScenarioContext.Current.Get<Uri>(courseName);

            foreach (var row in table.Rows)
            {
                var postUri = string.Format("{0}/ToBeDetermined", courseResourceUri.ToString());
                var outcomeRequest = new OutcomeRequest
                    {
                        Description = row["Description"],
                        TenantId = TenantId
                    };

                var response = ApiFeature.ApiTestHost.Client.PostAsync(postUri, outcomeRequest, new JsonMediaTypeFormatter()).Result;
                response.EnsureSuccessStatusCode();

                ScenarioContext.Current.Add(outcomeRequest.Description, response.Headers.Location);
            }
        }

        [Given(@"I add following course segments to '(.*)'")]
        public void GivenIAddFollowingCourseSegmentsTo(string courseName, Table table)
        {
            var courseResourceUri = ScenarioContext.Current.Get<Uri>(courseName);
            var postUri = string.Format("{0}/segments", courseResourceUri);
            var segmentRequest = table.CreateInstance<SaveCourseSegmentRequest>();
            segmentRequest.TenantId = TenantId;

            var response = ApiFeature.ApiTestHost.Client.PostAsync(postUri, segmentRequest, new JsonMediaTypeFormatter()).Result;
            response.EnsureSuccessStatusCode();

            ScenarioContext.Current.Add(segmentRequest.Name, response.Headers.Location);
        }

        [Given(@"I create the following learning outcomes for '(.*)' course segment:")]
        [When(@"I add the following learning outcomes for '(.*)' course segment:")]
        public void WhenIAddTheFollowingLearningOutcomesForCourseSegment(string segmentName, Table table)
        {
            // POST /segment/<segmentId>/outcome {description:blah} => /outcome/{outcomeId}
            var segmentResourceUri = ScenarioContext.Current.Get<Uri>(segmentName);
            // http://localhost/course/<courseId>/segments/<segmentId>
            // Change the code to return http://localhost/segments/<segmentId>
            var postUri = string.Format("{0}/outcome", segmentResourceUri);
            var outcomeList = table.CreateSet<OutcomeRequest>();

            foreach (var outcomeRequest in outcomeList)
            {
                outcomeRequest.TenantId = TenantId;
                var response = ApiFeature.ApiTestHost.Client.PostAsync(postUri, outcomeRequest, new JsonMediaTypeFormatter()).Result;
                response.EnsureSuccessStatusCode();

                ScenarioContext.Current.Add(outcomeRequest.Description, response.Headers.Location);
            }

            // POST   (Done) /outcome => /outcome/{outcomeId}
            // PUT    (Done) /course/{courseId}/segment/<segmentId>/outcome/{outcomeId} --> add an outcome to the segment
            // DELETE (Done) /course/{courseId}/segment/<segmentId>/outcome/{outcomeId} --> remove and outcome from the segment
        }

        [Then(@"'(.*)' course segment has the following learning outcomes:")]
        public void ThenCourseSegmentHasTheFollowingLearningOutcomes(string segmentName, Table table)
        {
            var segmentResourceUri = ScenarioContext.Current.Get<Uri>(segmentName);
            var response = ApiFeature.ApiTestHost.Client.GetAsync(segmentResourceUri.ToString()).Result;
            response.EnsureSuccessStatusCode();

            var courseSegmentInfo = response.Content.ReadAsAsync<CourseSegmentInfo>().Result;
            table.CompareToSet(courseSegmentInfo.SupportingOutcomes);
        }

        [When(@"I associate the following segment learning outcomes to '(.*)' course learning outcome")]
        public void WhenIAssociateTheFollowingSegmentLearningOutcomesToCourseLearningOutcome(string courseLearningOutcomeName, Table table)
        {
            var supportedLearningOutcomeResourceUri = ScenarioContext.Current.Get<Uri>(courseLearningOutcomeName);
            var supportingLearningOutcomeList = table.CreateSet<OutcomeRequest>();

            foreach (var learningOutcomeRequest in supportingLearningOutcomeList)
            {
                learningOutcomeRequest.TenantId = TenantId;
                var supportingLearningOutcomeResourceUri = ScenarioContext.Current.Get<Uri>(learningOutcomeRequest.Description);

                var putUri = string.Format("{0}/supports/{1}", supportingLearningOutcomeResourceUri,
                                           ExtractGuid(supportedLearningOutcomeResourceUri.AbsolutePath, 1));

                var response = ApiFeature.ApiTestHost.Client.PutAsync(putUri, new {}, new JsonMediaTypeFormatter()).Result;
                response.EnsureSuccessStatusCode();
            }
        }

        [Then(@"'(.*)' course has the following learning outcomes tree")]
        public void ThenCourseHasTheFollowingLearningOutcomesTree(string courseName, Table table)
        {
            var courseResourceUri = ScenarioContext.Current.Get<Uri>(courseName);
            var response = ApiFeature.ApiTestHost.Client.GetAsync(courseResourceUri).Result;
            response.EnsureSuccessStatusCode();

            var courseInfoResponse = response.Content.ReadAsAsync<CourseInfoResponse>().Result;

            foreach (var row in table.Rows)
            {
                var supportedOutcomeDescription = row["Description"];

                var supporedOutcome = courseInfoResponse.SupportingOutcomes.FirstOrDefault(o => o.Description == supportedOutcomeDescription);
                Assert.NotNull(supporedOutcome);

                if (string.IsNullOrEmpty(row["SupportingOutcomes"]))
                    continue;

                var supportingOutcomeDescriptions = row["SupportingOutcomes"].Split(',');

                foreach (var outcomeDescription in supportingOutcomeDescriptions)
                {
                    var supportingOutcome = supporedOutcome.SupportingOutcomes.FirstOrDefault(o => o.Description == outcomeDescription.Trim());
                    Assert.NotNull(supportingOutcome);
                }
            }
        }

        private string ExtractGuid(string str, int i)
        {
            var p = @"([a-z0-9]{8}[-][a-z0-9]{4}[-][a-z0-9]{4}[-][a-z0-9]{4}[-][a-z0-9]{12})";
            var mc = Regex.Matches(str, p);

            return mc[i].ToString();
        }

    }
}
