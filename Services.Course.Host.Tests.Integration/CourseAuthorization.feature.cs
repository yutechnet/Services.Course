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
    [NUnit.Framework.DescriptionAttribute("CourseAuthorization")]
    [NUnit.Framework.CategoryAttribute("Api")]
    public partial class CourseAuthorizationFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "CourseAuthorization.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "CourseAuthorization", "In order to protect the system\r\nAs a system owner\r\nI want to make sure that users" +
                    " cannot access resources they are not allowed to", ProgrammingLanguage.CSharp, new string[] {
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
        [NUnit.Framework.DescriptionAttribute("I can not create a course unless I have permission to do so.")]
        [NUnit.Framework.IgnoreAttribute()]
        [NUnit.Framework.TestCaseAttribute("eng101", "CourseCreate", "OrgTop", "OrgTop", "Created", null)]
        [NUnit.Framework.TestCaseAttribute("eng101", "CoursePublish", "OrgTop", "OrgTop", "Forbidden", null)]
        [NUnit.Framework.TestCaseAttribute("eng101", "", "OrgTop", "OrgTop", "Forbidden", null)]
        [NUnit.Framework.TestCaseAttribute("eng101", "CourseCreate", "OrgTop", "OrgMiddle", "Created", null)]
        [NUnit.Framework.TestCaseAttribute("eng101", "CourseCreate", "OrgMiddle", "OrgTop", "Forbidden", null)]
        public virtual void ICanNotCreateACourseUnlessIHavePermissionToDoSo_(string course, string capability, string organizationAssignedTo, string organizationCreatedAttempt, string statusCode, string[] exampleTags)
        {
            string[] @__tags = new string[] {
                    "ignore"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("I can not create a course unless I have permission to do so.", @__tags);
#line 7
this.ScenarioSetup(scenarioInfo);
#line 8
 testRunner.Given("I am user \"TestUser3\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Description",
                        "ParentOrganization"});
            table1.AddRow(new string[] {
                        "OrgTop",
                        "Top",
                        ""});
            table1.AddRow(new string[] {
                        "OrgMiddle",
                        "Middle",
                        "OrgTop"});
#line 9
 testRunner.And("the following organizations exist", ((string)(null)), table1, "And ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Organization",
                        "Capabilities"});
            table2.AddRow(new string[] {
                        "Role1",
                        "OrgTop",
                        string.Format("{0}", capability)});
#line 13
 testRunner.And("I create the following roles", ((string)(null)), table2, "And ");
#line 16
 testRunner.And(string.Format("I give the user role \"Role1\" for organization {0}", organizationAssignedTo), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 17
 testRunner.When(string.Format("I create a course {0} under organization {1}", course, organizationCreatedAttempt), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 18
 testRunner.Then(string.Format("I get \'{0}\' response", statusCode), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("I can not create a course when capabilities have been removed.")]
        [NUnit.Framework.IgnoreAttribute()]
        public virtual void ICanNotCreateACourseWhenCapabilitiesHaveBeenRemoved_()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("I can not create a course when capabilities have been removed.", new string[] {
                        "ignore"});
#line 29
this.ScenarioSetup(scenarioInfo);
#line 30
 testRunner.Given("I am user \"TestUser3\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Description",
                        "ParentOrganization"});
            table3.AddRow(new string[] {
                        "OrgTop",
                        "Top",
                        ""});
#line 31
 testRunner.And("the following organizations exist", ((string)(null)), table3, "And ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Organization",
                        "Capabilities"});
            table4.AddRow(new string[] {
                        "Role1",
                        "OrgTop",
                        "CourseCreate"});
#line 34
 testRunner.And("I create the following roles", ((string)(null)), table4, "And ");
#line 37
 testRunner.And("I give the user role \"Role1\" for organization OrgTop", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 38
 testRunner.And("I update the role \"Role1\" with capabilities \"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 39
 testRunner.When("I create a course eng101 under organization OrgTop", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 40
 testRunner.Then("I get \'Unauthorized\' response", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Create a course as a guest")]
        [NUnit.Framework.IgnoreAttribute()]
        public virtual void CreateACourseAsAGuest()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Create a course as a guest", new string[] {
                        "ignore"});
#line 43
this.ScenarioSetup(scenarioInfo);
#line 44
 testRunner.Given("That I am guest", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 45
 testRunner.When("I submit an authorized creation request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 46
 testRunner.Then("I should get a failure response", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("I can not view a course unless I have permission to do so.")]
        [NUnit.Framework.IgnoreAttribute()]
        [NUnit.Framework.TestCaseAttribute("", "", "CourseView", "eng101", "eng101", "Success", "#No org level, permission at object", null)]
        [NUnit.Framework.TestCaseAttribute("CourseView", "OrgMiddle", "", "", "", "Success", "#Org level permission, no object level", null)]
        [NUnit.Framework.TestCaseAttribute("CourseView", "OrgTop", "", "", "", "Success", "#parent org level perm, no object level", null)]
        [NUnit.Framework.TestCaseAttribute("CourseView", "OrgTop", "CourseView", "eng101", "", "Success", "#org level and object level permission", null)]
        [NUnit.Framework.TestCaseAttribute("", "", "", "", "", "Fail", "#no permissions", null)]
        public virtual void ICanNotViewACourseUnlessIHavePermissionToDoSo_(string orgCapability, string orgLevel, string objectCapability, string objectAssignedTo, string courseAssignedCapability, string statusCode, string description, string[] exampleTags)
        {
            string[] @__tags = new string[] {
                    "ignore"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("I can not view a course unless I have permission to do so.", @__tags);
#line 51
this.ScenarioSetup(scenarioInfo);
#line 52
 testRunner.Given("I am user \"TestUser3\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Description",
                        "ParentOrganization"});
            table5.AddRow(new string[] {
                        "OrgTop",
                        "Top",
                        ""});
            table5.AddRow(new string[] {
                        "OrgMiddle",
                        "Middle",
                        "OrgTop"});
#line 53
 testRunner.And("the following organizations exist", ((string)(null)), table5, "And ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Organization",
                        "Capabilities"});
            table6.AddRow(new string[] {
                        "CreateCourseRole",
                        "OrgTop",
                        "CourseCreate"});
            table6.AddRow(new string[] {
                        "ViewOrNothingCourseRole",
                        string.Format("{0}", orgLevel),
                        string.Format("{0}", orgCapability)});
#line 57
 testRunner.And("I create the following roles", ((string)(null)), table6, "And ");
#line 61
 testRunner.And("I give the user role \"CreateCourseRole\" for organization OrgTop", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 62
 testRunner.And(string.Format("I give the user role \"ViewCourseRole\" for organization {0}", orgLevel), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 63
 testRunner.And("I create a course \'eng101\' under organization \'OrgMiddle\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 66
 testRunner.Then(string.Format("I get \'{0}\' response", statusCode), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
