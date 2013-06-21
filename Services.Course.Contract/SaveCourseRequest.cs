using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BpeProducts.Services.Course.Contract
{
    public class SaveCourseRequest
    {
        public SaveCourseRequest()
        {
            ProgramIds = new List<Guid>();
		}

        public Guid Id { get; set; }

        public Guid TemplateCourseId { get; set; }

        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int TenantId { get; set; }
        public Guid OrganizationId { get; set; }

        public List<Guid> ProgramIds { get; set; }

        
    }
}
