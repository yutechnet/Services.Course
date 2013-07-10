﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.18047
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
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Create a course")]
        public virtual void CreateACourse()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Create a course", ((string[])(null)));
#line 7
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "Tenant Id",
                        "OrganizationId",
                        "CourseType",
                        "IsTemplate"});
            table1.AddRow(new string[] {
                        "English 101",
                        "ENG101",
                        "Ranji\'s awesome English Class",
                        "999999",
                        "C3885307-BDAD-480F-8E7C-51DFE5D80387",
                        "Traditional",
                        "false"});
#line 8
 testRunner.Given("I have a course with following info:", ((string)(null)), table1, "Given ");
#line 11
 testRunner.When("I submit a creation request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 12
 testRunner.Then("I should get a success confirmation message", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Edit a course")]
        public virtual void EditACourse()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Edit a course", ((string[])(null)));
#line 14
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "Tenant Id",
                        "OrganizationId",
                        "CourseType",
                        "IsTemplate"});
            table2.AddRow(new string[] {
                        "English 101",
                        "ENG101",
                        "Ranji\'s awesome English Class",
                        "999999",
                        "C3885307-BDAD-480F-8E7C-51DFE5D80387",
                        "Traditional",
                        "false"});
#line 15
 testRunner.Given("I have a course with following info:", ((string)(null)), table2, "Given ");
#line 18
 testRunner.When("I submit a creation request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "Tenant Id",
                        "OrganizationId",
                        "CourseType",
                        "IsTemplate"});
            table3.AddRow(new string[] {
                        "English 101",
                        "ENG101",
                        "Ranji\'s awesome English Class",
                        "999999",
                        "C3885307-BDAD-480F-8E7C-51DFE5D80387",
                        "Traditional",
                        "false"});
#line 19
 testRunner.And("I change the info to reflect the following:", ((string)(null)), table3, "And ");
#line 22
 testRunner.Then("I should get a success confirmation message", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 23
 testRunner.And("my course info is changed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Delete a course")]
        public virtual void DeleteACourse()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Delete a course", ((string[])(null)));
#line 25
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "Tenant Id",
                        "OrganizationId",
                        "CourseType",
                        "IsTemplate"});
            table4.AddRow(new string[] {
                        "English 101",
                        "ENG101",
                        "Ranji\'s awesome English Class",
                        "999999",
                        "C3885307-BDAD-480F-8E7C-51DFE5D80387",
                        "Traditional",
                        "false"});
#line 26
 testRunner.Given("I have an existing course with following info:", ((string)(null)), table4, "Given ");
#line 29
 testRunner.And("I delete this course", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 30
 testRunner.Then("I should get a success confirmation message", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 31
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
#line 33
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "Tenant Id",
                        "OrganizationId",
                        "CourseType",
                        "IsTemplate"});
            table5.AddRow(new string[] {
                        "English 101",
                        "ENG101",
                        "Ranji\'s awesome English Class",
                        "999999",
                        "C3885307-BDAD-480F-8E7C-51DFE5D80387",
                        "Traditional",
                        "false"});
#line 34
 testRunner.Given("I have an existing course with following info:", ((string)(null)), table5, "Given ");
#line 37
 testRunner.When(string.Format("I create a new course with {0}, {1}, {2}", name, code, description), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 38
 testRunner.And("I submit a creation request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 39
 testRunner.Then(string.Format("I should get the status code {0}", status), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("cannot create a course with missing data")]
        [NUnit.Framework.TestCaseAttribute("Psychology 101", "", "Amro\'s another awesome Psychology class", "BadRequest", null)]
        [NUnit.Framework.TestCaseAttribute("", "PSY101", "Amro\'s another awesome Psychology class", "BadRequest", null)]
        [NUnit.Framework.TestCaseAttribute("Physcology 103", "PSY103", "", "Created", null)]
        public virtual void CannotCreateACourseWithMissingData(string name, string code, string description, string status, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("cannot create a course with missing data", exampleTags);
#line 46
this.ScenarioSetup(scenarioInfo);
#line 47
 testRunner.When(string.Format("I create a new course with {0}, {1}, {2}", name, code, description), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 48
 testRunner.And("I submit a creation request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 49
 testRunner.Then(string.Format("I should get the status code {0}", status), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Return course by partial name")]
        public virtual void ReturnCourseByPartialName()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Return course by partial name", ((string[])(null)));
#line 57
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "Tenant Id",
                        "OrganizationId",
                        "CourseType",
                        "IsTemplate"});
            table6.AddRow(new string[] {
                        "English 101",
                        "ENGL101",
                        "Learn to read and write",
                        "999999",
                        "C3885307-BDAD-480F-8E7C-51DFE5D80387",
                        "Traditional",
                        "false"});
            table6.AddRow(new string[] {
                        "Engineering 200",
                        "ENG200",
                        "If you build it, they will come",
                        "999999",
                        "C3885307-BDAD-480F-8E7C-51DFE5D80387",
                        "Traditional",
                        "false"});
            table6.AddRow(new string[] {
                        "English 220",
                        "ENGL220",
                        "Essays",
                        "999999",
                        "C3885307-BDAD-480F-8E7C-51DFE5D80387",
                        "Traditional",
                        "false"});
            table6.AddRow(new string[] {
                        "Philosophy 100",
                        "PHIL100",
                        "To be, or not to be",
                        "999999",
                        "C3885307-BDAD-480F-8E7C-51DFE5D80387",
                        "Traditional",
                        "false"});
            table6.AddRow(new string[] {
                        "Philanthropy 101",
                        "PHILA101",
                        "Don\'t be greedy",
                        "999999",
                        "C3885307-BDAD-480F-8E7C-51DFE5D80387",
                        "Traditional",
                        "false"});
            table6.AddRow(new string[] {
                        "Chemistry 350",
                        "CHEM350",
                        "Periodic table of elements to the max",
                        "999999",
                        "C3885307-BDAD-480F-8E7C-51DFE5D80387",
                        "Traditional",
                        "false"});
#line 58
 testRunner.Given("I have existing courses with following info:", ((string)(null)), table6, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "Operation",
                        "Argument",
                        "Count"});
            table7.AddRow(new string[] {
                        "Eq",
                        "Engineering%20200",
                        "1"});
            table7.AddRow(new string[] {
                        "StartsWith",
                        "Eng",
                        "3"});
            table7.AddRow(new string[] {
                        "StartsWith",
                        "Phil",
                        "2"});
            table7.AddRow(new string[] {
                        "StartsWith",
                        "Engl",
                        "2"});
            table7.AddRow(new string[] {
                        "StartsWith",
                        "Philo",
                        "1"});
            table7.AddRow(new string[] {
                        "StartsWith",
                        "MATH",
                        "0"});
            table7.AddRow(new string[] {
                        "StartsWith",
                        "Chemistry",
                        "1"});
            table7.AddRow(new string[] {
                        "StartsWith",
                        "En*",
                        "0"});
            table7.AddRow(new string[] {
                        "StartsWith",
                        "\\",
                        "0"});
            table7.AddRow(new string[] {
                        "StartsWith",
                        "\\",
                        "0"});
            table7.AddRow(new string[] {
                        "StartsWith",
                        "\'\'E\'\'",
                        "0"});
            table7.AddRow(new string[] {
                        "StartsWith",
                        "C*%23",
                        "0"});
            table7.AddRow(new string[] {
                        "StartsWith",
                        "A%26",
                        "0"});
#line 66
 testRunner.Then("the course name counts are as follows:", ((string)(null)), table7, "Then ");
#line 81
 testRunner.Then("the course count is atleast \'6\' when search term is \'\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Add organization id to a course")]
        public virtual void AddOrganizationIdToACourse()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Add organization id to a course", ((string[])(null)));
#line 83
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "Tenant Id",
                        "OrganizationId",
                        "CourseType",
                        "IsTemplate"});
            table8.AddRow(new string[] {
                        "English 101",
                        "ENG101",
                        "Ranji\'s awesome English Class",
                        "999999",
                        "C3885307-BDAD-480F-8E7C-51DFE5D80387",
                        "Traditional",
                        "false"});
#line 84
 testRunner.Given("I have a course with following info:", ((string)(null)), table8, "Given ");
#line 87
 testRunner.When("I submit a creation request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 88
 testRunner.Then("the organization id is returned as part of the request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
