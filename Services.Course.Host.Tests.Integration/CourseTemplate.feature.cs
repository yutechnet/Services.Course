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
                        "Name"});
            table1.AddRow(new string[] {
                        "COB"});
#line 8
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
#line 11
 testRunner.And("I have the following capabilities", ((string)(null)), table2, "And ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Description",
                        "ProgramType",
                        "OrganizationName"});
            table3.AddRow(new string[] {
                        "Bachelor of Art",
                        "BA Program",
                        "BA",
                        "COB"});
            table3.AddRow(new string[] {
                        "Bachelor of Science",
                        "BS program",
                        "BS",
                        "COB"});
#line 16
 testRunner.And("I have the following programs", ((string)(null)), table3, "And ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "OrganizationName",
                        "CourseType",
                        "IsTemplate"});
            table4.AddRow(new string[] {
                        "Template 1",
                        "TemplateCode1",
                        "My First Course Template",
                        "COB",
                        "Traditional",
                        "true"});
#line 20
 testRunner.And("I have the following course templates", ((string)(null)), table4, "And ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Description",
                        "Type",
                        "ParentSegment"});
            table5.AddRow(new string[] {
                        "Week1",
                        "First week is slack time",
                        "TimeSpan",
                        ""});
            table5.AddRow(new string[] {
                        "Discussion",
                        "Discussion for the first week",
                        "Discussion",
                        "Week1"});
            table5.AddRow(new string[] {
                        "Discussion2",
                        "Discussion2 for the first week",
                        "Discussion",
                        "Week1"});
            table5.AddRow(new string[] {
                        "Topic",
                        "Topic for a discussion",
                        "Topic",
                        "Discussion"});
#line 23
 testRunner.And("I have the following course segments for \'Template 1\'", ((string)(null)), table5, "And ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Create a course from a template")]
        public virtual void CreateACourseFromATemplate()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Create a course from a template", ((string[])(null)));
#line 31
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "OrganizationName",
                        "IsTemplate"});
            table6.AddRow(new string[] {
                        "Course 1",
                        "CourseCode1",
                        "My First Course",
                        "COB",
                        "false"});
#line 32
testRunner.When("I create a course from the template \'Template 1\' with the following", ((string)(null)), table6, "When ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "OrganizationName",
                        "IsTemplate"});
            table7.AddRow(new string[] {
                        "Course 2",
                        "",
                        "My Second Course",
                        "COB",
                        "true"});
#line 35
testRunner.And("I create a course from the template \'Template 1\' with the following", ((string)(null)), table7, "And ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "OrganizationName",
                        "IsTemplate"});
            table8.AddRow(new string[] {
                        "Course 3",
                        "",
                        "",
                        "COB",
                        "true"});
#line 38
testRunner.And("I create a course from the template \'Template 1\' with the following", ((string)(null)), table8, "And ");
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
                        "My First Course"});
            table9.AddRow(new string[] {
                        "CourseType",
                        "Traditional"});
            table9.AddRow(new string[] {
                        "IsTemplate",
                        "false"});
#line 41
testRunner.Then("the course \'Course 1\' should have the following info", ((string)(null)), table9, "Then ");
#line hidden
            TechTalk.SpecFlow.Table table10 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table10.AddRow(new string[] {
                        "Name",
                        "Course 2"});
            table10.AddRow(new string[] {
                        "Code",
                        "TemplateCode1"});
            table10.AddRow(new string[] {
                        "Description",
                        "My Second Course"});
            table10.AddRow(new string[] {
                        "CourseType",
                        "Traditional"});
            table10.AddRow(new string[] {
                        "IsTemplate",
                        "true"});
#line 48
testRunner.And("the course \'Course 2\' should have the following info", ((string)(null)), table10, "And ");
#line hidden
            TechTalk.SpecFlow.Table table11 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table11.AddRow(new string[] {
                        "Name",
                        "Course 3"});
            table11.AddRow(new string[] {
                        "Code",
                        "TemplateCode1"});
            table11.AddRow(new string[] {
                        "Description",
                        "My First Course Template"});
            table11.AddRow(new string[] {
                        "CourseType",
                        "Traditional"});
            table11.AddRow(new string[] {
                        "IsTemplate",
                        "true"});
#line 55
testRunner.And("the course \'Course 3\' should have the following info", ((string)(null)), table11, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Ignore course Type in the request when creating from template")]
        public virtual void IgnoreCourseTypeInTheRequestWhenCreatingFromTemplate()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Ignore course Type in the request when creating from template", ((string[])(null)));
#line 63
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table12 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "OrganizationName",
                        "IsTemplate"});
            table12.AddRow(new string[] {
                        "Course 2",
                        "CourseCode2",
                        "My First Course Template",
                        "COB",
                        "false"});
#line 64
testRunner.When("I create a course from the template \'Template 1\' with the following", ((string)(null)), table12, "When ");
#line hidden
            TechTalk.SpecFlow.Table table13 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table13.AddRow(new string[] {
                        "Name",
                        "Course 2"});
            table13.AddRow(new string[] {
                        "Code",
                        "CourseCode2"});
            table13.AddRow(new string[] {
                        "Description",
                        "My First Course Template"});
            table13.AddRow(new string[] {
                        "CourseType",
                        "Traditional"});
            table13.AddRow(new string[] {
                        "IsTemplate",
                        "false"});
#line 67
testRunner.Then("the course \'Course 2\' should have the following info", ((string)(null)), table13, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Verify programs are copied from course template")]
        public virtual void VerifyProgramsAreCopiedFromCourseTemplate()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Verify programs are copied from course template", ((string[])(null)));
#line 75
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table14 = new TechTalk.SpecFlow.Table(new string[] {
                        "Program Name"});
            table14.AddRow(new string[] {
                        "Bachelor of Art"});
            table14.AddRow(new string[] {
                        "Bachelor of Science"});
#line 76
testRunner.When("I associate \'Template 1\' course with the following programs", ((string)(null)), table14, "When ");
#line hidden
            TechTalk.SpecFlow.Table table15 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "OrganizationName",
                        "IsTemplate"});
            table15.AddRow(new string[] {
                        "Course 3",
                        "CourseCode3",
                        "My First Course from Template",
                        "COB",
                        "false"});
#line 80
testRunner.And("I create a course from the template \'Template 1\' with the following", ((string)(null)), table15, "And ");
#line hidden
            TechTalk.SpecFlow.Table table16 = new TechTalk.SpecFlow.Table(new string[] {
                        "Program Name"});
            table16.AddRow(new string[] {
                        "Bachelor of Art"});
            table16.AddRow(new string[] {
                        "Bachelor of Science"});
#line 83
testRunner.Then("the course \'Course 3\' includes the following programs", ((string)(null)), table16, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Verify segments are copied from course template")]
        public virtual void VerifySegmentsAreCopiedFromCourseTemplate()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Verify segments are copied from course template", ((string[])(null)));
#line 88
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table17 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "OrganizationName",
                        "IsTemplate"});
            table17.AddRow(new string[] {
                        "Course 4",
                        "CourseCode4",
                        "My First Course from Template",
                        "COB",
                        "false"});
#line 89
testRunner.When("I create a course from the template \'Template 1\' with the following", ((string)(null)), table17, "When ");
#line hidden
            TechTalk.SpecFlow.Table table18 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Description",
                        "Type",
                        "ParentSegment"});
            table18.AddRow(new string[] {
                        "Week1",
                        "First week is slack time",
                        "TimeSpan",
                        ""});
            table18.AddRow(new string[] {
                        "Discussion",
                        "Discussion for the first week",
                        "Discussion",
                        "Week1"});
            table18.AddRow(new string[] {
                        "Discussion2",
                        "Discussion2 for the first week",
                        "Discussion",
                        "Week1"});
            table18.AddRow(new string[] {
                        "Topic",
                        "Topic for a discussion",
                        "Topic",
                        "Discussion"});
#line 92
testRunner.Then("the course \'Course 4\' should have these course segments", ((string)(null)), table18, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Verify outcomes are copied from course template")]
        public virtual void VerifyOutcomesAreCopiedFromCourseTemplate()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Verify outcomes are copied from course template", ((string[])(null)));
#line 99
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table19 = new TechTalk.SpecFlow.Table(new string[] {
                        "Description"});
            table19.AddRow(new string[] {
                        "first course learning outcome"});
            table19.AddRow(new string[] {
                        "second course learning outcome"});
#line 100
testRunner.When("I associate the newly created learning outcomes to \'Template 1\' course", ((string)(null)), table19, "When ");
#line hidden
            TechTalk.SpecFlow.Table table20 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "OrganizationName",
                        "IsTemplate"});
            table20.AddRow(new string[] {
                        "Course 5",
                        "CourseCode5",
                        "My First Course from Template",
                        "COB",
                        "false"});
#line 104
testRunner.And("I create a course from the template \'Template 1\' with the following", ((string)(null)), table20, "And ");
#line hidden
            TechTalk.SpecFlow.Table table21 = new TechTalk.SpecFlow.Table(new string[] {
                        "Description"});
            table21.AddRow(new string[] {
                        "first course learning outcome"});
            table21.AddRow(new string[] {
                        "second course learning outcome"});
#line 107
testRunner.Then("the course \'Template 1\' should have the following learning outcomes", ((string)(null)), table21, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Publish a course version, created from a template")]
        public virtual void PublishACourseVersionCreatedFromATemplate()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Publish a course version, created from a template", ((string[])(null)));
#line 112
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table22 = new TechTalk.SpecFlow.Table(new string[] {
                        "Program Name"});
            table22.AddRow(new string[] {
                        "Bachelor of Art"});
            table22.AddRow(new string[] {
                        "Bachelor of Science"});
#line 113
 testRunner.Given("I associate \'Template 1\' course with the following programs", ((string)(null)), table22, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table23 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "OrganizationName",
                        "IsTemplate"});
            table23.AddRow(new string[] {
                        "English 1010",
                        "ENG101",
                        "Ranji\'s awesome English Class",
                        "COB",
                        "false"});
#line 117
 testRunner.And("I create a course from the template \'Template 1\' with the following", ((string)(null)), table23, "And ");
#line hidden
            TechTalk.SpecFlow.Table table24 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Note"});
            table24.AddRow(new string[] {
                        "English 1010",
                        "Blah blah"});
#line 120
 testRunner.When("I publish the following courses", ((string)(null)), table24, "When ");
#line hidden
            TechTalk.SpecFlow.Table table25 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table25.AddRow(new string[] {
                        "Name",
                        "English 1010"});
            table25.AddRow(new string[] {
                        "Code",
                        "ENG101"});
            table25.AddRow(new string[] {
                        "Description",
                        "Ranji\'s awesome English Class"});
            table25.AddRow(new string[] {
                        "CourseType",
                        "Traditional"});
            table25.AddRow(new string[] {
                        "VersionNumber",
                        "1.0.0.0"});
            table25.AddRow(new string[] {
                        "IsPublished",
                        "true"});
            table25.AddRow(new string[] {
                        "PublishNote",
                        "Blah blah"});
#line 123
 testRunner.Then("the course \'English 1010\' should have the following info", ((string)(null)), table25, "Then ");
#line hidden
            TechTalk.SpecFlow.Table table26 = new TechTalk.SpecFlow.Table(new string[] {
                        "Program Name"});
            table26.AddRow(new string[] {
                        "Bachelor of Art"});
            table26.AddRow(new string[] {
                        "Bachelor of Science"});
#line 132
 testRunner.And("the course \'English 1010\' includes the following programs", ((string)(null)), table26, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Version a course, which was created from a template")]
        public virtual void VersionACourseWhichWasCreatedFromATemplate()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Version a course, which was created from a template", ((string[])(null)));
#line 137
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table27 = new TechTalk.SpecFlow.Table(new string[] {
                        "Program Name"});
            table27.AddRow(new string[] {
                        "Bachelor of Art"});
            table27.AddRow(new string[] {
                        "Bachelor of Science"});
#line 138
 testRunner.Given("I associate \'Template 1\' course with the following programs", ((string)(null)), table27, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table28 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "OrganizationName",
                        "IsTemplate"});
            table28.AddRow(new string[] {
                        "English 1010",
                        "ENG101",
                        "Ranji\'s awesome English Class",
                        "COB",
                        "false"});
#line 142
 testRunner.And("I create a course from the template \'Template 1\' with the following", ((string)(null)), table28, "And ");
#line hidden
            TechTalk.SpecFlow.Table table29 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Note"});
            table29.AddRow(new string[] {
                        "English 1010",
                        "Blah blah"});
#line 145
 testRunner.When("I publish the following courses", ((string)(null)), table29, "When ");
#line hidden
            TechTalk.SpecFlow.Table table30 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table30.AddRow(new string[] {
                        "VersionNumber",
                        "2.0a"});
#line 148
 testRunner.And("I create a new version of \'English 1010\' course named \'English 1010 v2\' with the " +
                    "following info", ((string)(null)), table30, "And ");
#line hidden
            TechTalk.SpecFlow.Table table31 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table31.AddRow(new string[] {
                        "Name",
                        "English 1010"});
            table31.AddRow(new string[] {
                        "Code",
                        "ENG101"});
            table31.AddRow(new string[] {
                        "Description",
                        "Ranji\'s awesome English Class"});
            table31.AddRow(new string[] {
                        "CourseType",
                        "Traditional"});
            table31.AddRow(new string[] {
                        "IsPublished",
                        "false"});
            table31.AddRow(new string[] {
                        "VersionNumber",
                        "2.0a"});
#line 151
 testRunner.Then("the course \'English 1010 v2\' should have the following info", ((string)(null)), table31, "Then ");
#line hidden
            TechTalk.SpecFlow.Table table32 = new TechTalk.SpecFlow.Table(new string[] {
                        "Program Name"});
            table32.AddRow(new string[] {
                        "Bachelor of Art"});
            table32.AddRow(new string[] {
                        "Bachelor of Science"});
#line 159
 testRunner.And("the course \'English 1010 v2\' includes the following programs", ((string)(null)), table32, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Cannot get the template after create a course version from a previously-published" +
            " version（DE395）")]
        public virtual void CannotGetTheTemplateAfterCreateACourseVersionFromAPreviously_PublishedVersionDE395()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Cannot get the template after create a course version from a previously-published" +
                    " version（DE395）", ((string[])(null)));
#line 164
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table33 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "OrganizationName",
                        "IsTemplate"});
            table33.AddRow(new string[] {
                        "English 2020",
                        "CourseCode1",
                        "My First Course Template",
                        "COB",
                        "false"});
#line 165
 testRunner.When("I create a course from the template \'Template 1\' with the following", ((string)(null)), table33, "When ");
#line hidden
            TechTalk.SpecFlow.Table table34 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Note"});
            table34.AddRow(new string[] {
                        "English 2020",
                        "Blah blah"});
#line 168
 testRunner.And("I publish the following courses", ((string)(null)), table34, "And ");
#line hidden
            TechTalk.SpecFlow.Table table35 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table35.AddRow(new string[] {
                        "VersionNumber",
                        "1.0.0.1"});
#line 171
 testRunner.And("I create a new version of \'English 2020\' course named \'English 2020 v1.0.0.1\' wit" +
                    "h the following info", ((string)(null)), table35, "And ");
#line 174
 testRunner.Then("The course \'English 2020 v1.0.0.1\' should have the template named \'Template 1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
