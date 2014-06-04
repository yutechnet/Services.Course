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
    [NUnit.Framework.DescriptionAttribute("CourseDeactivation")]
    [NUnit.Framework.CategoryAttribute("Api")]
    public partial class CourseDeactivationFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "CourseDeactivation.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "CourseDeactivation", "", ProgrammingLanguage.CSharp, new string[] {
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
                        "English 1010",
                        "ENG101",
                        "Ranji\'s awesome English Class",
                        "COB",
                        "Traditional",
                        "false"});
#line 14
 testRunner.And("I have the following courses", ((string)(null)), table3, "And ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Course is activated by default")]
        public virtual void CourseIsActivatedByDefault()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Course is activated by default", ((string[])(null)));
#line 18
this.ScenarioSetup(scenarioInfo);
#line 4
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table4.AddRow(new string[] {
                        "Name",
                        "English 1010"});
            table4.AddRow(new string[] {
                        "Code",
                        "ENG101"});
            table4.AddRow(new string[] {
                        "Description",
                        "Ranji\'s awesome English Class"});
            table4.AddRow(new string[] {
                        "VersionNumber",
                        "1.0.0.0"});
            table4.AddRow(new string[] {
                        "IsPublished",
                        "false"});
            table4.AddRow(new string[] {
                        "PublishNote",
                        ""});
            table4.AddRow(new string[] {
                        "IsActivated",
                        "true"});
#line 19
 testRunner.Then("the course \'English 1010\' should have the following info", ((string)(null)), table4, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Cannot deactivate unpublished course")]
        public virtual void CannotDeactivateUnpublishedCourse()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Cannot deactivate unpublished course", ((string[])(null)));
#line 29
this.ScenarioSetup(scenarioInfo);
#line 4
this.FeatureBackground();
#line 30
 testRunner.When("I deactivate the course \'English 1010\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 31
 testRunner.Then("I get \'BadRequest\' response", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Can deactivate published course")]
        public virtual void CanDeactivatePublishedCourse()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Can deactivate published course", ((string[])(null)));
#line 33
this.ScenarioSetup(scenarioInfo);
#line 4
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Note"});
            table5.AddRow(new string[] {
                        "English 1010",
                        "Blah blah"});
#line 34
 testRunner.When("I publish the following courses", ((string)(null)), table5, "When ");
#line 37
 testRunner.When("I deactivate the course \'English 1010\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table6.AddRow(new string[] {
                        "Name",
                        "English 1010"});
            table6.AddRow(new string[] {
                        "Code",
                        "ENG101"});
            table6.AddRow(new string[] {
                        "Description",
                        "Ranji\'s awesome English Class"});
            table6.AddRow(new string[] {
                        "VersionNumber",
                        "1.0.0.0"});
            table6.AddRow(new string[] {
                        "IsPublished",
                        "true"});
            table6.AddRow(new string[] {
                        "PublishNote",
                        "Blah blah"});
            table6.AddRow(new string[] {
                        "IsActivated",
                        "false"});
#line 38
 testRunner.Then("the course \'English 1010\' should have the following info", ((string)(null)), table6, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Can activate deactivated course")]
        public virtual void CanActivateDeactivatedCourse()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Can activate deactivated course", ((string[])(null)));
#line 48
this.ScenarioSetup(scenarioInfo);
#line 4
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Note"});
            table7.AddRow(new string[] {
                        "English 1010",
                        "Blah blah"});
#line 49
 testRunner.When("I publish the following courses", ((string)(null)), table7, "When ");
#line 52
 testRunner.When("I deactivate the course \'English 1010\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 53
 testRunner.When("I activate the course \'English 1010\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table8.AddRow(new string[] {
                        "Name",
                        "English 1010"});
            table8.AddRow(new string[] {
                        "Code",
                        "ENG101"});
            table8.AddRow(new string[] {
                        "Description",
                        "Ranji\'s awesome English Class"});
            table8.AddRow(new string[] {
                        "VersionNumber",
                        "1.0.0.0"});
            table8.AddRow(new string[] {
                        "IsPublished",
                        "true"});
            table8.AddRow(new string[] {
                        "PublishNote",
                        "Blah blah"});
            table8.AddRow(new string[] {
                        "IsActivated",
                        "true"});
#line 54
 testRunner.Then("the course \'English 1010\' should have the following info", ((string)(null)), table8, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
