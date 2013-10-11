using System;
using System.Runtime.Serialization;

namespace BpeProducts.Services.Course.Host.TempSectionContracts
{
    [DataContract]
    public class UpdateSectionLearningActivityRequest
    {
        [DataMember(IsRequired = true)]
        public string Name { get; set; }
        [DataMember(IsRequired = true)]
        public SectionLearningActivityType Type { get; set; }
        [DataMember(IsRequired = true)]
        public Boolean IsGradeable { get; set; }
        [DataMember(IsRequired = true)]
        public Boolean IsExtraCredit { get; set; }
        [DataMember(IsRequired = true)]
        public int MaxPoint { get; set; }
        [DataMember(IsRequired = true)]
        public int Weight { get; set; }
    }
}