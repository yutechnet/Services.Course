﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text.RegularExpressions;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Host.Tests.Integration.Operations;
using BpeProducts.Services.Course.Host.Tests.Integration.StepSetups;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace BpeProducts.Services.Course.Host.Tests.Integration
{
    [Binding]
    public class CourseVersioningSteps
    {
        private const int Tenant = 999999;

        [Given(@"I create the following course")]
        public void GivenICreateTheFollowingCourse(Table table)
        {
            var postUri = FeatureContext.Current.Get<string>("CourseLeadingPath");
            var saveCourseRequest = table.CreateInstance<SaveCourseRequest>();
            saveCourseRequest.TenantId = Tenant;

            var response =
                ApiFeature.ApiTestHost.Client.PostAsync(postUri, saveCourseRequest, new JsonMediaTypeFormatter()).Result;
            response.EnsureSuccessStatusCode();

            ScenarioContext.Current.Add(saveCourseRequest.Name, response.Headers.Location);
        }
    }
}
