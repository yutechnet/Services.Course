﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.18052
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
    [NUnit.Framework.DescriptionAttribute("ProgramAssociation")]
    [NUnit.Framework.CategoryAttribute("Api")]
    public partial class ProgramAssociationFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "ProgramAssociation.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "ProgramAssociation", "In order to add course to programs\r\nAs a course builder\r\nI want to associate cour" +
                    "se to programs", ProgrammingLanguage.CSharp, new string[] {
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
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Description",
                        "ProgramType",
                        "OrganizationId"});
            table1.AddRow(new string[] {
                        "Bachelor of Art",
                        "BA Program",
                        "BA",
                        "E2DF063D-E2A1-4F83-9BE0-218EC676C05F"});
            table1.AddRow(new string[] {
                        "Bachelor of Science",
                        "BS program",
                        "BS",
                        "E2DF063D-E2A1-4F83-9BE0-218EC676C05F"});
#line 8
 testRunner.Given("I have the following programs", ((string)(null)), table1, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "OrganizationId"});
            table2.AddRow(new string[] {
                        "English 101",
                        "ENG101",
                        "English 101",
                        "E2DF063D-E2A1-4F83-9BE0-218EC676C05F"});
            table2.AddRow(new string[] {
                        "Psychology 101",
                        "PSY101",
                        "Psychology 101",
                        "E2DF063D-E2A1-4F83-9BE0-218EC676C05F"});
#line 12
 testRunner.And("I have the following courses", ((string)(null)), table2, "And ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Associate a course with a program")]
        [NUnit.Framework.TestCaseAttribute("English 101", "Bachelor of Art", null)]
        [NUnit.Framework.TestCaseAttribute("Psychology 101", "Bachelor of Science", null)]
        [NUnit.Framework.TestCaseAttribute("English 101", "Bachelor of Science", null)]
        [NUnit.Framework.TestCaseAttribute("Psychology 101", "Bachelor of Art", null)]
        public virtual void AssociateACourseWithAProgram(string courseName, string programName, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Associate a course with a program", exampleTags);
#line 17
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 18
 testRunner.When(string.Format("I associate \'{0}\' course with \'{1}\' program", courseName, programName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 19
 testRunner.Then(string.Format("the course \'{0}\' includes \'{1}\' program association", courseName, programName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Associate a course with multiple programs")]
        public virtual void AssociateACourseWithMultiplePrograms()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Associate a course with multiple programs", ((string[])(null)));
#line 28
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Program Name"});
            table3.AddRow(new string[] {
                        "Bachelor of Art"});
            table3.AddRow(new string[] {
                        "Bachelor of Science"});
#line 29
 testRunner.When("I associate \'English 101\' course with the following programs", ((string)(null)), table3, "When ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Program Name"});
            table4.AddRow(new string[] {
                        "Bachelor of Art"});
            table4.AddRow(new string[] {
                        "Bachelor of Science"});
#line 33
 testRunner.Then("the course \'English 101\' includes the following program information:", ((string)(null)), table4, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Remove the course from the program")]
        public virtual void RemoveTheCourseFromTheProgram()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Remove the course from the program", ((string[])(null)));
#line 38
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Program Name"});
            table5.AddRow(new string[] {
                        "Bachelor of Art"});
            table5.AddRow(new string[] {
                        "Bachelor of Science"});
#line 39
 testRunner.When("I associate \'English 101\' course with the following programs", ((string)(null)), table5, "When ");
#line 43
 testRunner.And("I remove \'English 101\' course from \'Bachelor of Art\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 44
 testRunner.Then("the course \'English 101\' includes \'Bachelor of Science\' program association", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Get program with all courses that belong to it")]
        public virtual void GetProgramWithAllCoursesThatBelongToIt()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Get program with all courses that belong to it", ((string[])(null)));
#line 46
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Program Name"});
            table6.AddRow(new string[] {
                        "Bachelor of Art"});
#line 47
 testRunner.When("I associate \'English 101\' course with the following programs", ((string)(null)), table6, "When ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "Program Name"});
            table7.AddRow(new string[] {
                        "Bachelor of Art"});
#line 50
 testRunner.And("I associate \'Psychology 101\' course with the following programs", ((string)(null)), table7, "And ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "Course Name"});
            table8.AddRow(new string[] {
                        "English 101"});
            table8.AddRow(new string[] {
                        "Psychology 101"});
#line 53
 testRunner.Then("the program \'Bachelor of Art\' include the following course information:", ((string)(null)), table8, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
