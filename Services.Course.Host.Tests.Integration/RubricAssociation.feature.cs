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
            table3.AddRow(new string[] {
                        "Econ 200",
                        "E200",
                        "Macroeconomics",
                        "COB",
                        "Traditional",
                        "False"});
#line 16
 testRunner.And("I have the following courses", ((string)(null)), table3, "And ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Note"});
            table4.AddRow(new string[] {
                        "Econ 200",
                        "a note"});
#line 20
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
#line 23
 testRunner.And("I have the following course segments for \'Econ 100\'", ((string)(null)), table5, "And ");
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
#line 26
 testRunner.And("I add the following course learning activities to \'Week 1\' course segment", ((string)(null)), table6, "And ");
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
#line 33
 testRunner.And("I have the following rubrics", ((string)(null)), table7, "And ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Associate multiple rubric id\'s to learning activity")]
        public virtual void AssociateMultipleRubricIdSToLearningActivity()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Associate multiple rubric id\'s to learning activity", ((string[])(null)));
#line 39
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 40
 testRunner.When("I add rubric \'Math101 Rubric\' to \'Custom 1\' learning activity", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 41
 testRunner.And("I add rubric \'Econ101 Rubric\' to \'Custom 1\' learning activity", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "Title",
                        "Description",
                        "OrganizationId",
                        "PerformanceLevel",
                        "ScoringModel",
                        "MinPoint",
                        "MaxPoint",
                        "IsPublished"});
            table8.AddRow(new string[] {
                        "Math101 Rubric",
                        "Basic math assessment tool",
                        "D2DF063D-E2A1-4F83-9BE0-218EC676C05F",
                        "Low, Med, High",
                        "Unset",
                        "50",
                        "100",
                        "True"});
            table8.AddRow(new string[] {
                        "Econ101 Rubric",
                        "Basic econ assessment tool",
                        "D2DF063D-E2A1-4F83-9BE0-218EC676C05F",
                        "Low, Med, High",
                        "Unset",
                        "50",
                        "100",
                        "True"});
#line 42
 testRunner.Then("the learning activity \'Custom 1\' should have the following rubrics", ((string)(null)), table8, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Verify unpublished rubrics cannot be associated to learning activities")]
        public virtual void VerifyUnpublishedRubricsCannotBeAssociatedToLearningActivities()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Verify unpublished rubrics cannot be associated to learning activities", ((string[])(null)));
#line 48
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 49
 testRunner.When("I add rubric \'Eng101 Rubric\' to \'Custom 1\' learning activity", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 50
 testRunner.Then("I get \'BadRequest\' response", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Can only add rubrics to learning activities of type Custom")]
        public virtual void CanOnlyAddRubricsToLearningActivitiesOfTypeCustom()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Can only add rubrics to learning activities of type Custom", ((string[])(null)));
#line 52
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 53
 testRunner.When("I add rubric \'Math101 Rubric\' to \'Assignment  1\' learning activity", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 54
 testRunner.Then("I get \'BadRequest\' response", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Cannot add one rubric more than once to the same learning activity")]
        public virtual void CannotAddOneRubricMoreThanOnceToTheSameLearningActivity()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Cannot add one rubric more than once to the same learning activity", ((string[])(null)));
#line 56
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 57
 testRunner.When("I add rubric \'Math101 Rubric\' to \'Custom 1\' learning activity", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 58
 testRunner.And("I add rubric \'Math101 Rubric\' to \'Custom 1\' learning activity", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 59
 testRunner.Then("I get \'BadRequest\' response", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Can only add rubrics to learning activities that are gradable")]
        public virtual void CanOnlyAddRubricsToLearningActivitiesThatAreGradable()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Can only add rubrics to learning activities that are gradable", ((string[])(null)));
#line 61
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 62
 testRunner.When("I add rubric \'Math101 Rubric\' to \'Custom 2\' learning activity", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 63
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
#line 65
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 66
 testRunner.When("I add rubric \'Math101 Rubric\' to \'Custom 1\' learning activity", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table9.AddRow(new string[] {
                        "Name",
                        "Custom 1"});
            table9.AddRow(new string[] {
                        "Type",
                        "Custom"});
            table9.AddRow(new string[] {
                        "IsGradeable",
                        "false"});
            table9.AddRow(new string[] {
                        "IsExtraCredit",
                        "false"});
            table9.AddRow(new string[] {
                        "Weight",
                        "100"});
            table9.AddRow(new string[] {
                        "MaxPoint",
                        "100"});
            table9.AddRow(new string[] {
                        "ObjectId",
                        "D2DF063D-E2A1-4F83-9BE0-218EC676C05F"});
#line 67
 testRunner.And("I update \'Custom 1\' learning activity with the following info", ((string)(null)), table9, "And ");
#line 76
 testRunner.Then("I get \'BadRequest\' response", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table10 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table10.AddRow(new string[] {
                        "Name",
                        "Custom 1"});
            table10.AddRow(new string[] {
                        "Type",
                        "Assignment"});
            table10.AddRow(new string[] {
                        "IsGradeable",
                        "true"});
            table10.AddRow(new string[] {
                        "IsExtraCredit",
                        "false"});
            table10.AddRow(new string[] {
                        "Weight",
                        "100"});
            table10.AddRow(new string[] {
                        "MaxPoint",
                        "100"});
            table10.AddRow(new string[] {
                        "ObjectId",
                        "D2DF063D-E2A1-4F83-9BE0-218EC676C05F"});
#line 77
 testRunner.And("I update \'Custom 3\' learning activity with the following info", ((string)(null)), table10, "And ");
#line 86
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
#line 88
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table11 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Note"});
            table11.AddRow(new string[] {
                        "Econ 100",
                        "a note"});
#line 89
 testRunner.When("I publish the following courses", ((string)(null)), table11, "When ");
#line 92
 testRunner.And("I add rubric \'Math101 Rubric\' to \'Custom 1\' learning activity", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 93
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
#line 95
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
