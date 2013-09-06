using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using System.Linq;
using System.Linq.Expressions;

namespace BpeProducts.Services.Course.Host.Tests.Integration.StepSetups
{
    public static class TableExtensions
    {
        public static string GetValue(this Table table, string key, string defaultValue)
        {
            foreach (var row in table.Rows)
            {
                if (row["Field"] == key)
                    return row["Value"];
            }

            return defaultValue;
        }

        public static void ReplaceRow(this Table table, string identifyingColumnName, string identifyingRowValue,Dictionary<string,string> newRow)
        {
            foreach (var row in table.Rows)
            {
                if (row[identifyingColumnName] == identifyingRowValue)
                {
                    foreach (var key in row.Keys)
                    {
                        row[key] = newRow[key];
                    }
                }
            }
            
        }
    }
}