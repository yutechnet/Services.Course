﻿using System;
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
//    public class TestEntity
//    {
//        public string TestId { get; set; }
//        public SaveProgramRequest SaveProgramRequest { get; set; }
//
//    }

    [Binding]
    public class ProgramCreationSteps
    {

        private string _leadingPath;

        public ProgramCreationSteps()
        {
            var targetUri = new Uri(ConfigurationManager.AppSettings["TestHostBaseAddress"]);
            if (!targetUri.Host.Equals("localhost"))
            {
                _leadingPath = targetUri.PathAndQuery + "/programs";
            }
            else
            {
                _leadingPath = "/programs";
            }
        }

        [Given(@"I have a program with following info:")]
        public void GivenIHaveAProgramWithFollowingInfo(Table table)
        {
            var tempId = DateTime.Now.ToLongTimeString();
                //ScenarioContext.Current.Get<long>("ticks");
            var programRequest = new SaveProgramRequest
                {
                    Name = table.Rows[0]["Name"] + tempId,
                    Description = table.Rows[0]["Description"],
                    TenantId = "1"
                };

            ScenarioContext.Current.Add(tempId, "testId");
            ScenarioContext.Current.Add("programRequest", programRequest);
        }

        [Given(@"I have an existing program with following info:")]
        public void GivenIHaveAnExistingProgramWithFollowingInfo(Table table)
        {
            GivenIHaveAProgramWithFollowingInfo(table);
            WhenISubmitARequestToCreateAProgram();
            ThenTheOperationIsSuccessful();
        }
        
        [When(@"I submit a request to create a program")]
        public void WhenISubmitARequestToCreateAProgram()
        {
            var program = ScenarioContext.Current.Get<SaveProgramRequest>("programRequest");
            var response = ApiFeature.ApiTestHost.Client.PostAsync(_leadingPath, program, new JsonMediaTypeFormatter()).Result;

            if (ScenarioContext.Current.ContainsKey("createProgramResponse"))
            {
                ScenarioContext.Current.Remove("createProgramResponse");
            }

            ScenarioContext.Current.Add("createProgramResponse", response);


            if (ScenarioContext.Current.ContainsKey("responseToValidate"))
            {
                ScenarioContext.Current.Remove("responseToValidate");
            }
            ScenarioContext.Current.Add("responseToValidate", response);

        }

        [When(@"I modify the program info to reflect the following:")]
        public void WhenIModifyTheProgramInfoToReflectTheFollowing(Table table)
        {
            var editProgramRequest = new SaveProgramRequest
                {
                    Name = table.Rows[0]["Name"] + ScenarioContext.Current.Get<long>("ticks"),
                    Description = table.Rows[0]["Description"],
                    TenantId = "1"
                };

            ScenarioContext.Current.Add("editProgramRequest", editProgramRequest);
            var response = ScenarioContext.Current.Get<HttpResponseMessage>("createProgramResponse");
            var programResponseInfo = response.Content.ReadAsAsync<ProgramResponse>().Result;

            var result = ApiFeature.ApiTestHost.Client.PutAsync(_leadingPath + "/" + programResponseInfo.Id, editProgramRequest, new JsonMediaTypeFormatter()).Result;
            ScenarioContext.Current.Add("editProgramResponse", result);
            ScenarioContext.Current.Add("programId", programResponseInfo.Id);

            if (ScenarioContext.Current.ContainsKey("responseToValidate"))
            {
                ScenarioContext.Current.Remove("responseToValidate");
            }

            ScenarioContext.Current.Add("responseToValidate", result);
        }

        [When(@"I delete the program")]
        public void WhenIDeleteTheProgram()
        {
            var response = ScenarioContext.Current.Get<HttpResponseMessage>("createProgramResponse");
            var programInfoResponse = response.Content.ReadAsAsync<ProgramResponse>().Result;

            ScenarioContext.Current.Add("programId", programInfoResponse.Id);
            var delete = ApiFeature.ApiTestHost.Client.DeleteAsync(_leadingPath + "/" + programInfoResponse.Id).Result;
            delete.EnsureSuccessStatusCode();
        }

        [When(@"I create a new program with (.*), (.*)")]
        public void WhenICreateANewProgramWith(string name, string description)
        {
            var programRequest = new SaveProgramRequest
                {
                    Name = string.IsNullOrEmpty(name) ? name : name + ScenarioContext.Current.Get<long>("ticks"),
                    Description = description,
                    TenantId =  "1"
                };

            if (ScenarioContext.Current.ContainsKey("programRequest"))
            {
                ScenarioContext.Current.Remove("programRequest");
            }

            ScenarioContext.Current.Add("programRequest", programRequest);
        }

        [When(@"I wish to add the same program to another tenant")]
        public void WhenIWishToAddTheSameProgramToAnotherTenant()
        {
            
        }

        [When(@"I request a program id that does not exist")]
        public void WhenIRequestAProgramIdThatDoesNotExist()
        {
            const string nonExistentId = "4DE2024C-4C81-94CD-2BA7-A1AA0095359F";
            var result = ApiFeature.ApiTestHost.Client.GetAsync(_leadingPath + "/" + nonExistentId).Result;
            ScenarioContext.Current.Add("responseToValidate", result);
        }

        [When(@"I submit another request to create another program")]
        public void WhenISubmitAnotherRequestToCreateAnotherProgram(Table table)
        {
            //GivenIHaveAProgramWithFollowingInfo(table);
            var anotherProgramRequest = new SaveProgramRequest
                {
                    Name = table.Rows[0]["Name"] + ScenarioContext.Current.Get<long>("ticks"),
                    Description = table.Rows[0]["Description"],
                    TenantId = "1"
                };

            ScenarioContext.Current.Add("anotherProgramRequest", anotherProgramRequest);

            var program = ScenarioContext.Current.Get<SaveProgramRequest>("anotherProgramRequest");
            var response = ApiFeature.ApiTestHost.Client.PostAsync(_leadingPath, program, new JsonMediaTypeFormatter()).Result;

            ScenarioContext.Current.Add("anotherProgramResponse", response);
        }


        [When(@"I request to get all programs")]
        public void WhenIRequestToGetAllPrograms()
        {
            var getAll = ApiFeature.ApiTestHost.Client.GetAsync(_leadingPath).Result;
            getAll.EnsureSuccessStatusCode();

            var getAllData = getAll.Content.ReadAsAsync<List<ProgramResponse>>().Result;
            
            ScenarioContext.Current.Add("allPrograms", getAllData);
        }

        [Then(@"my program is returned")]
        public void ThenMyProgramIsReturned()
        {
            var firstRequest = ScenarioContext.Current.Get<HttpResponseMessage>("createProgramResponse");
            var secondRequest = ScenarioContext.Current.Get<HttpResponseMessage>("anotherProgramResponse");

            var originalRequest1 = firstRequest.Content.ReadAsAsync<ProgramResponse>().Result;
            var originalRequest2 = secondRequest.Content.ReadAsAsync<ProgramResponse>().Result;

            var allPrograms = ScenarioContext.Current.Get<List<ProgramResponse>>("allPrograms");

            Assert.That(allPrograms.Any(p=> p.Id == originalRequest1.Id));
            Assert.That(allPrograms.Any(p => p.Id == originalRequest2.Id));

//            allPrograms.ForEach(x => Assert.That(listOfTestIds.Contains(x.Id)));
//            Assert.That(allPrograms.Count, Is.GreaterThanOrEqualTo(listOfTestIds.Count()));            
        }

        
        [Then(@"the operation is successful")]
        public void ThenTheOperationIsSuccessful()
        {
            var programResponse = ScenarioContext.Current.Get<HttpResponseMessage>("responseToValidate");
            programResponse.EnsureSuccessStatusCode();
        }

        [Then(@"my program information is as follows:")]
        public void ThenMyProgramInformationIsAsFollows(Table table)
        {
            //Get GUID from program response
            var message = ScenarioContext.Current.Get<HttpResponseMessage>("createProgramResponse");
            var programInfo = message.Content.ReadAsAsync<ProgramResponse>().Result;
            var programId = programInfo.Id;

            //Perform a GET call with GUID
            var result = ApiFeature.ApiTestHost.Client.GetAsync(_leadingPath + "/" + programId).Result;
            result.EnsureSuccessStatusCode();

            //Assert that the properties of the program info response from the GET equals the original program request properties
            var originalProgramRequest = ScenarioContext.Current.Get<SaveProgramRequest>("programRequest");
            var latestRequest = result.Content.ReadAsAsync<ProgramResponse>().Result;

            Assert.That(latestRequest.Name, Is.EqualTo(originalProgramRequest.Name));
            Assert.That(latestRequest.Description, Is.EqualTo(originalProgramRequest.Description));
        }


        [Then(@"my program information is changed")]
        public void ThenMyProgramInformationIsChanged()
        {
            var programId = ScenarioContext.Current.Get<Guid>("programId");
            var response = ApiFeature.ApiTestHost.Client.GetAsync(_leadingPath + "/" + programId).Result;
            response.EnsureSuccessStatusCode();

            var programInfo = response.Content.ReadAsAsync<ProgramResponse>().Result;
            var editedRequest = ScenarioContext.Current.Get<SaveProgramRequest>("editProgramRequest");

            Assert.AreEqual(programInfo.Name, editedRequest.Name);
            Assert.AreEqual(programInfo.Description, editedRequest.Description);
        }

        [Then(@"the program no longer exists")]
        public void ThenTheProgramNoLongerExists()
        {
            var programId = ScenarioContext.Current.Get<Guid>("programId");
            var getResponse = ApiFeature.ApiTestHost.Client.GetAsync(_leadingPath + "/" + programId).Result;

            Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Then(@"I should get the expected status code (.*)")]
        public void ThenIShouldGetTheExpectedStatusCodeBadRequest(string status)
        {
            var response = ScenarioContext.Current.Get<HttpResponseMessage>("responseToValidate");
            var expectedCode = (HttpStatusCode) Enum.Parse(typeof (HttpStatusCode), status);

            Assert.That(response.StatusCode, Is.EqualTo(expectedCode));
        }
    }
}