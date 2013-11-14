using System;
using System.Collections.Generic;

namespace BpeProducts.Services.Course.Contract
{
    public class OutcomeInfo 
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string VersionNumber { get; set; }
        public bool IsPublished { get; set; }
        public string PublishNote { get; set; }

        public List<OutcomeInfo> SupportedOutcomes { get; set; }
    }
}
