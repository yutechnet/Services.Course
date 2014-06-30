﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.18063
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
    [NUnit.Framework.DescriptionAttribute("ProgramAssociation")]
    [NUnit.Framework.CategoryAttribute("Api")]
    public partial class ProgramAssociationFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "ProgramAssociation.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "ProgramAssociation", "In order to add course to programs\r\nAs a course builder\r\nI want to associate cour" +
                    "se to programs", ProgrammingLanguage.CSharp, new string[] {
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
                        "EditProgram"});
            table2.AddRow(new string[] {
                        "ViewProgram"});
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
            table3.AddRow(new string[] {
                        "asset3"});
#line 19
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
            table4.AddRow(new string[] {
                        "asset3",
                        "published"});
#line 24
 testRunner.And("Published the following assets", ((string)(null)), table4, "And ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Description",
                        "ProgramType",
                        "OrganizationName"});
            table5.AddRow(new string[] {
                        "Bachelor of Art",
                        "BA Program",
                        "BA",
                        "COB"});
            table5.AddRow(new string[] {
                        "Bachelor of Science",
                        "BS program",
                        "BS",
                        "COB"});
#line 29
 testRunner.And("I have the following programs", ((string)(null)), table5, "And ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "Description",
                        "OrganizationName",
                        "MetaData",
                        "ExtensionAssets"});
            table6.AddRow(new string[] {
                        "English 101",
                        "ENG101",
                        "English 101",
                        "COB",
                        "{someData}",
                        "asset1,asset2"});
            table6.AddRow(new string[] {
                        "Psychology 101",
                        "PSY101",
                        "Psychology 101",
                        "COB",
                        "{someData}",
                        "asset1,asset2"});
            table6.AddRow(new string[] {
                        "Econ 100",
                        "E100",
                        "Macroeconomics",
                        "COB",
                        "{someData}",
                        "asset1,asset2"});
            table6.AddRow(new string[] {
                        "Econ 400",
                        "E400",
                        "Microeconomics",
                        "COB",
                        "{someData}",
                        "asset1,asset2"});
#line 33
 testRunner.And("I have the following courses", ((string)(null)), table6, "And ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Associate a course with a program")]
        [NUnit.Framework.TestCaseAttribute("English 101", "Bachelor of Art", null)]
        [NUnit.Framework.TestCaseAttribute("Psychology 101", "Bachelor of Science", null)]
        [NUnit.Framework.TestCaseAttribute("English 101", "Bachelor of Science", null)]
        [NUnit.Framework.TestCaseAttribute("Psychology 101", "Bachelor of Art", null)]
        public virtual void AssociateACourseWithAProgram(string courseName, string programName, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Associate a course with a program", exampleTags);
#line 40
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 41
 testRunner.When(string.Format("I associate \'{0}\' course with \'{1}\' program", courseName, programName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 42
 testRunner.Then(string.Format("the course \'{0}\' includes \'{1}\' program association", courseName, programName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Associate a course with multiple programs")]
        public virtual void AssociateACourseWithMultiplePrograms()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Associate a course with multiple programs", ((string[])(null)));
#line 51
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "Program Name"});
            table7.AddRow(new string[] {
                        "Bachelor of Art"});
            table7.AddRow(new string[] {
                        "Bachelor of Science"});
#line 52
 testRunner.When("I associate \'English 101\' course with the following programs", ((string)(null)), table7, "When ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "Program Name"});
            table8.AddRow(new string[] {
                        "Bachelor of Art"});
            table8.AddRow(new string[] {
                        "Bachelor of Science"});
#line 56
 testRunner.Then("the course \'English 101\' includes the following program information", ((string)(null)), table8, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Remove the course from the program")]
        public virtual void RemoveTheCourseFromTheProgram()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Remove the course from the program", ((string[])(null)));
#line 61
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                        "Program Name"});
            table9.AddRow(new string[] {
                        "Bachelor of Art"});
            table9.AddRow(new string[] {
                        "Bachelor of Science"});
#line 62
 testRunner.When("I associate \'English 101\' course with the following programs", ((string)(null)), table9, "When ");
#line 66
 testRunner.And("I remove \'English 101\' course from \'Bachelor of Art\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 67
 testRunner.Then("the course \'English 101\' includes \'Bachelor of Science\' program association", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Get program with all courses that belong to it")]
        public virtual void GetProgramWithAllCoursesThatBelongToIt()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Get program with all courses that belong to it", ((string[])(null)));
#line 69
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table10 = new TechTalk.SpecFlow.Table(new string[] {
                        "Program Name"});
            table10.AddRow(new string[] {
                        "Bachelor of Art"});
#line 70
 testRunner.When("I associate \'English 101\' course with the following programs", ((string)(null)), table10, "When ");
#line hidden
            TechTalk.SpecFlow.Table table11 = new TechTalk.SpecFlow.Table(new string[] {
                        "Program Name"});
            table11.AddRow(new string[] {
                        "Bachelor of Art"});
#line 73
 testRunner.And("I associate \'Psychology 101\' course with the following programs", ((string)(null)), table11, "And ");
#line hidden
            TechTalk.SpecFlow.Table table12 = new TechTalk.SpecFlow.Table(new string[] {
                        "Course Name",
                        "Code",
                        "Description"});
            table12.AddRow(new string[] {
                        "English 101",
                        "ENG101",
                        "English 101"});
            table12.AddRow(new string[] {
                        "Psychology 101",
                        "PSY101",
                        "Psychology 101"});
#line 76
 testRunner.Then("the program \'Bachelor of Art\' include the following course information", ((string)(null)), table12, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Verify a course version can be created from a previously published version with p" +
            "rerequisites")]
        public virtual void VerifyACourseVersionCanBeCreatedFromAPreviouslyPublishedVersionWithPrerequisites()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Verify a course version can be created from a previously published version with p" +
                    "rerequisites", ((string[])(null)));
#line 81
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table13 = new TechTalk.SpecFlow.Table(new string[] {
                        "Program Name"});
            table13.AddRow(new string[] {
                        "Bachelor of Art"});
#line 82
 testRunner.When("I associate \'Econ 100\' course with the following programs", ((string)(null)), table13, "When ");
#line hidden
            TechTalk.SpecFlow.Table table14 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Note"});
            table14.AddRow(new string[] {
                        "Econ 100",
                        "a note"});
#line 85
 testRunner.And("I publish the following courses", ((string)(null)), table14, "And ");
#line hidden
            TechTalk.SpecFlow.Table table15 = new TechTalk.SpecFlow.Table(new string[] {
                        "Program Name"});
            table15.AddRow(new string[] {
                        "Bachelor of Art"});
#line 88
 testRunner.And("I associate \'Econ 400\' course with the following programs", ((string)(null)), table15, "And ");
#line hidden
            TechTalk.SpecFlow.Table table16 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name"});
            table16.AddRow(new string[] {
                        "Econ 100"});
#line 91
 testRunner.And("I add the following prerequisites to \'Econ 400\'", ((string)(null)), table16, "And ");
#line hidden
            TechTalk.SpecFlow.Table table17 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Note"});
            table17.AddRow(new string[] {
                        "Econ 400",
                        "a note"});
#line 94
 testRunner.And("I publish the following courses", ((string)(null)), table17, "And ");
#line hidden
            TechTalk.SpecFlow.Table table18 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table18.AddRow(new string[] {
                        "VersionNumber",
                        "1.0.0.1"});
#line 97
 testRunner.And("I create a new version of \'Econ 400\' course named \'Econ 400 v1.0.0.1\' with the fo" +
                    "llowing info", ((string)(null)), table18, "And ");
#line hidden
            TechTalk.SpecFlow.Table table19 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name"});
            table19.AddRow(new string[] {
                        "Econ 100"});
#line 100
 testRunner.Then("the course \'Econ 400 v1.0.0.1\' should have the following prerequisites", ((string)(null)), table19, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Search for programs with courses")]
        public virtual void SearchForProgramsWithCourses()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Search for programs with courses", ((string[])(null)));
#line 104
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table20 = new TechTalk.SpecFlow.Table(new string[] {
                        "Program Name"});
            table20.AddRow(new string[] {
                        "Bachelor of Art"});
            table20.AddRow(new string[] {
                        "Bachelor of Science"});
#line 105
 testRunner.When("I associate \'English 101\' course with the following programs", ((string)(null)), table20, "When ");
#line hidden
            TechTalk.SpecFlow.Table table21 = new TechTalk.SpecFlow.Table(new string[] {
                        "Program Name"});
            table21.AddRow(new string[] {
                        "Bachelor of Art"});
#line 109
 testRunner.And("I associate \'Psychology 101\' course with the following programs", ((string)(null)), table21, "And ");
#line hidden
            TechTalk.SpecFlow.Table table22 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Description",
                        "ProgramType",
                        "OrganizationName"});
            table22.AddRow(new string[] {
                        "Bachelor of Art",
                        "BA Program",
                        "BA",
                        "COB"});
            table22.AddRow(new string[] {
                        "Bachelor of Science",
                        "BS program",
                        "BS",
                        "COB"});
#line 112
 testRunner.Then("the organization \'COB\' has the following programs", ((string)(null)), table22, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
