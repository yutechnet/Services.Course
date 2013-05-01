﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Host.Controllers;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace BpeProducts.Services.Course.Host.Tests.Integration
{
    [Binding]
    public class ProgramAssociationSteps
    {
        private string _coursesLeadingPath;
        private string _programsLeadingPath;

        public ProgramAssociationSteps()
        {
            var targetUri = new Uri(ConfigurationManager.AppSettings["TestHostBaseAddress"]);
            if (!targetUri.Host.Equals("localhost"))
            {
                _coursesLeadingPath = targetUri.PathAndQuery + "/courses";
                _programsLeadingPath = targetUri.PathAndQuery + "/programs";
            }
            else
            {
                _coursesLeadingPath = "/courses";
                _programsLeadingPath = "/programs";
            }
        }

        [Given(@"the following programs exist:")]
        public void GivenTheFollowingProgramsExist(Table table)
        {
            foreach (var row in table.Rows)
            {
                var saveProgramRequest = new SaveProgramRequest
                    {
                        Description = row["Description"],
                        Name = row["Name"],
                        TenantId = "1"
                    };
                CreateProgram(saveProgramRequest);
            }
        }
        
        [Given(@"the following courses exist:")]
        public void GivenTheFollowingCoursesExist(Table table)
        {
            foreach (var row in table.Rows)
            {
                var saveCourseRequest = new SaveCourseRequest
                    {
                        Code = row["Code"],
                        Description = row["Description"],
                        Name = row["Name"],
                        TenantId = 1
                    };
                CreateCourse(saveCourseRequest);
            }
        }

        [Given(@"I associate '(.*)' course with the following programs:")]
        public void GivenIAssociateCourseWithTheFollowingPrograms(string courseName, Table table)
        {
            WhenIAssociateCourseWithTheFollowingPrograms(courseName, table);
        }
        
        [When(@"I associate '(.*)' course with '(.*)' program")]
        public void WhenIAssociateCourseWithProgram(string courseName, string programName)
        {
            var courseInfo = ScenarioContext.Current.Get<CourseInfoResponse>(courseName);
            var saveCourseRequest = new SaveCourseRequest
            {
                Code = courseInfo.Code,
                Description = courseInfo.Description,
                Id = courseInfo.Id,
                Name = courseInfo.Name,
                ProgramIds = new List<Guid>(),
                TenantId = 1
            };
            var programs = ScenarioContext.Current.Get<List<ProgramResponse>>("programs");
            saveCourseRequest.ProgramIds.Add(programs.Find(p => p.Name == programName).Id);
            var response = ApiFeature.ApiTestHost.Client.PutAsync(_coursesLeadingPath + "/" + courseInfo.Id, saveCourseRequest, new JsonMediaTypeFormatter()).Result;
            response.EnsureSuccessStatusCode();
        }

        [When(@"I associate '(.*)' course with the following programs:")]
        public void WhenIAssociateCourseWithTheFollowingPrograms(string courseName, Table table)
        {
            var courseInfo = ScenarioContext.Current.Get<CourseInfoResponse>(courseName);
            var saveCourseRequest = new SaveCourseRequest
            {
                Code = courseInfo.Code,
                Description = courseInfo.Description,
                Id = courseInfo.Id,
                Name = courseInfo.Name,
                ProgramIds = new List<Guid>(),
                TenantId = 1
            };
            var programs = ScenarioContext.Current.Get<List<ProgramResponse>>("programs");
            foreach (var row in table.Rows)
            {
                saveCourseRequest.ProgramIds.Add(programs.Find(p => p.Name == row["Program Name"]).Id);
            }
            var response = ApiFeature.ApiTestHost.Client.PutAsync(_coursesLeadingPath + "/" + courseInfo.Id, saveCourseRequest, new JsonMediaTypeFormatter()).Result;
            response.EnsureSuccessStatusCode();
        }

        [When(@"I remove '(.*)' course from '(.*)'")]
        public void WhenIRemoveCourseFrom(string courseName, string programName)
        {
            var courseInfo = ScenarioContext.Current.Get<CourseInfoResponse>(courseName);
            courseInfo = GetCourseById(courseInfo.Id).Content.ReadAsAsync<CourseInfoResponse>().Result;
            var saveCourseRequest = new SaveCourseRequest
            {
                Code = courseInfo.Code,
                Description = courseInfo.Description,
                Id = courseInfo.Id,
                Name = courseInfo.Name,
                ProgramIds = new List<Guid>(),
                TenantId = 1
            };

            // Find out the id of the program to remove
            var programs = ScenarioContext.Current.Get<List<ProgramResponse>>("programs");
            var programToRemove = programs.First(p => p.Name.Equals(programName));

            foreach (var programId in courseInfo.ProgramIds)
            {
                if (programId != programToRemove.Id)
                {
                    saveCourseRequest.ProgramIds.Add(programId);
                }
            }
            // Save
            var response = ApiFeature.ApiTestHost.Client.PutAsync(_coursesLeadingPath + "/" + courseInfo.Id, saveCourseRequest, new JsonMediaTypeFormatter()).Result;
            response.EnsureSuccessStatusCode();

            // ScenarioContext.Current.Add(courseName, response.Content.ReadAsAsync<CourseInfoResponse>().Result);
        }

        [Then(@"the course '(.*)' includes '(.*)' program association")]
        public void ThenTheCourseIncludesProgramAssociation(string courseName, string programName)
        {
            var createCourseResponse = ScenarioContext.Current.Get<CourseInfoResponse>(courseName);
            var courseInfoResponse = GetCourseById(createCourseResponse.Id).Content.ReadAsAsync<CourseInfoResponse>().Result;

            var programs = ScenarioContext.Current.Get<IList<ProgramResponse>>("programs");
            var program = programs.First(p => p.Name.Equals(programName));

            Assert.That(courseInfoResponse.ProgramIds.Count, Is.EqualTo(1));
            CollectionAssert.Contains(courseInfoResponse.ProgramIds, program.Id);
        }

        [Then(@"the course '(.*)' includes the following program information:")]
        public void ThenTheCourseIncludesTheFollowingProgramInformation(string courseName, Table table)
        {
            var createCourseResponse = ScenarioContext.Current.Get<CourseInfoResponse>(courseName);
            var courseInfoResponse = GetCourseById(createCourseResponse.Id).Content.ReadAsAsync<CourseInfoResponse>().Result;

            var programs = ScenarioContext.Current.Get<IList<ProgramResponse>>("programs");
            var programIds = (from programResponse in programs select programResponse.Id).ToList(); 

            Assert.That(programIds.Count, Is.EqualTo(courseInfoResponse.ProgramIds.Count));
            foreach (var programId in courseInfoResponse.ProgramIds)
            {
                CollectionAssert.Contains(programIds, programId);
            }
        }

        private void CreateCourse(SaveCourseRequest saveCourseRequest)
        {
            var response = ApiFeature.ApiTestHost.Client.PostAsync(_coursesLeadingPath, saveCourseRequest, new JsonMediaTypeFormatter()).Result;
            response.EnsureSuccessStatusCode();
            var course = response.Content.ReadAsAsync<CourseInfoResponse>().Result;
            ScenarioContext.Current[course.Name] = course;
        }

        private void CreateProgram(SaveProgramRequest saveProgramRequest)
        {
            var response = ApiFeature.ApiTestHost.Client.PostAsync(_programsLeadingPath, saveProgramRequest, new JsonMediaTypeFormatter()).Result;
            response.EnsureSuccessStatusCode();
            var program = response.Content.ReadAsAsync<ProgramResponse>().Result;

            IList<ProgramResponse> programs = ScenarioContext.Current.ContainsKey("programs")
                                                   ? ScenarioContext.Current.Get<List<ProgramResponse>>("programs")
                                                   : new List<ProgramResponse>();
            programs.Add(program);
            ScenarioContext.Current["programs"] = programs;

        }

        private HttpResponseMessage GetCourseByName(string courseName)
        {
            var response = ApiFeature.ApiTestHost.Client.GetAsync(_coursesLeadingPath + "?name=" + courseName).Result;
            response.EnsureSuccessStatusCode();
            return response;
        }

        private HttpResponseMessage GetCourseById(Guid courseId)
        {
            var response = ApiFeature.ApiTestHost.Client.GetAsync(_coursesLeadingPath + "/" + courseId).Result;
            response.EnsureSuccessStatusCode();
            return response;
        }

        private HttpResponseMessage GetProgramByName(string programName)
        {
            var response =
                ApiFeature.ApiTestHost.Client.GetAsync(_coursesLeadingPath + 
                    string.Format("?$filter=substringof(Name,'{0}'", programName))
                    .Result;
            response.EnsureSuccessStatusCode();
            return response;
        }
    }
}
