using BpeProducts.Common.WebApi;

namespace BpeProducts.Services.Course.Contract
{
    public class OutcomeResponse : ResponseBase
    {
        public string Description { get; set; }
        public string VersionNumber { get; set; }
        public bool IsPublished { get; set; }
        public string PublishNote { get; set; }
    }
}
