using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace BpeProducts.Services.Course.Contract
{
    [DataContract]
    public class SaveCourseRequest
    {
        public SaveCourseRequest()
        {
            ProgramIds = new List<Guid>();
		}

        public Guid Id { get; set; }

        [DataMember]
        public Guid? TemplateCourseId { get; set; }
        
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Code { get; set; }
        
        [DataMember]
        public string Description { get; set; }

        [DataMember(IsRequired = true)]
        public ECourseType CourseType { get; set; }

        [DataMember(IsRequired = true)]
        public bool IsTemplate { get; set; }

        [DataMember(IsRequired = true)]
        public int TenantId { get; set; }

        [DataMember(IsRequired = true)]
        public Guid OrganizationId { get; set; }

        [DataMember]
        public List<Guid> ProgramIds { get; set; }

		[DataMember]
		public List<Guid> PrerequisiteCourseIds { get; set; }        
    }
}
