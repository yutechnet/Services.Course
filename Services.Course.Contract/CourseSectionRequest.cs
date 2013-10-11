using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BpeProducts.Services.Course.Contract
{
    [DataContract]
    public class CourseSectionRequest
    {
        [DataMember(IsRequired = true)]
        public Uri SectionServiceUri { get; set; }

        [DataMember(IsRequired = true)]
        public string Name { get; set; }

        [DataMember(IsRequired = true)]
        public string Code { get; set; }

        [DataMember(IsRequired = true)]
        public DateTime StartDate { get; set; }

        [DataMember]
        public DateTime? EndDate { get; set; }
    }
}
