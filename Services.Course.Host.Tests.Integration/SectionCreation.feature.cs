﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.18444
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
    [NUnit.Framework.DescriptionAttribute("Create Section")]
    [NUnit.Framework.CategoryAttribute("Api")]
    public partial class CreateSectionFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "SectionCreation.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Create Section", "", ProgrammingLanguage.CSharp, new string[] {
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
                        "Capability"});
            table1.AddRow(new string[] {
                        "CourseCreate"});
            table1.AddRow(new string[] {
                        "CoursePublish"});
            table1.AddRow(new string[] {
                        "CourseView"});
#line 5
 testRunner.Given("I have the following capabilities", ((string)(null)), table1, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name"});
            table2.AddRow(new string[] {
                        "COB"});
#line 10
  testRunner.And("the following organizations exist", ((string)(null)), table2, "And ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "OrganizationName",
                        "Credit"});
            table3.AddRow(new string[] {
                        "Math 101",
                        "M101",
                        "Basic mathematics",
                        "COB",
                        "12"});
#line 13
 testRunner.And("I have the following courses", ((string)(null)), table3, "And ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Cannot create a section from a course that is not published")]
        public virtual void CannotCreateASectionFromACourseThatIsNotPublished()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Cannot create a section from a course that is not published", ((string[])(null)));
#line 17
this.ScenarioSetup(scenarioInfo);
#line 4
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "CourseName",
                        "Name",
                        "CourseCode",
                        "SectionCode",
                        "StartDate",
                        "EndDate"});
            table4.AddRow(new string[] {
                        "Math 101",
                        "Math 334",
                        "MATH334.ABC",
                        "MATH334.ABCSectionCode",
                        "2/15/2014",
                        "6/15/2014"});
#line 18
 testRunner.When("I create the following sections", ((string)(null)), table4, "When ");
#line 21
 testRunner.Then("I get \'BadRequest\' response", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Can create a section from a course that is published")]
        public virtual void CanCreateASectionFromACourseThatIsPublished()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Can create a section from a course that is published", ((string[])(null)));
#line 23
this.ScenarioSetup(scenarioInfo);
#line 4
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Note"});
            table5.AddRow(new string[] {
                        "Math 101",
                        "published"});
#line 24
 testRunner.When("I publish the following courses", ((string)(null)), table5, "When ");
#line 27
 testRunner.And("The section service returns \'Created\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "CourseName",
                        "Name",
                        "CourseCode",
                        "SectionCode",
                        "StartDate",
                        "EndDate"});
            table6.AddRow(new string[] {
                        "Math 101",
                        "Math 334",
                        "MATH334.ABC",
                        "MATH334.ABCSectionCode",
                        "2/15/2014",
                        "6/15/2014"});
#line 28
 testRunner.And("I create the following sections", ((string)(null)), table6, "And ");
#line 31
 testRunner.Then("I get \'Created\' response", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Create a section from a course returns status of section service")]
        public virtual void CreateASectionFromACourseReturnsStatusOfSectionService()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Create a section from a course returns status of section service", ((string[])(null)));
#line 33
this.ScenarioSetup(scenarioInfo);
#line 4
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Note"});
            table7.AddRow(new string[] {
                        "Math 101",
                        "published"});
#line 34
 testRunner.When("I publish the following courses", ((string)(null)), table7, "When ");
#line 37
 testRunner.And("The section service returns \'Forbidden\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "CourseName",
                        "Name",
                        "CourseCode",
                        "SectionCode",
                        "StartDate",
                        "EndDate"});
            table8.AddRow(new string[] {
                        "Math 101",
                        "Math 334",
                        "MATH334.ABC",
                        "MATH334.ABCSectionCode",
                        "2/15/2014",
                        "6/15/2014"});
#line 38
 testRunner.And("I create the following sections", ((string)(null)), table8, "And ");
#line 41
 testRunner.Then("I get \'Forbidden\' response", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Cannot create a section from a deactivated course")]
        public virtual void CannotCreateASectionFromADeactivatedCourse()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Cannot create a section from a deactivated course", ((string[])(null)));
#line 43
this.ScenarioSetup(scenarioInfo);
#line 4
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Note"});
            table9.AddRow(new string[] {
                        "Math 101",
                        "published"});
#line 44
 testRunner.When("I publish the following courses", ((string)(null)), table9, "When ");
#line 47
 testRunner.When("I deactivate the course \'Math 101\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table10 = new TechTalk.SpecFlow.Table(new string[] {
                        "CourseName",
                        "Name",
                        "CourseCode",
                        "SectionCode",
                        "StartDate",
                        "EndDate"});
            table10.AddRow(new string[] {
                        "Math 101",
                        "Math 334",
                        "MATH334.ABC",
                        "MATH334.ABCSectionCode",
                        "2/15/2014",
                        "6/15/2014"});
#line 48
 testRunner.And("I create the following sections", ((string)(null)), table10, "And ");
#line 51
 testRunner.Then("I get \'BadRequest\' response", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
