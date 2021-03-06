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
    [NUnit.Framework.DescriptionAttribute("CoursePrerequisites")]
    [NUnit.Framework.CategoryAttribute("Api")]
    public partial class CoursePrerequisitesFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "CoursePreRequisite.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "CoursePrerequisites", "As a course creator\r\nI want to be able to declare the set of courses that need to" +
                    " be accomplished prior to a given course\r\nSo that students can be enrolled in th" +
                    "ese courses", ProgrammingLanguage.CSharp, new string[] {
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
            table2.AddRow(new string[] {
                        "EditCourse"});
#line 11
 testRunner.And("I have the following capabilities", ((string)(null)), table2, "And ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name"});
            table3.AddRow(new string[] {
                        "asset1"});
            table3.AddRow(new string[] {
                        "asset2"});
#line 17
 testRunner.And("I have the following assets", ((string)(null)), table3, "And ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "PublishNote"});
            table4.AddRow(new string[] {
                        "asset1",
                        "published"});
            table4.AddRow(new string[] {
                        "asset2",
                        "published"});
#line 21
 testRunner.And("Published the following assets", ((string)(null)), table4, "And ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "OrganizationName",
                        "CourseType",
                        "IsTemplate",
                        "MetaData",
                        "ExtensionAssets"});
            table5.AddRow(new string[] {
                        "Econ 100",
                        "E100",
                        "Macroeconomics",
                        "COB",
                        "Traditional",
                        "False",
                        "{someData}",
                        "asset1,asset2"});
            table5.AddRow(new string[] {
                        "Econ 200",
                        "E200",
                        "Microeconomics",
                        "COB",
                        "Traditional",
                        "False",
                        "{someData}",
                        "asset1,asset2"});
            table5.AddRow(new string[] {
                        "Econ 250",
                        "E100",
                        "Intro to Econometrics",
                        "COB",
                        "Traditional",
                        "False",
                        "{someData}",
                        "asset1,asset2"});
            table5.AddRow(new string[] {
                        "Econ 300",
                        "E100",
                        "Applied Econometrics",
                        "COB",
                        "Traditional",
                        "False",
                        "{someData}",
                        "asset1,asset2"});
            table5.AddRow(new string[] {
                        "Econ 350",
                        "E350",
                        "Labor Economics",
                        "COB",
                        "Traditional",
                        "False",
                        "{someData}",
                        "asset1,asset2"});
            table5.AddRow(new string[] {
                        "Econ 400",
                        "E400",
                        "Advanced Econometrics",
                        "COB",
                        "Traditional",
                        "False",
                        "{someData}",
                        "asset1,asset2"});
            table5.AddRow(new string[] {
                        "Econ 450",
                        "E100",
                        "Financial Economics",
                        "COB",
                        "Traditional",
                        "False",
                        "{someData}",
                        "asset1,asset2"});
            table5.AddRow(new string[] {
                        "Math 101",
                        "M101",
                        "Basic mathematics",
                        "COB",
                        "Traditional",
                        "False",
                        "{someData}",
                        "asset1,asset2"});
            table5.AddRow(new string[] {
                        "Math 150",
                        "M101",
                        "Geometry",
                        "COB",
                        "Traditional",
                        "False",
                        "{someData}",
                        "asset1,asset2"});
            table5.AddRow(new string[] {
                        "Math 200",
                        "M200",
                        "Calculus",
                        "COB",
                        "Traditional",
                        "False",
                        "{someData}",
                        "asset1,asset2"});
#line 25
 testRunner.And("I have the following courses", ((string)(null)), table5, "And ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Add a course prerequisite")]
        public virtual void AddACoursePrerequisite()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Add a course prerequisite", ((string[])(null)));
#line 38
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Note"});
            table6.AddRow(new string[] {
                        "Econ 100",
                        "a note"});
            table6.AddRow(new string[] {
                        "Econ 200",
                        "a note"});
#line 39
 testRunner.When("I publish the following courses", ((string)(null)), table6, "When ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name"});
            table7.AddRow(new string[] {
                        "Econ 100"});
            table7.AddRow(new string[] {
                        "Econ 200"});
#line 43
 testRunner.And("I add the following prerequisites to \'Econ 400\'", ((string)(null)), table7, "And ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name"});
            table8.AddRow(new string[] {
                        "Econ 100"});
            table8.AddRow(new string[] {
                        "Econ 200"});
#line 47
 testRunner.Then("the course \'Econ 400\' should have the following prerequisites", ((string)(null)), table8, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Remove a course from the prerequisite list")]
        public virtual void RemoveACourseFromThePrerequisiteList()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Remove a course from the prerequisite list", ((string[])(null)));
#line 52
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Note"});
            table9.AddRow(new string[] {
                        "Econ 100",
                        "a note"});
            table9.AddRow(new string[] {
                        "Econ 200",
                        "a note"});
            table9.AddRow(new string[] {
                        "Econ 300",
                        "a note"});
            table9.AddRow(new string[] {
                        "Econ 350",
                        "a note"});
#line 53
 testRunner.When("I publish the following courses", ((string)(null)), table9, "When ");
#line hidden
            TechTalk.SpecFlow.Table table10 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name"});
            table10.AddRow(new string[] {
                        "Econ 100"});
            table10.AddRow(new string[] {
                        "Econ 200"});
#line 59
 testRunner.And("I add the following prerequisites to \'Econ 450\'", ((string)(null)), table10, "And ");
#line hidden
            TechTalk.SpecFlow.Table table11 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name"});
            table11.AddRow(new string[] {
                        "Econ 300"});
            table11.AddRow(new string[] {
                        "Econ 350"});
#line 63
 testRunner.And("I add the following prerequisites to \'Econ 450\'", ((string)(null)), table11, "And ");
#line hidden
            TechTalk.SpecFlow.Table table12 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name"});
            table12.AddRow(new string[] {
                        "Econ 300"});
            table12.AddRow(new string[] {
                        "Econ 350"});
#line 67
 testRunner.Then("the course \'Econ 450\' should have the following prerequisites", ((string)(null)), table12, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Cannot add a prerequisite to a course that is published")]
        public virtual void CannotAddAPrerequisiteToACourseThatIsPublished()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Cannot add a prerequisite to a course that is published", ((string[])(null)));
#line 72
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table13 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Note"});
            table13.AddRow(new string[] {
                        "Econ 300",
                        "a note"});
#line 73
 testRunner.When("I publish the following courses", ((string)(null)), table13, "When ");
#line hidden
            TechTalk.SpecFlow.Table table14 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name"});
            table14.AddRow(new string[] {
                        "Math 101"});
#line 76
 testRunner.And("I add the following prerequisites to \'Econ 300\'", ((string)(null)), table14, "And ");
#line 79
 testRunner.Then("I get \'BadRequest\' response", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Cannot add an unpublished course ass a prerequisite")]
        public virtual void CannotAddAnUnpublishedCourseAssAPrerequisite()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Cannot add an unpublished course ass a prerequisite", ((string[])(null)));
#line 81
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table15 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name"});
            table15.AddRow(new string[] {
                        "Math 150"});
#line 82
 testRunner.When("I add the following prerequisites to \'Math 200\'", ((string)(null)), table15, "When ");
#line 85
 testRunner.Then("I get \'BadRequest\' response", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Verify prerequisites are copied from course template")]
        public virtual void VerifyPrerequisitesAreCopiedFromCourseTemplate()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Verify prerequisites are copied from course template", ((string[])(null)));
#line 87
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table16 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "OrganizationName",
                        "CourseType",
                        "IsTemplate",
                        "MetaData",
                        "ExtensionAssets"});
            table16.AddRow(new string[] {
                        "Template 1",
                        "TemplateCode1",
                        "My First Course Template",
                        "COB",
                        "Traditional",
                        "true",
                        "{someData}",
                        "asset1,asset2"});
#line 88
 testRunner.Given("I have the following course templates", ((string)(null)), table16, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table17 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Note"});
            table17.AddRow(new string[] {
                        "Econ 100",
                        "a note"});
            table17.AddRow(new string[] {
                        "Econ 200",
                        "a note"});
#line 91
 testRunner.When("I publish the following courses", ((string)(null)), table17, "When ");
#line hidden
            TechTalk.SpecFlow.Table table18 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name"});
            table18.AddRow(new string[] {
                        "Econ 100"});
            table18.AddRow(new string[] {
                        "Econ 200"});
#line 95
 testRunner.And("I add the following prerequisites to \'Template 1\'", ((string)(null)), table18, "And ");
#line hidden
            TechTalk.SpecFlow.Table table19 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "OrganizationName",
                        "IsTemplate"});
            table19.AddRow(new string[] {
                        "English 2020",
                        "CourseCode1",
                        "My First Course Template",
                        "COB",
                        "false"});
#line 99
 testRunner.And("I create a course from the template \'Template 1\' with the following", ((string)(null)), table19, "And ");
#line hidden
            TechTalk.SpecFlow.Table table20 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name"});
            table20.AddRow(new string[] {
                        "Econ 100"});
            table20.AddRow(new string[] {
                        "Econ 200"});
#line 102
 testRunner.Then("the course \'English 2020\' should have the following prerequisites", ((string)(null)), table20, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Verify prerequisites should be saved when creating a course")]
        public virtual void VerifyPrerequisitesShouldBeSavedWhenCreatingACourse()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Verify prerequisites should be saved when creating a course", ((string[])(null)));
#line 107
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table21 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Note"});
            table21.AddRow(new string[] {
                        "Econ 100",
                        "a note"});
            table21.AddRow(new string[] {
                        "Econ 200",
                        "a note"});
#line 108
 testRunner.When("I publish the following courses", ((string)(null)), table21, "When ");
#line hidden
            TechTalk.SpecFlow.Table table22 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "Tenant Id",
                        "OrganizationName",
                        "CourseType",
                        "IsTemplate",
                        "MetaData",
                        "ExtensionAssets",
                        "Prerequisites"});
            table22.AddRow(new string[] {
                        "English 101",
                        "ENG101",
                        "Ranji\'s awesome English Class",
                        "999999",
                        "COB",
                        "Traditional",
                        "false",
                        "{someData}",
                        "B40CE4F4-434A-4987-80A8-58F795C212EB",
                        "Econ 100,Econ 200"});
#line 112
 testRunner.And("I have an existing course with following info:", ((string)(null)), table22, "And ");
#line hidden
            TechTalk.SpecFlow.Table table23 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name"});
            table23.AddRow(new string[] {
                        "Econ 100"});
            table23.AddRow(new string[] {
                        "Econ 200"});
#line 115
 testRunner.Then("the course \'English 101\' should have the following prerequisites", ((string)(null)), table23, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Verify prerequisites should be saved when editng a course")]
        public virtual void VerifyPrerequisitesShouldBeSavedWhenEditngACourse()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Verify prerequisites should be saved when editng a course", ((string[])(null)));
#line 120
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table24 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Note"});
            table24.AddRow(new string[] {
                        "Econ 100",
                        "a note"});
            table24.AddRow(new string[] {
                        "Econ 200",
                        "a note"});
            table24.AddRow(new string[] {
                        "Econ 300",
                        "a note"});
            table24.AddRow(new string[] {
                        "Econ 400",
                        "a note"});
#line 121
 testRunner.When("I publish the following courses", ((string)(null)), table24, "When ");
#line hidden
            TechTalk.SpecFlow.Table table25 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "Tenant Id",
                        "OrganizationName",
                        "CourseType",
                        "IsTemplate",
                        "MetaData",
                        "ExtensionAssets",
                        "Prerequisites"});
            table25.AddRow(new string[] {
                        "English 101",
                        "ENG101",
                        "Ranji\'s awesome English Class",
                        "999999",
                        "COB",
                        "Traditional",
                        "false",
                        "{someData}",
                        "B40CE4F4-434A-4987-80A8-58F795C212EB",
                        "Econ 100,Econ 200"});
#line 127
 testRunner.And("I have an existing course with following info:", ((string)(null)), table25, "And ");
#line hidden
            TechTalk.SpecFlow.Table table26 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "CourseType",
                        "IsTemplate",
                        "Credit",
                        "MetaData",
                        "ExtensionAssets",
                        "Prerequisites"});
            table26.AddRow(new string[] {
                        "English 101",
                        "ENG101",
                        "John\'s awesome English Class",
                        "Traditional",
                        "false",
                        "10",
                        "{differentData}",
                        "B40CE4F4-434A-4987-80A8-58F795C212EB",
                        "Econ 300,Econ 400"});
#line 130
 testRunner.And("I change the info to reflect the following:", ((string)(null)), table26, "And ");
#line hidden
            TechTalk.SpecFlow.Table table27 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name"});
            table27.AddRow(new string[] {
                        "Econ 300"});
            table27.AddRow(new string[] {
                        "Econ 400"});
#line 133
 testRunner.Then("the course \'English 101\' should have the following prerequisites", ((string)(null)), table27, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
