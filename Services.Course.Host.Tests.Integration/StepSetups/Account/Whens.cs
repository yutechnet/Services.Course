using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Host.Tests.Integration.Operations;
using TechTalk.SpecFlow;

namespace BpeProducts.Services.Course.Host.Tests.Integration.StepSetups.Account
{
    [Binding]
    public class Whens
    {
        [When(@"I create a course under organization (.*)")]
        [When(@"I create a course under organization ""(.*)""")]
        public void WhenICreateACourseUnderOrganization(string oranizationName)
        {
            var org = Givens.Organizations[oranizationName];

            var saveCourseRequest = new SaveCourseRequest
            {
                Name = "RandomCourse",
                Description = "RandomCourse",
                Code = "RandomCourse",
                CourseType = ECourseType.Traditional,
                IsTemplate = false,
                OrganizationId = org.Id
            };

            PostOperations.CreateCourse(saveCourseRequest.Name, saveCourseRequest);
        }
    }
}
