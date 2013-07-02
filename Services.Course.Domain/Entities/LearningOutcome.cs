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
    public class LearningOutcome : TenantEntity,IHaveOutcomes, IVersionable<LearningOutcome>
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

        public virtual void SetOriginalEntity(LearningOutcome originalEntity)
        {
            this.OriginalEntity = originalEntity;
        }

        public virtual LearningOutcome OriginalEntity { get; set; }
        public virtual LearningOutcome ParentEntity { get; set; }
        public virtual string VersionNumber { get; set; }
        public virtual string PublishNote { get; set; }
        public virtual bool IsPublished { get; set; }
    }
}
