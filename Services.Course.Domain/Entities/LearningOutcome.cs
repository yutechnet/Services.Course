using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Common.NHibernate;

namespace BpeProducts.Services.Course.Domain.Entities
{
    public class LearningOutcome : TenantEntity,IHaveOutcomes
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
    }
}
