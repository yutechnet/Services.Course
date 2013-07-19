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
    [NUnit.Framework.DescriptionAttribute("CourseTemplate")]
    [NUnit.Framework.CategoryAttribute("Api")]
    public partial class CourseTemplateFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "CourseTemplate.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "CourseTemplate", "In order to create courses easily and quickly\r\nAs a course builder\r\nI want to cre" +
                    "ate courses from a template", ProgrammingLanguage.CSharp, new string[] {
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
testRunner.Given("the following programs exist:", ((string)(null)), table1, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Description",
                        "TenantId"});
            table2.AddRow(new string[] {
                        "first program learning outcome",
                        "999999"});
            table2.AddRow(new string[] {
                        "second program learning outcome",
                        "999999"});
#line 12
testRunner.And("I associate the following learning outcomes to \'Bachelor of Art\' program:", ((string)(null)), table2, "And ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "Tenant SegmentId",
                        "OrganizationId",
                        "CourseType",
                        "IsTemplate"});
            table3.AddRow(new string[] {
                        "Template 1",
                        "TemplateCode1",
                        "My First Course Template",
                        "999999",
                        "C3885307-BDAD-480F-8E7C-51DFE5D80387",
                        "Traditional",
                        "true"});
#line 16
testRunner.And("I have the following course template", ((string)(null)), table3, "And ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Program Name"});
            table4.AddRow(new string[] {
                        "Bachelor of Art"});
            table4.AddRow(new string[] {
                        "Bachelor of Science"});
#line 19
testRunner.And("I associate \'Template 1\' course with the following programs:", ((string)(null)), table4, "And ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Description",
                        "TenantId"});
            table5.AddRow(new string[] {
                        "first course learning outcome",
                        "999999"});
            table5.AddRow(new string[] {
                        "second course learning outcome",
                        "999999"});
#line 23
testRunner.And("I assoicate the following learning outcomes to \'Template 1\' course:", ((string)(null)), table5, "And ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "CourseCode",
                        "Description"});
            table6.AddRow(new string[] {
                        "TemplateCode1",
                        "first course learning outcome"});
            table6.AddRow(new string[] {
                        "TemplateCode1",
                        "second course learning outcome"});
#line 27
testRunner.And("I assoicate the following outcomes to \'second program learning outcome\'", ((string)(null)), table6, "And ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Description",
                        "Type",
                        "ParentSegment"});
            table7.AddRow(new string[] {
                        "Week1",
                        "First week is slack time",
                        "TimeSpan",
                        ""});
            table7.AddRow(new string[] {
                        "Discussion",
                        "Discussion for the first week",
                        "Discussion",
                        "Week1"});
            table7.AddRow(new string[] {
                        "Discussion2",
                        "Discussion2 for the first week",
                        "Discussion",
                        "Week1"});
            table7.AddRow(new string[] {
                        "Topic",
                        "Topic for a discussion",
                        "Topic",
                        "Discussion"});
#line 31
testRunner.And("I add following course segments to \'Template 1\':", ((string)(null)), table7, "And ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Create a course from a template")]
        public virtual void CreateACourseFromATemplate()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Create a course from a template", ((string[])(null)));
#line 39
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "Tenant SegmentId",
                        "OrganizationId",
                        "CourseType",
                        "IsTemplate"});
            table8.AddRow(new string[] {
                        "Course 1",
                        "CourseCode1",
                        "My First Course Template",
                        "999999",
                        "b50cada2-b1ba-4b2e-b82c-8ca7125fb39b",
                        "Traditional",
                        "false"});
#line 40
testRunner.When("I create a course from the template \'Template 1\' with the following:", ((string)(null)), table8, "When ");
#line hidden
            TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table9.AddRow(new string[] {
                        "Name",
                        "Course 1"});
            table9.AddRow(new string[] {
                        "Code",
                        "CourseCode1"});
            table9.AddRow(new string[] {
                        "Description",
                        "My First Course Template"});
            table9.AddRow(new string[] {
                        "OrganizationId",
                        "b50cada2-b1ba-4b2e-b82c-8ca7125fb39b"});
            table9.AddRow(new string[] {
                        "CourseType",
                        "Traditional"});
            table9.AddRow(new string[] {
                        "IsTemplate",
                        "false"});
#line 43
testRunner.Then("the course should have the following info:", ((string)(null)), table9, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Ignore course Type in the request when creating from template")]
        public virtual void IgnoreCourseTypeInTheRequestWhenCreatingFromTemplate()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Ignore course Type in the request when creating from template", ((string[])(null)));
#line 52
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table10 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "Tenant SegmentId",
                        "OrganizationId",
                        "CourseType",
                        "IsTemplate"});
            table10.AddRow(new string[] {
                        "Course 2",
                        "CourseCode2",
                        "My First Course Template",
                        "999999",
                        "C3885307-BDAD-480F-8E7C-51DFE5D80387",
                        "Competency",
                        "false"});
#line 53
testRunner.When("I create a course from the template \'Template 1\' with the following:", ((string)(null)), table10, "When ");
#line hidden
            TechTalk.SpecFlow.Table table11 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table11.AddRow(new string[] {
                        "Name",
                        "Course 2"});
            table11.AddRow(new string[] {
                        "Code",
                        "CourseCode2"});
            table11.AddRow(new string[] {
                        "Description",
                        "My First Course Template"});
            table11.AddRow(new string[] {
                        "OrganizationId",
                        "C3885307-BDAD-480F-8E7C-51DFE5D80387"});
            table11.AddRow(new string[] {
                        "CourseType",
                        "Traditional"});
            table11.AddRow(new string[] {
                        "IsTemplate",
                        "false"});
#line 56
testRunner.Then("the course should have the following info:", ((string)(null)), table11, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Verify programs are copied from course template")]
        public virtual void VerifyProgramsAreCopiedFromCourseTemplate()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Verify programs are copied from course template", ((string[])(null)));
#line 65
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table12 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "Tenant SegmentId",
                        "OrganizationId",
                        "CourseType",
                        "IsTemplate"});
            table12.AddRow(new string[] {
                        "Course 3",
                        "CourseCode3",
                        "My First Course from Template",
                        "999999",
                        "C3885307-BDAD-480F-8E7C-51DFE5D80387",
                        "Traditional",
                        "false"});
#line 66
testRunner.When("I create a course from the template \'Template 1\' with the following:", ((string)(null)), table12, "When ");
#line hidden
            TechTalk.SpecFlow.Table table13 = new TechTalk.SpecFlow.Table(new string[] {
                        "Program Name"});
            table13.AddRow(new string[] {
                        "Bachelor of Art"});
            table13.AddRow(new string[] {
                        "Bachelor of Science"});
#line 69
testRunner.Then("the course \'Course 3\' includes the following program information:", ((string)(null)), table13, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Verify segments are copied from course template")]
        public virtual void VerifySegmentsAreCopiedFromCourseTemplate()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Verify segments are copied from course template", ((string[])(null)));
#line 74
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table14 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "Tenant SegmentId",
                        "OrganizationId",
                        "CourseType",
                        "IsTemplate"});
            table14.AddRow(new string[] {
                        "Course 4",
                        "CourseCode4",
                        "My First Course from Template",
                        "999999",
                        "C3885307-BDAD-480F-8E7C-51DFE5D80387",
                        "Traditional",
                        "false"});
#line 75
testRunner.When("I create a course from the template \'Template 1\' with the following:", ((string)(null)), table14, "When ");
#line hidden
            TechTalk.SpecFlow.Table table15 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Description",
                        "Type",
                        "ParentSegment"});
            table15.AddRow(new string[] {
                        "Week1",
                        "First week is slack time",
                        "TimeSpan",
                        ""});
            table15.AddRow(new string[] {
                        "Discussion",
                        "Discussion for the first week",
                        "Discussion",
                        "Week1"});
            table15.AddRow(new string[] {
                        "Discussion2",
                        "Discussion2 for the first week",
                        "Discussion",
                        "Week1"});
            table15.AddRow(new string[] {
                        "Topic",
                        "Topic for a discussion",
                        "Topic",
                        "Discussion"});
#line 78
testRunner.Then("the course \'Course 4\' should have these course segments:", ((string)(null)), table15, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Verify outcomes are copied from course template")]
        public virtual void VerifyOutcomesAreCopiedFromCourseTemplate()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Verify outcomes are copied from course template", ((string[])(null)));
#line 85
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table16 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "Tenant SegmentId",
                        "OrganizationId",
                        "CourseType",
                        "IsTemplate"});
            table16.AddRow(new string[] {
                        "Course 5",
                        "CourseCode5",
                        "My First Course from Template",
                        "999999",
                        "C3885307-BDAD-480F-8E7C-51DFE5D80387",
                        "Traditional",
                        "false"});
#line 86
testRunner.When("I create a course from the template \'Template 1\' with the following:", ((string)(null)), table16, "When ");
#line hidden
            TechTalk.SpecFlow.Table table17 = new TechTalk.SpecFlow.Table(new string[] {
                        "Description"});
            table17.AddRow(new string[] {
                        "first course learning outcome"});
            table17.AddRow(new string[] {
                        "second course learning outcome"});
#line 89
testRunner.Then("the course \'Template 1\' includes the following learning outcomes:", ((string)(null)), table17, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
