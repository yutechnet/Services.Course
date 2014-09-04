using System;
using System.Collections.Generic;
using BpeProducts.Common.WebApiTest.Framework;
using BpeProducts.Services.Assessment.Contract;

namespace BpeProducts.Services.Course.Host.Tests.Integration.Resources.Assessment
{
    public interface IOutcomeInfoResource : IResource
    {
        string CorrelationId { get; set; }
        string Description { get; set; }
        bool IsPublished { get; set; }
        DateTime? PublishDate { get; set; }
        string PublishNote { get; set; }
        List<OutcomeInfo> SupportedOutcomes { get; set; }
        string Tag { get; set; }
        int TenantId { get; set; }
        string Title { get; set; }
        string VersionNumber { get; set; }
    }
}
