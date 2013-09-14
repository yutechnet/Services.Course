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
        [When(@"I create a course (.*) under organization (.*)")]
		[When(@"I create a course ""(.*)"" under organization ""(.*)""")]
        public void WhenICreateACourseUnderOrganization(string courseName,string oranizationName)
        {
	        CourseCreationSteps.CreateACourse(courseName, oranizationName);
        }
    }
}
