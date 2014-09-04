using System;
using BpeProducts.Common.WebApiTest.Framework;
using BpeProducts.Common.WebApiTest.Extensions;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources.Assessment;
using BpeProducts.Services.Assessment.Contract;
using TechTalk.SpecFlow;

namespace BpeProducts.Services.Course.Host.Tests.Integration.StepSetups.Assessment
{
    [Binding]
    public class Givens
    {
        [Given(@"I have the following assessments")]
        public void GivenIHaveTheFollowingAssessments(Table table)
        {
            foreach (var row in table.Rows)
            {
                var assessmentId = Guid.NewGuid();
                ApiFeature.MockAssessmentClient.Setup(a => a.GetAssessment(assessmentId)).Returns(new AssessmentInfo
                    {
                        AssessmentType = (AssessmentType) Enum.Parse(typeof (AssessmentType), row["AssessmentType"]),
                        Id = assessmentId,
                        Instructions = row["Instructions"],
                        Title = row["Name"], 
                        IsPublished = row.GetValue("IsPublished", false),
                    });

                Resources<AssessmentResource>.Add(row["Name"], new AssessmentResource { Id = assessmentId});
            }
        }
    }
}
