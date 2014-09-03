using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using BpeProducts.Common.Contract.Validation;

namespace BpeProducts.Services.Course.Contract
{
    [DataContract]
    public class CourseSectionRequest
    {
        [DataMember(IsRequired = true)]
        public string Name { get; set; }

        [DataMember(IsRequired = true)]
        public string CourseCode { get; set; }

        [DataMember(IsRequired = true)]
        public string SectionCode { get; set; }

        [DataMember(IsRequired = true)]
        [DisallowEmptyGuid]
        public Guid OrganizationId { get; set; }

        [DataMember(IsRequired = true)]
        public DateTime StartDate { get; set; }

        [DataMember]
        public DateTime? EndDate { get; set; }

        [DataMember]
        public string CorrelationId { get; set; }
    }
}
