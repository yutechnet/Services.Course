using System.Collections.Generic;
using BpeProducts.Common.WebApi;

namespace BpeProducts.Services.Course.Contract
{
    public class OutcomeInfo : ResponseBase
    {
        public string Description { get; set; }
        public string VersionNumber { get; set; }
        public bool IsPublished { get; set; }
        public string PublishNote { get; set; }

        public List<OutcomeInfo> SupportedOutcomes { get; set; }
    }
}
