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
    [NUnit.Framework.DescriptionAttribute("CourseContent")]
    [NUnit.Framework.CategoryAttribute("Api")]
    public partial class CourseContentFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "CourseContent.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "CourseContent", "", ProgrammingLanguage.CSharp, new string[] {
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
#line 4
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name"});
            table1.AddRow(new string[] {
                        "COB"});
#line 5
 testRunner.And("the following organizations exist", ((string)(null)), table1, "And ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Capability"});
            table2.AddRow(new string[] {
                        "CourseCreate"});
            table2.AddRow(new string[] {
                        "CoursePublish"});
            table2.AddRow(new string[] {
                        "CourseView"});
#line 8
 testRunner.And("I have the following capabilities", ((string)(null)), table2, "And ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "OrganizationName",
                        "CourseType",
                        "IsTemplate"});
            table3.AddRow(new string[] {
                        "Econ 100",
                        "E100",
                        "Macroeconomics",
                        "COB",
                        "Traditional",
                        "False"});
#line 13
 testRunner.And("I have the following courses", ((string)(null)), table3, "And ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Description",
                        "Type",
                        "ParentSegment"});
            table4.AddRow(new string[] {
                        "Week 1",
                        "First week is slack time",
                        "TimeSpan",
                        ""});
#line 16
 testRunner.And("I have the following course segments for \'Econ 100\'", ((string)(null)), table4, "And ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Type",
                        "IsGradeable",
                        "IsExtraCredit",
                        "Weight",
                        "MaxPoint",
                        "ObjectId"});
            table5.AddRow(new string[] {
                        "Discussion 1",
                        "Discussion",
                        "True",
                        "true",
                        "100",
                        "20",
                        "D2DF063D-E2A1-4F83-9BE0-218EC676C05F"});
            table5.AddRow(new string[] {
                        "Assignment 1",
                        "Assignment",
                        "True",
                        "true",
                        "100",
                        "20",
                        "D2DF063D-E2A1-4F83-9BE0-218EC676C05F"});
#line 19
 testRunner.Given("I add the following course learning activities to \'Week 1\' course segment", ((string)(null)), table5, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name"});
            table6.AddRow(new string[] {
                        "file1"});
            table6.AddRow(new string[] {
                        "file2"});
#line 23
 testRunner.And("I have the following assets", ((string)(null)), table6, "And ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Add course content")]
        public virtual void AddCourseContent()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Add course content", ((string[])(null)));
#line 28
this.ScenarioSetup(scenarioInfo);
#line 4
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name"});
            table7.AddRow(new string[] {
                        "file1"});
            table7.AddRow(new string[] {
                        "file2"});
#line 29
 testRunner.When("I add the following assets as content to \'Discussion 1\' learning activity", ((string)(null)), table7, "When ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name"});
            table8.AddRow(new string[] {
                        "file1"});
            table8.AddRow(new string[] {
                        "file2"});
#line 33
 testRunner.Then("\'Discussion 1\' learning activity has the following content", ((string)(null)), table8, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
