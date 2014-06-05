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
    [NUnit.Framework.DescriptionAttribute("ProgramAuthorization")]
    [NUnit.Framework.CategoryAttribute("Api")]
    public partial class ProgramAuthorizationFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "ProgramAuthorization.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "ProgramAuthorization", "In order to perform CRUD on program, \r\nI would need to have the required permissi" +
                    "on\r\nCreate/Edit/Delete - requires EditProgram capability\r\nView - requires ViewPr" +
                    "ogram capability", ProgrammingLanguage.CSharp, new string[] {
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
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("I can not create a program unless I have permission to do so.")]
        [NUnit.Framework.TestCaseAttribute("EditProgram", "Created", null)]
        [NUnit.Framework.TestCaseAttribute("", "Forbidden", null)]
        public virtual void ICanNotCreateAProgramUnlessIHavePermissionToDoSo_(string capability, string statusCode, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("I can not create a program unless I have permission to do so.", exampleTags);
#line 8
this.ScenarioSetup(scenarioInfo);
#line 9
 testRunner.Given(string.Format("I have the \'{0}\' capability", capability), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Description",
                        "ProgramType",
                        "OrganizationName",
                        "GraduationRequirements"});
            table1.AddRow(new string[] {
                        "Bachelor of Science",
                        "Economics",
                        "MA",
                        "Default",
                        "requirement one"});
#line 10
 testRunner.When("I create the following programs", ((string)(null)), table1, "When ");
#line 13
 testRunner.Then(string.Format("I get \'{0}\' response", statusCode), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("I can view a program when I do have permission.")]
        [NUnit.Framework.TestCaseAttribute("ViewProgram", "OK", null)]
        [NUnit.Framework.TestCaseAttribute("", "Forbidden", null)]
        public virtual void ICanViewAProgramWhenIDoHavePermission_(string capability, string statusCode, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("I can view a program when I do have permission.", exampleTags);
#line 19
this.ScenarioSetup(scenarioInfo);
#line 20
 testRunner.Given("I have the \'EditProgram\' capability", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Description",
                        "ProgramType",
                        "OrganizationName",
                        "GraduationRequirements"});
            table2.AddRow(new string[] {
                        "Bachelor of Science",
                        "Economics",
                        "MA",
                        "Default",
                        "requirement one"});
#line 21
 testRunner.When("I create the following programs", ((string)(null)), table2, "When ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "ObjectType",
                        "ObjectName",
                        "Capability"});
            table3.AddRow(new string[] {
                        "program",
                        "Bachelor of Science",
                        string.Format("{0}", capability)});
#line 24
 testRunner.Given("I have the following object capabilities", ((string)(null)), table3, "Given ");
#line 27
 testRunner.When("I get the program \'Bachelor of Science\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 28
 testRunner.Then(string.Format("I get \'{0}\' response", statusCode), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("I can edit a program when I do have permission.")]
        [NUnit.Framework.TestCaseAttribute("EditProgram", "OK", null)]
        [NUnit.Framework.TestCaseAttribute("", "Forbidden", null)]
        public virtual void ICanEditAProgramWhenIDoHavePermission_(string capability, string statusCode, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("I can edit a program when I do have permission.", exampleTags);
#line 35
this.ScenarioSetup(scenarioInfo);
#line 36
 testRunner.Given("I have the \'EditProgram\' capability", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Description",
                        "ProgramType",
                        "OrganizationName",
                        "GraduationRequirements"});
            table4.AddRow(new string[] {
                        "Bachelor of Science",
                        "Economics",
                        "MA",
                        "Default",
                        "requirement one"});
#line 37
 testRunner.When("I create the following programs", ((string)(null)), table4, "When ");
#line 40
 testRunner.When("I am \'TestUser1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "ObjectType",
                        "ObjectName",
                        "Capability"});
            table5.AddRow(new string[] {
                        "program",
                        "Bachelor of Science",
                        string.Format("{0}", capability)});
#line 41
 testRunner.Given("I have the following object capabilities", ((string)(null)), table5, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table6.AddRow(new string[] {
                        "Name",
                        "Bachelor of Arts"});
            table6.AddRow(new string[] {
                        "Description",
                        "English"});
            table6.AddRow(new string[] {
                        "ProgramType",
                        "BA"});
            table6.AddRow(new string[] {
                        "OrganizationId",
                        "E2DF063D-E2A1-4F83-9BE0-218EC676C05F"});
            table6.AddRow(new string[] {
                        "GraduationRequirements",
                        "requirement one update"});
#line 44
 testRunner.When("I modify the program \'Bachelor of Science\' info to reflect the following", ((string)(null)), table6, "When ");
#line 51
 testRunner.Then(string.Format("I get \'{0}\' response", statusCode), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("I can delete a program when I do have permission.")]
        [NUnit.Framework.TestCaseAttribute("EditProgram", "NoContent", null)]
        [NUnit.Framework.TestCaseAttribute("", "Forbidden", null)]
        public virtual void ICanDeleteAProgramWhenIDoHavePermission_(string capability, string statusCode, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("I can delete a program when I do have permission.", exampleTags);
#line 57
this.ScenarioSetup(scenarioInfo);
#line 58
 testRunner.Given("I have the \'EditProgram\' capability", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Description",
                        "ProgramType",
                        "OrganizationName",
                        "GraduationRequirements"});
            table7.AddRow(new string[] {
                        "Bachelor of Science",
                        "Economics",
                        "MA",
                        "Default",
                        "requirement one"});
#line 59
 testRunner.When("I create the following programs", ((string)(null)), table7, "When ");
#line 62
 testRunner.When("I am \'TestUser1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "ObjectType",
                        "ObjectName",
                        "Capability"});
            table8.AddRow(new string[] {
                        "program",
                        "Bachelor of Science",
                        string.Format("{0}", capability)});
#line 63
 testRunner.Given("I have the following object capabilities", ((string)(null)), table8, "Given ");
#line 66
 testRunner.When("I delete the program \'Bachelor of Science\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 67
 testRunner.Then(string.Format("I get \'{0}\' response", statusCode), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion