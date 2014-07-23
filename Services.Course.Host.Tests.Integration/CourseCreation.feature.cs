﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.18010
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
#line 6
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name"});
            table1.AddRow(new string[] {
                        "COB"});
#line 7
 testRunner.Given("the following organizations exist", ((string)(null)), table1, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Capability"});
            table2.AddRow(new string[] {
                        "CourseCreate"});
            table2.AddRow(new string[] {
                        "CoursePublish"});
            table2.AddRow(new string[] {
                        "CourseView"});
            table2.AddRow(new string[] {
                        "EditCourse"});
#line 10
 testRunner.And("I have the following capabilities", ((string)(null)), table2, "And ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Create a course")]
        public virtual void CreateACourse()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Create a course", ((string[])(null)));
#line 16
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "Tenant Id",
                        "OrganizationName",
                        "CourseType",
                        "IsTemplate",
                        "Credit",
                        "MetaData",
                        "ExtensionAssets"});
            table3.AddRow(new string[] {
                        "English 101",
                        "ENG101",
                        "Ranji\'s awesome English Class",
                        "999999",
                        "COB",
                        "Traditional",
                        "false",
                        "5",
                        "{someData}",
                        "B40CE4F4-434A-4987-80A8-58F795C212EB,6B7D1752-2A8D-4848-B8BC-1B1E42164499"});
#line 17
 testRunner.Given("I have a course with following info:", ((string)(null)), table3, "Given ");
#line 20
 testRunner.When("I submit a creation request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 21
 testRunner.Then("I get \'Created\' response", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Edit a course")]
        public virtual void EditACourse()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Edit a course", ((string[])(null)));
#line 23
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "Tenant Id",
                        "OrganizationName",
                        "CourseType",
                        "IsTemplate",
                        "Credit",
                        "MetaData",
                        "ExtensionAssets"});
            table4.AddRow(new string[] {
                        "English 101",
                        "ENG101",
                        "Ranji\'s awesome English Class",
                        "999999",
                        "COB",
                        "Traditional",
                        "false",
                        "5",
                        "{someData}",
                        "B40CE4F4-434A-4987-80A8-58F795C212EB"});
#line 24
 testRunner.Given("I have a course with following info:", ((string)(null)), table4, "Given ");
#line 27
 testRunner.When("I submit a creation request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "CourseType",
                        "IsTemplate",
                        "Credit",
                        "MetaData",
                        "ExtensionAssets"});
            table5.AddRow(new string[] {
                        "English 101",
                        "ENG101",
                        "John\'s awesome English Class",
                        "Traditional",
                        "false",
                        "10",
                        "{differentData}",
                        "B40CE4F4-434A-4987-80A8-58F795C212EB,6B7D1752-2A8D-4848-B8BC-1B1E42164499"});
#line 28
 testRunner.And("I change the info to reflect the following:", ((string)(null)), table5, "And ");
#line 31
 testRunner.Then("I get \'NoContent\' response", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 32
 testRunner.And("my course info is changed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Delete a course")]
        public virtual void DeleteACourse()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Delete a course", ((string[])(null)));
#line 34
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "Tenant Id",
                        "OrganizationName",
                        "CourseType",
                        "IsTemplate",
                        "MetaData",
                        "ExtensionAssets"});
            table6.AddRow(new string[] {
                        "English 101",
                        "ENG101",
                        "Ranji\'s awesome English Class",
                        "999999",
                        "COB",
                        "Traditional",
                        "false",
                        "{someData}",
                        "B40CE4F4-434A-4987-80A8-58F795C212EB"});
#line 35
 testRunner.Given("I have an existing course with following info:", ((string)(null)), table6, "Given ");
#line 38
 testRunner.And("I delete this course", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 39
 testRunner.Then("I get \'NoContent\' response", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 40
 testRunner.And("my course no longer exists", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Can create a course with same name or code")]
        [NUnit.Framework.TestCaseAttribute("Psychology 101", "PSY102", "Amro\'s another awesome Psychology class", "Created", "COB", null)]
        [NUnit.Framework.TestCaseAttribute("Psychology 102", "PSY101", "Amro\'s another awesome Psychology class", "Created", "COB", null)]
        public virtual void CanCreateACourseWithSameNameOrCode(string name, string code, string description, string status, string organizationName, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Can create a course with same name or code", exampleTags);
#line 42
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "Tenant Id",
                        "OrganizationName",
                        "CourseType",
                        "IsTemplate",
                        "MetaData",
                        "ExtensionAssets"});
            table7.AddRow(new string[] {
                        "English 101",
                        "ENG101",
                        "Ranji\'s awesome English Class",
                        "999999",
                        "COB",
                        "Traditional",
                        "false",
                        "{someData}",
                        "B40CE4F4-434A-4987-80A8-58F795C212EB"});
#line 43
 testRunner.Given("I have an existing course with following info:", ((string)(null)), table7, "Given ");
#line 46
 testRunner.When(string.Format("I create a new course with {0}, {1}, {2}, {3}", name, code, description, organizationName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 47
 testRunner.And("I submit a creation request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 48
 testRunner.Then(string.Format("I get \'{0}\' response", status), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("cannot create a course with missing data")]
        [NUnit.Framework.TestCaseAttribute("Psychology 101", "", "Amro\'s another awesome Psychology class", "COB", "BadRequest", null)]
        [NUnit.Framework.TestCaseAttribute("", "PSY101", "Amro\'s another awesome Psychology class", "COB", "BadRequest", null)]
        [NUnit.Framework.TestCaseAttribute("Physcology 103", "PSY103", "", "COB", "Created", null)]
        public virtual void CannotCreateACourseWithMissingData(string name, string code, string description, string organizationName, string status, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("cannot create a course with missing data", exampleTags);
#line 55
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 56
 testRunner.When(string.Format("I create a new course with {0}, {1}, {2}, {3}", name, code, description, organizationName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 57
 testRunner.And("I submit a creation request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 58
 testRunner.Then(string.Format("I get \'{0}\' response", status), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Return course by partial name")]
        public virtual void ReturnCourseByPartialName()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Return course by partial name", ((string[])(null)));
#line 69
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "Tenant Id",
                        "OrganizationName",
                        "CourseType",
                        "IsTemplate"});
            table8.AddRow(new string[] {
                        "English 101",
                        "ENGL101",
                        "Learn to read and write",
                        "999999",
                        "COB",
                        "Traditional",
                        "false"});
            table8.AddRow(new string[] {
                        "Engineering 200",
                        "ENG200",
                        "If you build it, they will come",
                        "999999",
                        "COB",
                        "Traditional",
                        "false"});
            table8.AddRow(new string[] {
                        "English 220",
                        "ENGL220",
                        "Essays",
                        "999999",
                        "COB",
                        "Traditional",
                        "false"});
            table8.AddRow(new string[] {
                        "Philosophy 100",
                        "PHIL100",
                        "To be, or not to be",
                        "999999",
                        "COB",
                        "Traditional",
                        "false"});
            table8.AddRow(new string[] {
                        "Philanthropy 101",
                        "PHILA101",
                        "Don\'t be greedy",
                        "999999",
                        "COB",
                        "Traditional",
                        "false"});
            table8.AddRow(new string[] {
                        "Chemistry 350",
                        "CHEM350",
                        "Periodic table of elements to the max",
                        "999999",
                        "COB",
                        "Traditional",
                        "false"});
#line 70
 testRunner.Given("I have existing courses with following info:", ((string)(null)), table8, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                        "Operation",
                        "Argument",
                        "Count"});
            table9.AddRow(new string[] {
                        "Eq",
                        "Engineering%20200",
                        "1"});
            table9.AddRow(new string[] {
                        "StartsWith",
                        "Eng",
                        "3"});
            table9.AddRow(new string[] {
                        "StartsWith",
                        "Phil",
                        "2"});
            table9.AddRow(new string[] {
                        "StartsWith",
                        "Engl",
                        "2"});
            table9.AddRow(new string[] {
                        "StartsWith",
                        "Philo",
                        "1"});
            table9.AddRow(new string[] {
                        "StartsWith",
                        "MATH",
                        "0"});
            table9.AddRow(new string[] {
                        "StartsWith",
                        "Chemistry",
                        "1"});
            table9.AddRow(new string[] {
                        "StartsWith",
                        "En*",
                        "0"});
            table9.AddRow(new string[] {
                        "StartsWith",
                        "\\",
                        "0"});
            table9.AddRow(new string[] {
                        "StartsWith",
                        "\\",
                        "0"});
            table9.AddRow(new string[] {
                        "StartsWith",
                        "\'\'E\'\'",
                        "0"});
            table9.AddRow(new string[] {
                        "StartsWith",
                        "C*%23",
                        "0"});
            table9.AddRow(new string[] {
                        "StartsWith",
                        "A%26",
                        "0"});
#line 78
 testRunner.Then("the course name counts are as follows:", ((string)(null)), table9, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Add organization id to a course")]
        public virtual void AddOrganizationIdToACourse()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Add organization id to a course", ((string[])(null)));
#line 94
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table10 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "Tenant Id",
                        "OrganizationName",
                        "CourseType",
                        "IsTemplate",
                        "MetaData",
                        "ExtensionAssets"});
            table10.AddRow(new string[] {
                        "English 101",
                        "ENG101",
                        "Ranji\'s awesome English Class",
                        "999999",
                        "COB",
                        "Traditional",
                        "false",
                        "{someData}",
                        "B40CE4F4-434A-4987-80A8-58F795C212EB"});
#line 95
 testRunner.Given("I have a course with following info:", ((string)(null)), table10, "Given ");
#line 98
 testRunner.When("I submit a creation request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 99
 testRunner.Then("the organization id is returned as part of the request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
