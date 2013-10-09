using System.Runtime.Serialization;

namespace BpeProducts.Services.Course.Host.TempSectionContracts
{
    [DataContract]
    public class UpdateSectionSegmentRequest
    {
        [DataMember(IsRequired = true)]
        public string Name { get; set; }
        [DataMember(IsRequired = true)]
        public string Description { get; set; }
        [DataMember(IsRequired = true)]
        public string Type { get; set; }
    }
}