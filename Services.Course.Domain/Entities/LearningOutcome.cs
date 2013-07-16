using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Common.NHibernate;
using BpeProducts.Common.NHibernate.Version;

namespace BpeProducts.Services.Course.Domain.Entities
{
    public class LearningOutcome : VersionableEntity, IHaveOutcomes 
    {
        [Required]
        public virtual string Description { get; set; }
		public virtual IList<LearningOutcome> Outcomes { get; set; }
		//public virtual IList<IHaveOutcomes> AssociatedEntities { get; set; } 

        public LearningOutcome()
	    {
		    Outcomes = new List<LearningOutcome>();
			//AssociatedEntities = new List<IHaveOutcomes>();
	    }

        protected override VersionableEntity Clone()
        {
            return new LearningOutcome
                {
                    Id = Guid.NewGuid(),
                    Description = this.Description,
                    Outcomes = new List<LearningOutcome>(this.Outcomes),
                    ActiveFlag = true,
                    TenantId = this.TenantId,
                    OrganizationId = this.OrganizationId
                };
        }
    }
}
