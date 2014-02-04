using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BpeProducts.Services.Course.Contract
{
    [DataContract]
    public class CreateCourseFromTemplateRequest
    {
        [DataMember(IsRequired = true)]
        public Guid TemplateCourseId { get; set; }

        [DataMember(IsRequired = true)]
        public int TenantId { get; set; }

        [DataMember(IsRequired = true)]
        public Guid OrganizationId { get; set; }

        [DataMember(IsRequired = true)]
        public bool IsTemplate { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}
