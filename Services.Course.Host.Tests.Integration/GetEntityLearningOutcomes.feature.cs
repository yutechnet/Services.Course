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
    [NUnit.Framework.DescriptionAttribute("GetEntityLearningOutcome")]
    [NUnit.Framework.CategoryAttribute("Api")]
    public partial class GetEntityLearningOutcomeFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "GetEntityLearningOutcomes.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "GetEntityLearningOutcome", "Given a course, program or segment \r\nAs a program manager\r\nI want to know what Le" +
                    "arningOutcomes are associated with them", ProgrammingLanguage.CSharp, new string[] {
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
                        "Program1",
                        "Program1",
                        "BA",
                        "E2DF063D-E2A1-4F83-9BE0-218EC676C05F"});
            table1.AddRow(new string[] {
                        "Program2",
                        "Program2",
                        "BS",
                        "E2DF063D-E2A1-4F83-9BE0-218EC676C05F"});
            table1.AddRow(new string[] {
                        "Program3",
                        "Program3",
                        "MA",
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
                        "Course1",
                        "1",
                        "Course1",
                        "E2DF063D-E2A1-4F83-9BE0-218EC676C05F"});
            table2.AddRow(new string[] {
                        "Course2",
                        "2",
                        "Course2",
                        "E2DF063D-E2A1-4F83-9BE0-218EC676C05F"});
#line 13
 testRunner.And("I have the following courses", ((string)(null)), table2, "And ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Description",
                        "Type",
                        "ParentSegmentName"});
            table3.AddRow(new string[] {
                        "Week1",
                        "First week is slack time",
                        "TimeSpan",
                        ""});
            table3.AddRow(new string[] {
                        "Discussion",
                        "Discussion for the first week",
                        "Discussion",
                        "Week1"});
            table3.AddRow(new string[] {
                        "Discussion2",
                        "Discussion2 for the first week",
                        "Discussion",
                        "Week1"});
            table3.AddRow(new string[] {
                        "Topic",
                        "Topic for a discussion",
                        "Topic",
                        "Discussion"});
#line 17
    testRunner.And("I have the following course segments for \'Course1\'", ((string)(null)), table3, "And ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Description"});
            table4.AddRow(new string[] {
                        "LO1"});
            table4.AddRow(new string[] {
                        "LO2"});
            table4.AddRow(new string[] {
                        "LO3"});
            table4.AddRow(new string[] {
                        "LO4"});
            table4.AddRow(new string[] {
                        "LO5"});
#line 23
 testRunner.And("I have the following learning outcomes", ((string)(null)), table4, "And ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Can get entity learning outcomes for programs")]
        public virtual void CanGetEntityLearningOutcomesForPrograms()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Can get entity learning outcomes for programs", ((string[])(null)));
#line 31
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Description"});
            table5.AddRow(new string[] {
                        "LO1"});
#line 32
 testRunner.When("I associate the existing learning outcomes to \'Program1\' program", ((string)(null)), table5, "When ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Description"});
            table6.AddRow(new string[] {
                        "LO2"});
            table6.AddRow(new string[] {
                        "LO3"});
#line 35
 testRunner.And("I associate the existing learning outcomes to \'Program2\' program", ((string)(null)), table6, "And ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "EntityType",
                        "EntityName",
                        "LearningOutcomeName"});
            table7.AddRow(new string[] {
                        "Program",
                        "Program1",
                        "LO1"});
            table7.AddRow(new string[] {
                        "Program",
                        "Program2",
                        "LO1, LO2"});
            table7.AddRow(new string[] {
                        "Program",
                        "Program3",
                        ""});
#line 39
 testRunner.Then("I get the entity learning outcomes as follows:", ((string)(null)), table7, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
