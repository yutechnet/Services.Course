using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Host.Tests.Integration.Operations;
using BpeProducts.Services.Course.Host.Tests.Integration.StepSetups;
using TechTalk.SpecFlow;

namespace BpeProducts.Services.Course.Host.Tests.Integration
{
    [Binding]
    public class GetEntityLearningOutcomeSteps
    {
        [Then(@"I get the entity learning outcomes as follows:")]
        public void ThenIGetTheEntityLearningOutcomesAsFollows(Table table)
        {
            var entityIds = new List<Guid>();
            foreach (var row in table.Rows)
            {
                var entityType = row["EntityType"];
                var entityName = row["EntityName"];

                switch (entityType)
                {
                    case "Program":
                        {
                            var program = Givens.Programs[entityName];
                            entityIds.Add(program.Id);
                        }
                        break;
                    case "Course":
                        
                        break;
                    case "Section":

                        break;
                }
            }

            var outcomes = GetOperations.GetEntityLearningOutcomes(entityIds);
        }
    }
}
