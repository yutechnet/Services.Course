using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Host.Controllers;
using NUnit.Framework;
using TechTalk.SpecFlow;
using System.Linq;

namespace BpeProducts.Services.Course.Host.Tests.Integration
{
    [Binding]
    public class ProgramCreationSteps
    {

        private readonly string _leadingPath;
        private string _tempId;
        private List<ProgramResponse> _programs;

        private SaveProgramRequest _programRequest;
        private SaveProgramRequest _editProgramRequest;
        private ProgramResponse _programResponse;
        private HttpResponseMessage _responseMessageToValidate;
        private SaveProgramRequest _anotherProgramRequest;
        private ProgramResponse _secondProgramResponse;
        private Guid _nonExistentId;

        public ProgramCreationSteps()
        {
            var targetUri = new Uri(ConfigurationManager.AppSettings["TestHostBaseAddress"]);
            if (!targetUri.Host.Equals("localhost"))
            {
                _leadingPath = targetUri.PathAndQuery + "/program";
            }
            else
            {
                _leadingPath = "/program";
            }
        }

        [Given(@"I have a program with following info:")]
        public void GivenIHaveAProgramWithFollowingInfo(Table table)
        {
            _tempId = DateTime.Now.ToLongTimeString();
            _programRequest = new SaveProgramRequest
                {
                    Name = table.Rows[0]["Name"] + _tempId,
                    Description = table.Rows[0]["Description"],
                    TenantId = table.Rows[0]["Tenant"],
                    OrganizationId = new Guid(table.Rows[0]["OrganizationId"]),
                    ProgramType = table.Rows[0]["ProgramType"]
                };
        }

        [Given(@"I have an existing program with following info:")]
        public void GivenIHaveAnExistingProgramWithFollowingInfo(Table table)
        {
            GivenIHaveAProgramWithFollowingInfo(table);
            WhenISubmitARequestToCreateAProgram();
        }
        
        [When(@"I submit a request to create a program")]
        public void WhenISubmitARequestToCreateAProgram()
        {
            _responseMessageToValidate = ApiFeature.ApiTestHost.Client.PostAsync(_leadingPath, _programRequest, new JsonMediaTypeFormatter()).Result;
            _responseMessageToValidate.EnsureSuccessStatusCode();

            _programResponse = _responseMessageToValidate.Content.ReadAsAsync<ProgramResponse>().Result;

        }

        [When(@"I modify the program info to reflect the following:")]
        public void WhenIModifyTheProgramInfoToReflectTheFollowing(Table table)
        {
            _editProgramRequest = new SaveProgramRequest
                {
                    Name = table.Rows[0]["Name"] + ScenarioContext.Current.Get<long>("ticks"),
                    Description = table.Rows[0]["Description"],
                    TenantId = table.Rows[0]["Tenant"],
                    OrganizationId = new Guid(table.Rows[0]["OrganizationId"]),
                    ProgramType = table.Rows[0]["ProgramType"]
                };

            _responseMessageToValidate = ApiFeature.ApiTestHost.Client.PutAsync(_leadingPath + "/" + _programResponse.Id, _editProgramRequest, new JsonMediaTypeFormatter()).Result;
            _responseMessageToValidate.EnsureSuccessStatusCode();
        }

        [When(@"I delete the program")]
        public void WhenIDeleteTheProgram()
        {
            _responseMessageToValidate = ApiFeature.ApiTestHost.Client.DeleteAsync(_leadingPath + "/" + _programResponse.Id).Result;
            _responseMessageToValidate.EnsureSuccessStatusCode();
        }

        [When(@"I create a new program with (.*), (.*), (.*)")]
        public void WhenICreateANewProgramWith(string tenant, string name, string description)
        {
            _programRequest = new SaveProgramRequest
                {
                    Name = string.IsNullOrEmpty(name) ? name : name + ScenarioContext.Current.Get<long>("ticks"),
                    Description = description,
                    TenantId = tenant,
                    OrganizationId = Guid.NewGuid(),
                    ProgramType = "BA"
                };

            _responseMessageToValidate = ApiFeature.ApiTestHost.Client.PostAsync(_leadingPath, _programRequest, new JsonMediaTypeFormatter()).Result;
        }

        [When(@"I request a program id that does not exist")]
        public void WhenIRequestAProgramIdThatDoesNotExist()
        {
            _nonExistentId = Guid.NewGuid();
            _responseMessageToValidate = ApiFeature.ApiTestHost.Client.GetAsync(_leadingPath + "/" + _nonExistentId).Result;
        }

        [When(@"I submit another request to create another program")]
        public void WhenISubmitAnotherRequestToCreateAnotherProgram(Table table)
        {
            _anotherProgramRequest = new SaveProgramRequest
                {
                    Name = table.Rows[0]["Name"] + ScenarioContext.Current.Get<long>("ticks"),
                    Description = table.Rows[0]["Description"],
                    TenantId = table.Rows[0]["Tenant"],
                    OrganizationId = new Guid(table.Rows[0]["OrganizationId"]),
                    ProgramType = table.Rows[0]["ProgramType"]
                };

            _responseMessageToValidate = ApiFeature.ApiTestHost.Client.PostAsync(_leadingPath, _anotherProgramRequest, new JsonMediaTypeFormatter()).Result;
            _responseMessageToValidate.EnsureSuccessStatusCode();
            _secondProgramResponse = _responseMessageToValidate.Content.ReadAsAsync<ProgramResponse>().Result;
        }


        [When(@"I request to get all programs")]
        public void WhenIRequestToGetAllPrograms()
        {
            var getAll = ApiFeature.ApiTestHost.Client.GetAsync(_leadingPath).Result;
            getAll.EnsureSuccessStatusCode();

            _programs = getAll.Content.ReadAsAsync<List<ProgramResponse>>().Result;
            
        }

        [Then(@"my program is returned")]
        public void ThenMyProgramIsReturned()
        {
            Assert.That(_programs.Any(p=> p.Id == _programResponse.Id));
            Assert.That(_programs.Any(p => p.Id == _secondProgramResponse.Id));
        }

        [Then(@"my program information is as follows:")]
        public void ThenMyProgramInformationIsAsFollows(Table table)
        {
            var programId = _programResponse.Id;
            var result = ApiFeature.ApiTestHost.Client.GetAsync(_leadingPath + "/" + programId).Result;
            result.EnsureSuccessStatusCode();
            _programResponse = result.Content.ReadAsAsync<ProgramResponse>().Result;
            
            Assert.That(_programResponse.Name, Is.EqualTo(_programRequest.Name));
            Assert.That(_programResponse.Description, Is.EqualTo(_programRequest.Description));
        }


        [Then(@"my program information is changed")]
        public void ThenMyProgramInformationIsChanged()
        {
            _responseMessageToValidate = ApiFeature.ApiTestHost.Client.GetAsync(_leadingPath + "/" + _programResponse.Id).Result;
            _responseMessageToValidate.EnsureSuccessStatusCode();
            _programResponse = _responseMessageToValidate.Content.ReadAsAsync<ProgramResponse>().Result;

            Assert.AreEqual(_programResponse.Name, _editProgramRequest.Name);
            Assert.AreEqual(_programResponse.Description, _editProgramRequest.Description);
        }

        [Then(@"the program no longer exists")]
        public void ThenTheProgramNoLongerExists()
        {
            _responseMessageToValidate = ApiFeature.ApiTestHost.Client.GetAsync(_leadingPath + "/" + _programResponse.Id).Result;
            Assert.That(_responseMessageToValidate.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Then(@"I should get the expected status code (.*)")]
        public void ThenIShouldGetTheExpectedStatusCodeBadRequest(string status)
        {
            var expectedCode = (HttpStatusCode) Enum.Parse(typeof (HttpStatusCode), status);
            Assert.That(_responseMessageToValidate.StatusCode, Is.EqualTo(expectedCode));
        }
    }
}
