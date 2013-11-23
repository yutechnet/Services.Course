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
    [NUnit.Framework.DescriptionAttribute("Rubric Association")]
    [NUnit.Framework.IgnoreAttribute()]
    [NUnit.Framework.CategoryAttribute("Api")]
    public partial class RubricAssociationFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "RubricAssociation.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Rubric Association", "In order to assess student\'s work\r\nAs a Learning Activity Creator\r\nI want to decl" +
                    "are rubric associations to learning activities", ProgrammingLanguage.CSharp, new string[] {
                        "Api",
                        "Ignore"});
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
#line 8
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name"});
            table1.AddRow(new string[] {
                        "COB"});
#line 9
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
#line 12
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
            table3.AddRow(new string[] {
                        "Econ 200",
                        "E200",
                        "Macroeconomics",
                        "COB",
                        "Traditional",
                        "False"});
#line 17
 testRunner.And("I have the following courses", ((string)(null)), table3, "And ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Note"});
            table4.AddRow(new string[] {
                        "Econ 200",
                        "a note"});
#line 21
 testRunner.When("I publish the following courses", ((string)(null)), table4, "When ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Description",
                        "Type",
                        "ParentSegment"});
            table5.AddRow(new string[] {
                        "Week 1",
                        "First week is slack time",
                        "TimeSpan",
                        ""});
#line 24
 testRunner.Given("I have the following course segments for \'Econ 100\'", ((string)(null)), table5, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Type",
                        "IsGradeable",
                        "IsExtraCredit",
                        "Weight",
                        "MaxPoint",
                        "ObjectId"});
            table6.AddRow(new string[] {
                        "Discussion 1",
                        "Discussion",
                        "True",
                        "true",
                        "100",
                        "20",
                        "D2DF063D-E2A1-4F83-9BE0-218EC676C05F"});
            table6.AddRow(new string[] {
                        "Assignment 1",
                        "Assignment",
                        "True",
                        "true",
                        "100",
                        "20",
                        "D2DF063D-E2A1-4F83-9BE0-218EC676C05F"});
            table6.AddRow(new string[] {
                        "Custom 1",
                        "Custom",
                        "True",
                        "true",
                        "100",
                        "20",
                        "D2DF063D-E2A1-4F83-9BE0-218EC676C05F"});
            table6.AddRow(new string[] {
                        "Custom 2",
                        "Custom",
                        "False",
                        "true",
                        "100",
                        "20",
                        "D2DF063D-E2A1-4F83-9BE0-218EC676C05F"});
            table6.AddRow(new string[] {
                        "Custom 3",
                        "Custom",
                        "True",
                        "true",
                        "100",
                        "20",
                        "D2DF063D-E2A1-4F83-9BE0-218EC676C05F"});
#line 27
 testRunner.Given("I add the following course learning activities to \'Week 1\' course segment", ((string)(null)), table6, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "Title",
                        "Description",
                        "OrganizationId",
                        "PerformanceLevel",
                        "ScoringModel",
                        "MinPoint",
                        "MaxPoint",
                        "IsPublished"});
            table7.AddRow(new string[] {
                        "Math101 Rubric",
                        "Basic math assessment tool",
                        "D2DF063D-E2A1-4F83-9BE0-218EC676C05F",
                        "Low, Med, High",
                        "Unset",
                        "50",
                        "100",
                        "True"});
            table7.AddRow(new string[] {
                        "Econ101 Rubric",
                        "Basic econ assessment tool",
                        "D2DF063D-E2A1-4F83-9BE0-218EC676C05F",
                        "Low, Med, High",
                        "Unset",
                        "50",
                        "100",
                        "True"});
            table7.AddRow(new string[] {
                        "Eng101 Rubric",
                        "Basic english assessment tool",
                        "D2DF063D-E2A1-4F83-9BE0-218EC676C05F",
                        "Low, Med, High",
                        "Unset",
                        "50",
                        "100",
                        "False"});
#line 34
 testRunner.When("I have the following rubrics", ((string)(null)), table7, "When ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Associate multiple rubric id\'s to learning activity")]
        public virtual void AssociateMultipleRubricIdSToLearningActivity()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Associate multiple rubric id\'s to learning activity", ((string[])(null)));
#line 40
this.ScenarioSetup(scenarioInfo);
#line 8
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "Title"});
            table8.AddRow(new string[] {
                        "Math101 Rubric"});
            table8.AddRow(new string[] {
                        "Econ101 Rubric"});
#line 41
 testRunner.When("I associate the following rubrics to \'Custom 1\' learning activity", ((string)(null)), table8, "When ");
#line hidden
            TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                        "Title",
                        "Description",
                        "OrganizationId",
                        "PerformanceLevel",
                        "ScoringModel",
                        "MinPoint",
                        "MaxPoint",
                        "IsPublished"});
            table9.AddRow(new string[] {
                        "Math101 Rubric",
                        "Basic math assessment tool",
                        "D2DF063D-E2A1-4F83-9BE0-218EC676C05F",
                        "Low, Med, High",
                        "Unset",
                        "50",
                        "100",
                        "True"});
            table9.AddRow(new string[] {
                        "Econ101 Rubric",
                        "Basic econ assessment tool",
                        "D2DF063D-E2A1-4F83-9BE0-218EC676C05F",
                        "Low, Med, High",
                        "Unset",
                        "50",
                        "100",
                        "True"});
#line 45
 testRunner.Then("the learning activity \'Custom 1\' should have the following rubrics", ((string)(null)), table9, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Verify unpublished rubrics cannot be associated to learning activities")]
        public virtual void VerifyUnpublishedRubricsCannotBeAssociatedToLearningActivities()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Verify unpublished rubrics cannot be associated to learning activities", ((string[])(null)));
#line 51
this.ScenarioSetup(scenarioInfo);
#line 8
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table10 = new TechTalk.SpecFlow.Table(new string[] {
                        "Title"});
            table10.AddRow(new string[] {
                        "Eng101 Rubric"});
#line 52
 testRunner.When("I associate the following rubrics to \'Custom 1\' learning activity", ((string)(null)), table10, "When ");
#line 55
 testRunner.Then("I get \'BadRequest\' response", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Can only add rubrics to learning activities of type Custom")]
        public virtual void CanOnlyAddRubricsToLearningActivitiesOfTypeCustom()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Can only add rubrics to learning activities of type Custom", ((string[])(null)));
#line 57
this.ScenarioSetup(scenarioInfo);
#line 8
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table11 = new TechTalk.SpecFlow.Table(new string[] {
                        "Title"});
            table11.AddRow(new string[] {
                        "Math101 Rubric"});
#line 58
 testRunner.When("I associate the following rubrics to \'Assignment 1\' learning activity", ((string)(null)), table11, "When ");
#line 61
 testRunner.Then("I get \'BadRequest\' response", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Cannot add one rubric more than once to the same learning activity")]
        public virtual void CannotAddOneRubricMoreThanOnceToTheSameLearningActivity()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Cannot add one rubric more than once to the same learning activity", ((string[])(null)));
#line 63
this.ScenarioSetup(scenarioInfo);
#line 8
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table12 = new TechTalk.SpecFlow.Table(new string[] {
                        "Title"});
            table12.AddRow(new string[] {
                        "Math101 Rubric"});
            table12.AddRow(new string[] {
                        "Math101 Rubric"});
#line 64
 testRunner.When("I associate the following rubrics to \'Custom 1\' learning activity", ((string)(null)), table12, "When ");
#line 68
 testRunner.Then("I get \'BadRequest\' response", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Can only add rubrics to learning activities that are gradable")]
        public virtual void CanOnlyAddRubricsToLearningActivitiesThatAreGradable()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Can only add rubrics to learning activities that are gradable", ((string[])(null)));
#line 70
this.ScenarioSetup(scenarioInfo);
#line 8
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table13 = new TechTalk.SpecFlow.Table(new string[] {
                        "Title"});
            table13.AddRow(new string[] {
                        "Math101 Rubric"});
#line 71
 testRunner.When("I associate the following rubrics to \'Custom 2\' learning activity", ((string)(null)), table13, "When ");
#line 74
 testRunner.Then("I get \'BadRequest\' response", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Cannot modify learning activity to update type or gradability if rubric is alread" +
            "y assigned")]
        public virtual void CannotModifyLearningActivityToUpdateTypeOrGradabilityIfRubricIsAlreadyAssigned()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Cannot modify learning activity to update type or gradability if rubric is alread" +
                    "y assigned", ((string[])(null)));
#line 76
this.ScenarioSetup(scenarioInfo);
#line 8
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table14 = new TechTalk.SpecFlow.Table(new string[] {
                        "Title"});
            table14.AddRow(new string[] {
                        "Math101 Rubric"});
#line 77
 testRunner.When("I associate the following rubrics to \'Custom 1\' learning activity", ((string)(null)), table14, "When ");
#line hidden
            TechTalk.SpecFlow.Table table15 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table15.AddRow(new string[] {
                        "Name",
                        "Custom 1"});
            table15.AddRow(new string[] {
                        "Type",
                        "Custom"});
            table15.AddRow(new string[] {
                        "IsGradeable",
                        "false"});
            table15.AddRow(new string[] {
                        "IsExtraCredit",
                        "false"});
            table15.AddRow(new string[] {
                        "Weight",
                        "100"});
            table15.AddRow(new string[] {
                        "MaxPoint",
                        "100"});
            table15.AddRow(new string[] {
                        "ObjectId",
                        "D2DF063D-E2A1-4F83-9BE0-218EC676C05F"});
#line 80
 testRunner.And("I update \'Custom 1\' learning activity with the following info", ((string)(null)), table15, "And ");
#line 89
 testRunner.Then("I get \'BadRequest\' response", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table16 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table16.AddRow(new string[] {
                        "Name",
                        "Custom 1"});
            table16.AddRow(new string[] {
                        "Type",
                        "Assignment"});
            table16.AddRow(new string[] {
                        "IsGradeable",
                        "true"});
            table16.AddRow(new string[] {
                        "IsExtraCredit",
                        "false"});
            table16.AddRow(new string[] {
                        "Weight",
                        "100"});
            table16.AddRow(new string[] {
                        "MaxPoint",
                        "100"});
            table16.AddRow(new string[] {
                        "ObjectId",
                        "D2DF063D-E2A1-4F83-9BE0-218EC676C05F"});
#line 90
 testRunner.And("I update \'Custom 3\' learning activity with the following info", ((string)(null)), table16, "And ");
#line 99
 testRunner.Then("I get \'BadRequest\' response", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Course must be unpublished for any rubric association/disassociation (to learning" +
            " activities)")]
        public virtual void CourseMustBeUnpublishedForAnyRubricAssociationDisassociationToLearningActivities()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Course must be unpublished for any rubric association/disassociation (to learning" +
                    " activities)", ((string[])(null)));
#line 101
this.ScenarioSetup(scenarioInfo);
#line 8
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table17 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Note"});
            table17.AddRow(new string[] {
                        "Econ 100",
                        "a note"});
#line 102
 testRunner.When("I publish the following courses", ((string)(null)), table17, "When ");
#line hidden
            TechTalk.SpecFlow.Table table18 = new TechTalk.SpecFlow.Table(new string[] {
                        "Title"});
            table18.AddRow(new string[] {
                        "Math101 Rubric"});
#line 105
 testRunner.And("I associate the following rubrics to \'Custom 1\' learning activity", ((string)(null)), table18, "And ");
#line 108
 testRunner.Then("I get \'BadRequest\' response", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Cannot delete a rubric from a learning activity where the rubric is not associate" +
            "d")]
        public virtual void CannotDeleteARubricFromALearningActivityWhereTheRubricIsNotAssociated()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Cannot delete a rubric from a learning activity where the rubric is not associate" +
                    "d", ((string[])(null)));
#line 110
this.ScenarioSetup(scenarioInfo);
#line 8
this.FeatureBackground();
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
