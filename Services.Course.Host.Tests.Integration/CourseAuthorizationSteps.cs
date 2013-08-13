using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using BpeProducts.Common.WebApiTest;
using BpeProducts.Services.Course.Contract;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace BpeProducts.Services.Course.Host.Tests.Integration
{
	[Binding]
	public class CourseAuthorizationSteps
	{
		private readonly string _leadingPath;

		private AuthenticationHeaderValue _originalToken;

		// For additional details on SpecFlow step definitions see http://go.specflow.org/doc-stepdef
		public CourseAuthorizationSteps()
        {
            var targetUri = new Uri(ConfigurationManager.AppSettings["TestHostBaseAddress"]);
            if (!targetUri.Host.Equals("localhost"))
            {
                _leadingPath = targetUri.PathAndQuery + "/course";
            }
            else
            {
                _leadingPath = "/course";
            }
        }

		[BeforeScenario]
		public void BeforeScenario()
		{
			_originalToken = ApiFeature.ApiTestHost.Client.DefaultRequestHeaders.Authorization;
		}

		[AfterScenario]
		public void AfterScenario()
		{
			ApiFeature.ApiTestHost.Client.DefaultRequestHeaders.Authorization = _originalToken;
		}

		[Given(@"That I am admin")]
        public void GivenThatIAmAdmin()
        {
            ApiFeature.ApiTestHost.SetTestUser(TestUserName.SuperSaml);
        }

		[Given(@"That I am guest")]
		public void GivenThatIAmGuest()
		{
            ApiFeature.ApiTestHost.SetTestUser(TestUserName.GuestUser1);           
		}

		[When(@"I submit an authorized creation request")]
		public void WhenISubmitAnAuthorizedCreationRequest()
		{
			var saveCourseRequest = new SaveCourseRequest()
				{
					Name = "Test",
					Code = "Test",
					Description = "Test",
					TenantId = 999999,					              
					OrganizationId = Guid.NewGuid()
				};

			var response = ApiFeature.ApiTestHost.Client.PostAsync(_leadingPath, saveCourseRequest, new JsonMediaTypeFormatter()).Result;

			if (ScenarioContext.Current.ContainsKey("createCourseResponse"))
			{
				ScenarioContext.Current.Remove("createCourseResponse");
			}
			ScenarioContext.Current.Add("createCourseResponse", response);

			// this is the response to ensure the success code
			if (ScenarioContext.Current.ContainsKey("responseToValidate"))
			{
				ScenarioContext.Current.Remove("responseToValidate");
			}
			ScenarioContext.Current.Add("responseToValidate", response);
		}

		[Then(@"I should get a success response")]
		public void ThenIShouldGetASuccessResponse()
		{
			var response = ScenarioContext.Current.Get<HttpResponseMessage>("responseToValidate");
			response.EnsureSuccessStatusCode();
		}

		[Then(@"I should get a failure response")]
		public void ThenIShouldGetAFailureResponse()
		{
			var response = ScenarioContext.Current.Get<HttpResponseMessage>("responseToValidate");
			Assert.AreEqual(response.StatusCode,HttpStatusCode.Unauthorized);
		}
	}
}
