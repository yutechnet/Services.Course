using System;
using TechTalk.SpecFlow;

namespace BpeProducts.Services.Course.Host.Tests.Integration
{
    [Binding]
    public class LearningOutcomeVersioningSteps
    {
        [Given(@"I create the following learning outcome")]
        public void GivenICreateTheFollowingLearningOutcome(Table table)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"I publish '(.*)' learning outcome with the following info")]
        public void GivenIPublishLearningOutcomeWithTheFollowingInfo(string p0, Table table)
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"I retrieve '(.*)' learning outcome")]
        public void WhenIRetrieveLearningOutcome(string p0)
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"I update '(.*)' learning outcome with the following info")]
        public void WhenIUpdateLearningOutcomeWithTheFollowingInfo(string p0, Table table)
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"I publish '(.*)' learning outcome with the following info")]
        public void WhenIPublishLearningOutcomeWithTheFollowingInfo(string p0, Table table)
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"I delete '(.*)' learning outcome")]
        public void WhenIDeleteLearningOutcome(string p0)
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"I create a learning outcome without a version")]
        public void WhenICreateALearningOutcomeWithoutAVersion()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"the learning outcome should have the following info")]
        public void ThenTheLearningOutcomeShouldHaveTheFollowingInfo(Table table)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"the learning outcome '(.*)' should have the following info")]
        public void ThenTheLearningOutcomeShouldHaveTheFollowingInfo(string p0, Table table)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
