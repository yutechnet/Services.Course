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
    [NUnit.Framework.DescriptionAttribute("CourseLearningActivity")]
    [NUnit.Framework.CategoryAttribute("Api")]
    public partial class CourseLearningActivityFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "CourseLearningActivity.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "CourseLearningActivity", "As a course creator\r\nI would like to specify grading attributes to learning activ" +
                    "ities\r\nSo that I can turn them into gradebook items", ProgrammingLanguage.CSharp, new string[] {
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
#line 16
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
#line 19
 testRunner.And("I have the following course segments for \'Econ 100\'", ((string)(null)), table4, "And ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Add a learning activity")]
        public virtual void AddALearningActivity()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Add a learning activity", ((string[])(null)));
#line 23
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table5.AddRow(new string[] {
                        "Name",
                        "Discussion 1"});
            table5.AddRow(new string[] {
                        "Type",
                        "Discussion"});
            table5.AddRow(new string[] {
                        "IsGradeable",
                        "True"});
            table5.AddRow(new string[] {
                        "IsExtraCredit",
                        "False"});
            table5.AddRow(new string[] {
                        "Description",
                        "Desc"});
            table5.AddRow(new string[] {
                        "Weight",
                        "100"});
            table5.AddRow(new string[] {
                        "MaxPoint",
                        "20"});
            table5.AddRow(new string[] {
                        "ObjectId",
                        "D2DF063D-E2A1-4F83-9BE0-218EC676C05F"});
#line 24
 testRunner.When("I add the following learning activity to \'Week 1\' course segment", ((string)(null)), table5, "When ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table6.AddRow(new string[] {
                        "Name",
                        "Discussion 1"});
            table6.AddRow(new string[] {
                        "Type",
                        "Discussion"});
            table6.AddRow(new string[] {
                        "Description",
                        "Desc"});
            table6.AddRow(new string[] {
                        "IsGradeable",
                        "True"});
            table6.AddRow(new string[] {
                        "IsExtraCredit",
                        "False"});
            table6.AddRow(new string[] {
                        "Weight",
                        "100"});
            table6.AddRow(new string[] {
                        "MaxPoint",
                        "20"});
            table6.AddRow(new string[] {
                        "ObjectId",
                        "D2DF063D-E2A1-4F83-9BE0-218EC676C05F"});
#line 34
 testRunner.Then("my course learning activity \'Discussion 1\' contains the following", ((string)(null)), table6, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Add a learning activity with acitve, inactive, duedate")]
        public virtual void AddALearningActivityWithAcitveInactiveDuedate()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Add a learning activity with acitve, inactive, duedate", ((string[])(null)));
#line 45
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table7.AddRow(new string[] {
                        "Name",
                        "Discussion 1"});
            table7.AddRow(new string[] {
                        "Type",
                        "Discussion"});
            table7.AddRow(new string[] {
                        "IsGradeable",
                        "True"});
            table7.AddRow(new string[] {
                        "IsExtraCredit",
                        "False"});
            table7.AddRow(new string[] {
                        "Description",
                        "Desc"});
            table7.AddRow(new string[] {
                        "Weight",
                        "100"});
            table7.AddRow(new string[] {
                        "MaxPoint",
                        "20"});
            table7.AddRow(new string[] {
                        "ObjectId",
                        "D2DF063D-E2A1-4F83-9BE0-218EC676C05F"});
            table7.AddRow(new string[] {
                        "ActiveDate",
                        "15"});
            table7.AddRow(new string[] {
                        "InactiveDate",
                        "30"});
            table7.AddRow(new string[] {
                        "DueDate",
                        "60"});
#line 46
 testRunner.When("I add the following learning activity to \'Week 1\' course segment", ((string)(null)), table7, "When ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table8.AddRow(new string[] {
                        "Name",
                        "Discussion 1"});
            table8.AddRow(new string[] {
                        "Type",
                        "Discussion"});
            table8.AddRow(new string[] {
                        "IsGradeable",
                        "True"});
            table8.AddRow(new string[] {
                        "IsExtraCredit",
                        "False"});
            table8.AddRow(new string[] {
                        "Description",
                        "Desc"});
            table8.AddRow(new string[] {
                        "Weight",
                        "100"});
            table8.AddRow(new string[] {
                        "MaxPoint",
                        "20"});
            table8.AddRow(new string[] {
                        "ObjectId",
                        "D2DF063D-E2A1-4F83-9BE0-218EC676C05F"});
            table8.AddRow(new string[] {
                        "ActiveDate",
                        "15"});
            table8.AddRow(new string[] {
                        "InactiveDate",
                        "30"});
            table8.AddRow(new string[] {
                        "DueDate",
                        "60"});
#line 59
 testRunner.Then("my course learning activity \'Discussion 1\' contains the following", ((string)(null)), table8, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Get a list of all learning activities")]
        public virtual void GetAListOfAllLearningActivities()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Get a list of all learning activities", ((string[])(null)));
#line 74
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Type",
                        "IsGradeable",
                        "IsExtraCredit",
                        "Weight",
                        "MaxPoint",
                        "ObjectId"});
            table9.AddRow(new string[] {
                        "Discussion 1",
                        "Discussion",
                        "True",
                        "true",
                        "100",
                        "20",
                        "D2DF063D-E2A1-4F83-9BE0-218EC676C05F"});
            table9.AddRow(new string[] {
                        "Assignment 1",
                        "Assignment",
                        "True",
                        "true",
                        "100",
                        "20",
                        "D2DF063D-E2A1-4F83-9BE0-218EC676C05F"});
            table9.AddRow(new string[] {
                        "Quiz 1",
                        "Quiz",
                        "True",
                        "true",
                        "100",
                        "20",
                        "D2DF063D-E2A1-4F83-9BE0-218EC676C05F"});
            table9.AddRow(new string[] {
                        "Assessment 1",
                        "Assessment",
                        "True",
                        "true",
                        "100",
                        "20",
                        "D2DF063D-E2A1-4F83-9BE0-218EC676C05F"});
#line 75
 testRunner.Given("I add the following course learning activities to \'Week 1\' course segment", ((string)(null)), table9, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table10 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name"});
            table10.AddRow(new string[] {
                        "Discussion 1"});
            table10.AddRow(new string[] {
                        "Assignment 1"});
            table10.AddRow(new string[] {
                        "Quiz 1"});
            table10.AddRow(new string[] {
                        "Assessment 1"});
#line 81
 testRunner.Then("the segment \'Week 1\' should have the following learning activities", ((string)(null)), table10, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Update a learning activity NEW")]
        public virtual void UpdateALearningActivityNEW()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Update a learning activity NEW", ((string[])(null)));
#line 88
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table11 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Type",
                        "IsGradeable",
                        "IsExtraCredit",
                        "Weight",
                        "MaxPoint",
                        "ObjectId"});
            table11.AddRow(new string[] {
                        "Discussion 1",
                        "Discussion",
                        "True",
                        "true",
                        "100",
                        "20",
                        "D2DF063D-E2A1-4F83-9BE0-218EC676C05F"});
#line 89
 testRunner.Given("I add the following course learning activities to \'Week 1\' course segment", ((string)(null)), table11, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table12 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table12.AddRow(new string[] {
                        "Name",
                        "Discussion 1"});
            table12.AddRow(new string[] {
                        "Type",
                        "Assignment"});
            table12.AddRow(new string[] {
                        "IsGradeable",
                        "false"});
            table12.AddRow(new string[] {
                        "IsExtraCredit",
                        "false"});
            table12.AddRow(new string[] {
                        "Description",
                        "Desc"});
            table12.AddRow(new string[] {
                        "Weight",
                        "100"});
            table12.AddRow(new string[] {
                        "MaxPoint",
                        "100"});
            table12.AddRow(new string[] {
                        "ObjectId",
                        "D2DF063D-E2A1-4F83-9BE0-218EC676C05F"});
            table12.AddRow(new string[] {
                        "CustomAttribute",
                        "{\"id\":\"49\",\"summary\":\"asdf\",\"type\":\"Parabola.Web.Contract.Discussion.Forum\"}"});
#line 92
 testRunner.When("I update \'Discussion 1\' learning activity with the following info", ((string)(null)), table12, "When ");
#line hidden
            TechTalk.SpecFlow.Table table13 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table13.AddRow(new string[] {
                        "Name",
                        "Discussion 1"});
            table13.AddRow(new string[] {
                        "Type",
                        "Assignment"});
            table13.AddRow(new string[] {
                        "IsGradeable",
                        "false"});
            table13.AddRow(new string[] {
                        "IsExtraCredit",
                        "false"});
            table13.AddRow(new string[] {
                        "Description",
                        "Desc"});
            table13.AddRow(new string[] {
                        "Weight",
                        "100"});
            table13.AddRow(new string[] {
                        "MaxPoint",
                        "100"});
            table13.AddRow(new string[] {
                        "ObjectId",
                        "D2DF063D-E2A1-4F83-9BE0-218EC676C05F"});
            table13.AddRow(new string[] {
                        "CustomAttribute",
                        "{\"id\":\"49\",\"summary\":\"asdf\",\"type\":\"Parabola.Web.Contract.Discussion.Forum\"}"});
#line 103
 testRunner.Then("my course learning activity \'Discussion 1\' contains the following", ((string)(null)), table13, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Delete a learning activity")]
        public virtual void DeleteALearningActivity()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Delete a learning activity", ((string[])(null)));
#line 115
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table14 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Type",
                        "IsGradeable",
                        "IsExtraCredit",
                        "Weight",
                        "MaxPoint",
                        "ObjectId"});
            table14.AddRow(new string[] {
                        "Discussion 1",
                        "Discussion",
                        "True",
                        "true",
                        "100",
                        "20",
                        "D2DF063D-E2A1-4F83-9BE0-218EC676C05F"});
#line 116
 testRunner.Given("I add the following course learning activities to \'Week 1\' course segment", ((string)(null)), table14, "Given ");
#line 119
 testRunner.When("I remove \"Discussion 1\" learning activity", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 120
 testRunner.And("I retrieve the course learning activity \'Discussion 1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 121
 testRunner.Then("I get \'NotFound\' response", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Cannot add a learning activity to a course that is already published")]
        public virtual void CannotAddALearningActivityToACourseThatIsAlreadyPublished()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Cannot add a learning activity to a course that is already published", ((string[])(null)));
#line 123
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table15 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Note"});
            table15.AddRow(new string[] {
                        "Econ 100",
                        "Blah blah"});
#line 124
 testRunner.When("I publish the following courses", ((string)(null)), table15, "When ");
#line hidden
            TechTalk.SpecFlow.Table table16 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table16.AddRow(new string[] {
                        "Name",
                        "Assignment 2"});
            table16.AddRow(new string[] {
                        "Type",
                        "Assignment"});
            table16.AddRow(new string[] {
                        "IsGradeable",
                        "True"});
            table16.AddRow(new string[] {
                        "IsExtraCredit",
                        "False"});
            table16.AddRow(new string[] {
                        "Weight",
                        "50"});
            table16.AddRow(new string[] {
                        "MaxPoint",
                        "20"});
            table16.AddRow(new string[] {
                        "ObjectId",
                        "D2DF063D-E2A1-4F83-9BE0-218EC676C05F"});
#line 127
 testRunner.And("I add the following learning activity to \'Week 1\' course segment", ((string)(null)), table16, "And ");
#line 136
 testRunner.Then("I get \'BadRequest\' response", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Add a new learning activity with a custom type")]
        public virtual void AddANewLearningActivityWithACustomType()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Add a new learning activity with a custom type", ((string[])(null)));
#line 138
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table17 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table17.AddRow(new string[] {
                        "Name",
                        "Custom 1"});
            table17.AddRow(new string[] {
                        "Type",
                        "Custom"});
            table17.AddRow(new string[] {
                        "IsGradeable",
                        "True"});
            table17.AddRow(new string[] {
                        "IsExtraCredit",
                        "False"});
            table17.AddRow(new string[] {
                        "Weight",
                        "100"});
            table17.AddRow(new string[] {
                        "MaxPoint",
                        "20"});
            table17.AddRow(new string[] {
                        "ObjectId",
                        "D2DF063D-E2A1-4F83-9BE0-218EC676C05F"});
            table17.AddRow(new string[] {
                        "CustomAttribute",
                        "{\"id\":\"49\",\"summary\":\"asdf\",\"type\":\"Parabola.Web.Contract.Discussion.Forum\"}"});
#line 139
 testRunner.When("I add the following learning activity to \'Week 1\' course segment", ((string)(null)), table17, "When ");
#line hidden
            TechTalk.SpecFlow.Table table18 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table18.AddRow(new string[] {
                        "Name",
                        "Custom 1"});
            table18.AddRow(new string[] {
                        "Type",
                        "Custom"});
            table18.AddRow(new string[] {
                        "IsGradeable",
                        "True"});
            table18.AddRow(new string[] {
                        "IsExtraCredit",
                        "False"});
            table18.AddRow(new string[] {
                        "Weight",
                        "100"});
            table18.AddRow(new string[] {
                        "MaxPoint",
                        "20"});
            table18.AddRow(new string[] {
                        "ObjectId",
                        "D2DF063D-E2A1-4F83-9BE0-218EC676C05F"});
            table18.AddRow(new string[] {
                        "CustomAttribute",
                        "{\"id\":\"49\",\"summary\":\"asdf\",\"type\":\"Parabola.Web.Contract.Discussion.Forum\"}"});
#line 149
 testRunner.Then("my course learning activity \'Custom 1\' contains the following", ((string)(null)), table18, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
