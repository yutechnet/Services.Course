using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using BpeProducts.Common.Contract.Validation;

namespace BpeProducts.Services.Course.Contract
{
    [DataContract]
    public class CreateCourseFromTemplateRequest
    {
        [DataMember(IsRequired = true)]
        [DisallowEmptyGuid]
        public Guid TemplateCourseId { get; set; }

        [DataMember(IsRequired = true)]
        public int TenantId { get; set; }

        [DataMember(IsRequired = true)]
        [DisallowEmptyGuid]
        public Guid OrganizationId { get; set; }

        [DataMember(IsRequired = true)]
        public bool IsTemplate { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string MetaData { get; set; }

        [DataMember]
        public List<Guid> ExtensionAssets { get; set; }
    }
}
