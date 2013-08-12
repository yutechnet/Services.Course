using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Host.Tests.Integration.Operations;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources;
using BpeProducts.Services.Course.Host.Tests.Integration.StepSetups;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace BpeProducts.Services.Course.Host.Tests.Integration
{
    [Binding]
    public class GetEntityLearningOutcomeSteps
    {
        [Then(@"I get the entity learning outcomes as follows:")]
        public void ThenIGetTheEntityLearningOutcomesAsFollows(Table table)
        {
            var expectedEntityOutcomes = new Dictionary<IResource, IList<Guid>>();
            foreach (var row in table.Rows)
            {
                var entityType = row["EntityType"];
                var entityName = row["EntityName"];
                IResource resource = null;

                switch (entityType)
                {
                    case "Program":
                        resource = Givens.Programs[entityName];
                        break;
                    case "Course":
                        resource = Givens.Courses[entityName];
                        break;
                    case "Segment":
                        resource = Givens.Segments[entityName];
                        break;
                }

                if (resource == null) 
                    throw new Exception("No recourse found for entity type " + entityType);

                var expectedOutcomes = (from o in row["LearningOutcomes"].Split(new[] { ',' }) where !string.IsNullOrWhiteSpace(o) select Givens.LearningOutcomes[o.Trim()].Id).ToList();
                expectedEntityOutcomes.Add(resource, expectedOutcomes);
            }

            var entityIdsToGet = (from e in expectedEntityOutcomes.Keys select e.Id).ToList();
            var actualEntityOutcomes = GetOperations.GetEntityLearningOutcomes(entityIdsToGet);

            foreach (var expectedEntity in expectedEntityOutcomes)
            {
                List<OutcomeInfo> outcomes;

                if (actualEntityOutcomes.TryGetValue(expectedEntity.Key.Id, out outcomes))
                {
                    var actualEntityOutcomeIds = (from o in outcomes select o.Id).ToList();
                    CollectionAssert.AreEquivalent(expectedEntity.Value.ToList(), actualEntityOutcomeIds);
                }
                else
                {
                    Assert.That(!actualEntityOutcomes.Keys.Contains(expectedEntity.Key.Id));
                }
            }
        }
    }
}
