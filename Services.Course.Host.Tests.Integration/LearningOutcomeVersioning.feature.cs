﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.17929
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
    [NUnit.Framework.DescriptionAttribute("LearningOutcomeVersioning")]
    [NUnit.Framework.CategoryAttribute("Api")]
    public partial class LearningOutcomeVersioningFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "LearningOutcomeVersioning.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "LearningOutcomeVersioning", "In order to allow continuous enhancement of learning outcome\r\nAs a course builder" +
                    "\r\nI want to version the learning outcome", ProgrammingLanguage.CSharp, new string[] {
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
                        "Field",
                        "Value"});
            table1.AddRow(new string[] {
                        "Description",
                        "first learning outcome"});
            table1.AddRow(new string[] {
                        "TenantId",
                        "1"});
#line 8
 testRunner.Given("I create the following learning outcome", ((string)(null)), table1, "Given ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Create a default version")]
        public virtual void CreateADefaultVersion()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Create a default version", ((string[])(null)));
#line 13
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 14
 testRunner.When("I retrieve \'first learning outcome\' learning outcome", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table2.AddRow(new string[] {
                        "Description",
                        "first learning outcome"});
            table2.AddRow(new string[] {
                        "VersionNumber",
                        "1.0.0.0"});
#line 15
 testRunner.Then("the learning outcome should have the following info", ((string)(null)), table2, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Edit a learning outcome version")]
        public virtual void EditALearningOutcomeVersion()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Edit a learning outcome version", ((string[])(null)));
#line 20
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table3.AddRow(new string[] {
                        "Description",
                        "second learning outcome"});
            table3.AddRow(new string[] {
                        "TenantId",
                        "1"});
#line 21
 testRunner.When("I update \'first learning outcome\' learning outcome with the following info", ((string)(null)), table3, "When ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table4.AddRow(new string[] {
                        "Description",
                        "second learning outcome"});
            table4.AddRow(new string[] {
                        "VersionNumber",
                        "1.0.0.0"});
#line 25
 testRunner.Then("the learning outcome \'second learning outcome\' should have the following info", ((string)(null)), table4, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Publish a learning outcome version")]
        public virtual void PublishALearningOutcomeVersion()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Publish a learning outcome version", ((string[])(null)));
#line 30
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table5.AddRow(new string[] {
                        "PublishNote",
                        "Blah blah"});
#line 31
 testRunner.When("I publish \'first learning outcome\' learning outcome with the following info", ((string)(null)), table5, "When ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table6.AddRow(new string[] {
                        "Description",
                        "first learning outcome"});
            table6.AddRow(new string[] {
                        "VersionNumber",
                        "1.0.0.0"});
            table6.AddRow(new string[] {
                        "IsPublished",
                        "true"});
            table6.AddRow(new string[] {
                        "PublishNote",
                        "Blah blah"});
#line 34
 testRunner.Then("the learning outcome \'first learning outcome\' should have the following info", ((string)(null)), table6, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Published version cannot be modified")]
        public virtual void PublishedVersionCannotBeModified()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Published version cannot be modified", ((string[])(null)));
#line 41
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table7.AddRow(new string[] {
                        "PublishNote",
                        "Blah blah"});
#line 42
 testRunner.Given("I publish \'first learning outcome\' learning outcome with the following info", ((string)(null)), table7, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table8.AddRow(new string[] {
                        "Description",
                        "third learning outcome"});
            table8.AddRow(new string[] {
                        "TenantId",
                        "1"});
#line 45
 testRunner.When("I update \'first learning outcome\' learning outcome with the following info", ((string)(null)), table8, "When ");
#line 49
 testRunner.Then("I get \'Forbidden\' response", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Published version cannot be deleted")]
        public virtual void PublishedVersionCannotBeDeleted()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Published version cannot be deleted", ((string[])(null)));
#line 51
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table9.AddRow(new string[] {
                        "PublishNote",
                        "Blah blah"});
#line 52
 testRunner.Given("I publish \'first learning outcome\' learning outcome with the following info", ((string)(null)), table9, "Given ");
#line 55
 testRunner.When("I delete \'first learning outcome\' learning outcome", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 56
 testRunner.Then("I get \'Forbidden\' response", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Create a learning outcome version from a previously-published version")]
        public virtual void CreateALearningOutcomeVersionFromAPreviously_PublishedVersion()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Create a learning outcome version from a previously-published version", ((string[])(null)));
#line 58
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table10 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table10.AddRow(new string[] {
                        "PublishNote",
                        "Blah blah"});
#line 59
 testRunner.Given("I publish \'first learning outcome\' learning outcome with the following info", ((string)(null)), table10, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table11 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table11.AddRow(new string[] {
                        "VersionNumber",
                        "2.0a"});
#line 62
 testRunner.When("I create a new version of \'first learning outcome\' with the following info", ((string)(null)), table11, "When ");
#line hidden
            TechTalk.SpecFlow.Table table12 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table12.AddRow(new string[] {
                        "Description",
                        "first learning outcome"});
            table12.AddRow(new string[] {
                        "VersionNumber",
                        "2.0a"});
            table12.AddRow(new string[] {
                        "IsPublished",
                        "false"});
#line 65
 testRunner.Then("the learning outcome \'first learning outcome\' should have the following info", ((string)(null)), table12, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Cannot publish the same version twice")]
        public virtual void CannotPublishTheSameVersionTwice()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Cannot publish the same version twice", ((string[])(null)));
#line 71
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table13 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table13.AddRow(new string[] {
                        "PublishNote",
                        "Blah blah"});
#line 72
 testRunner.Given("I publish \'first learning outcome\' learning outcome with the following info", ((string)(null)), table13, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table14 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table14.AddRow(new string[] {
                        "VersionNumber",
                        "1.0.0.0"});
#line 75
 testRunner.When("I create a new version of \'first learning outcome\' with the following info", ((string)(null)), table14, "When ");
#line 78
 testRunner.Then("I get \'Conflict\' response", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Cannot create a version off non-existing version")]
        public virtual void CannotCreateAVersionOffNon_ExistingVersion()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Cannot create a version off non-existing version", ((string[])(null)));
#line 80
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table15 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table15.AddRow(new string[] {
                        "VersionNumber",
                        "1.0.0.0"});
#line 81
 testRunner.When("I create a new version of \'RandomOutcome\' with the following info", ((string)(null)), table15, "When ");
#line 84
 testRunner.Then("I get \'NotFound\' response", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Cannot publish without a version")]
        public virtual void CannotPublishWithoutAVersion()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Cannot publish without a version", ((string[])(null)));
#line 86
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 87
 testRunner.When("I create a learning outcome without a version", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 88
 testRunner.Then("I get \'BadRequest\' response", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
