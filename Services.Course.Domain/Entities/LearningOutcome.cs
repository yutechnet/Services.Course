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
        private IList<LearningOutcome> _outcomes = new List<LearningOutcome>();

        [Required]
        public virtual string Description { get; set; }

		public virtual IList<LearningOutcome> Outcomes
		{
		    get { return _outcomes; }
		    protected internal set { _outcomes = value; }
		}

        public virtual LearningOutcome AddLearningOutcome(LearningOutcome outcome)
        {
            _outcomes.Add(outcome);
            return this;
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
