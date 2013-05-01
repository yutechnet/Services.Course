﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.18034
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace BpeProducts.Services.Course.Host.Tests.Integration
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Course Management")]
    [NUnit.Framework.CategoryAttribute("Api")]
    public partial class CourseManagementFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "CourseCreation.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Course Management", "In order to publish a course\r\nAs a course builder\r\nI want to create, edit and del" +
                    "ete a course", ProgrammingLanguage.CSharp, new string[] {
                        "Api"});
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void FeatureBackground()
        {
#line 7
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Create a course")]
        public virtual void CreateACourse()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Create a course", ((string[])(null)));
#line 9
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description"});
            table1.AddRow(new string[] {
                        "English 101",
                        "ENG101",
                        "Ranji\'s awesome English Class"});
#line 10
 testRunner.Given("I have a course with following info:", ((string)(null)), table1, "Given ");
#line 13
 testRunner.When("I submit a creation request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 14
 testRunner.Then("I should get a success confirmation message", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Return course by course name")]
        public virtual void ReturnCourseByCourseName()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Return course by course name", ((string[])(null)));
#line 16
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "Tenant Id"});
            table2.AddRow(new string[] {
                        "English 101",
                        "ENG101",
                        "Ranji\'s awesome English Class",
                        "1"});
#line 17
 testRunner.Given("I have a course with following info:", ((string)(null)), table2, "Given ");
#line 20
 testRunner.When("I submit a creation request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 21
 testRunner.Then("I should get a success confirmation message", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 22
 testRunner.And("I can retrieve the course by course name", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Return course by course code")]
        public virtual void ReturnCourseByCourseCode()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Return course by course code", ((string[])(null)));
#line 24
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "Tenant Id"});
            table3.AddRow(new string[] {
                        "English 101",
                        "ENG101",
                        "Ranji\'s awesome English Class",
                        "1"});
#line 25
 testRunner.Given("I have a course with following info:", ((string)(null)), table3, "Given ");
#line 28
 testRunner.When("I submit a creation request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 29
 testRunner.Then("I should get a success confirmation message", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 30
 testRunner.And("I can retrieve the course by course code", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Return 404 when course name is not found")]
        public virtual void Return404WhenCourseNameIsNotFound()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Return 404 when course name is not found", ((string[])(null)));
#line 32
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "Tenant Id"});
            table4.AddRow(new string[] {
                        "English 101",
                        "ENG101",
                        "Ranji\'s awesome English Class",
                        "1"});
#line 33
 testRunner.Given("I have a course with following info:", ((string)(null)), table4, "Given ");
#line 36
 testRunner.When("I submit a creation request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 37
 testRunner.And("I request a course name that does not exist", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 38
 testRunner.Then("I should get a not found message returned", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Edit a course")]
        public virtual void EditACourse()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Edit a course", ((string[])(null)));
#line 40
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description"});
            table5.AddRow(new string[] {
                        "English 101",
                        "ENG101",
                        "Ranji\'s awesome English Class"});
#line 41
 testRunner.Given("I have a course with following info:", ((string)(null)), table5, "Given ");
#line 44
 testRunner.When("I submit a creation request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description"});
            table6.AddRow(new string[] {
                        "English 202",
                        "ENG202",
                        "Ranji\'s awesome English Class"});
#line 45
 testRunner.And("I change the info to reflect the following:", ((string)(null)), table6, "And ");
#line 48
 testRunner.Then("I should get a success confirmation message", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 49
 testRunner.And("my course info is changed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Delete a course")]
        public virtual void DeleteACourse()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Delete a course", ((string[])(null)));
#line 51
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description"});
            table7.AddRow(new string[] {
                        "Psychology 101",
                        "PSY101",
                        "Amro\'s awesome Psychology class"});
#line 52
 testRunner.Given("I have an existing course with following info:", ((string)(null)), table7, "Given ");
#line 55
 testRunner.And("I delete this course", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 56
 testRunner.Then("I should get a success confirmation message", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 57
 testRunner.And("my course no longer exists", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Can create a course with same name or code")]
        [NUnit.Framework.TestCaseAttribute("Psychology 101", "PSY102", "Amro\'s another awesome Psychology class", "Created", null)]
        [NUnit.Framework.TestCaseAttribute("Psychology 102", "PSY101", "Amro\'s another awesome Psychology class", "Created", null)]
        public virtual void CanCreateACourseWithSameNameOrCode(string name, string code, string description, string status, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Can create a course with same name or code", exampleTags);
#line 59
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description"});
            table8.AddRow(new string[] {
                        "Psychology 101",
                        "PSY101",
                        "Amro\'s awesome Psychology class"});
#line 60
 testRunner.Given("I have an existing course with following info:", ((string)(null)), table8, "Given ");
#line 63
 testRunner.When(string.Format("I create a new course with {0}, {1}, {2}", name, code, description), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 64
 testRunner.And("I submit a creation request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 65
 testRunner.Then(string.Format("I should get the status code {0}", status), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("cannot create a course with missing data")]
        [NUnit.Framework.TestCaseAttribute("Psychology 101", "", "Amro\'s another awesome Psychology class", "1", "BadRequest", null)]
        [NUnit.Framework.TestCaseAttribute("", "PSY101", "Amro\'s another awesome Psychology class", "1", "BadRequest", null)]
        [NUnit.Framework.TestCaseAttribute("Physcology 103", "PSY103", "", "1", "Created", null)]
        public virtual void CannotCreateACourseWithMissingData(string name, string code, string description, string tenantId, string status, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("cannot create a course with missing data", exampleTags);
#line 72
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 73
 testRunner.When(string.Format("I create a new course with {0}, {1}, {2}", name, code, description), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 74
 testRunner.And("I submit a creation request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 75
 testRunner.Then(string.Format("I should get the status code {0}", status), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
