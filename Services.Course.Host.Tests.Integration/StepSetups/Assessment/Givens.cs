using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Common.WebApiTest.Framework;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources.Assessment;
using Services.Assessment.Contract;
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
                        Title = row["Name"]
                    });
                Resources<AssessmentResource>.Add(row["Name"], new AssessmentResource { Id = assessmentId});
            }
        }
    }
}
