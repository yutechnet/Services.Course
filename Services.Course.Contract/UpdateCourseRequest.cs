using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BpeProducts.Services.Course.Contract
{
    [DataContract]
    public class UpdateCourseRequest
    {
        public UpdateCourseRequest()
        {
            ProgramIds = new List<Guid>();
            PrerequisiteCourseIds = new List<Guid>();
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember(IsRequired = true)]
        public ECourseType CourseType { get; set; }

        [DataMember]
        public List<Guid> ProgramIds { get; set; }

        [DataMember]
        public List<Guid> PrerequisiteCourseIds { get; set; }

        [DataMember]
        public bool IsTemplate { get; set; }

        [DataMember]
        public decimal Credit { get; set; }

        [DataMember]
        public string MetaData { get; set; }

        [DataMember]
        public List<Guid> ExtensionAssets { get; set; }

        [DataMember]
        public string CorrelationId { get; set; }
    }
}