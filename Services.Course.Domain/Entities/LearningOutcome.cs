using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Common.Exceptions;
using BpeProducts.Common.NHibernate;
using BpeProducts.Common.NHibernate.Version;

namespace BpeProducts.Services.Course.Domain.Entities
{
    public class LearningOutcome : VersionableEntity, IHaveOutcomes 
    {
        private IList<LearningOutcome> _supportingOutcomes = new List<LearningOutcome>();

        [Required]
        public virtual string Description { get; set; }

		public virtual IList<LearningOutcome> SupportingOutcomes
		{
		    get { return _supportingOutcomes; }
		    protected internal set { _supportingOutcomes = value; }
		}

        public virtual LearningOutcome SupportOutcome(LearningOutcome outcome)
        {
            if(outcome == this)
                throw new ForbiddenException("Outcome cannot support itself");

            _supportingOutcomes.Add(outcome);
            return this;
        }

        public virtual void UnsupportOutcome(LearningOutcome supportingOutcome)
        {
            _supportingOutcomes.Remove(supportingOutcome);
        }

        protected override VersionableEntity Clone()
        {
            return new LearningOutcome
                {
                    Id = Guid.NewGuid(),
                    Description = this.Description,
                    SupportingOutcomes = new List<LearningOutcome>(this.SupportingOutcomes),
                    ActiveFlag = true,
                    TenantId = this.TenantId,
                    OrganizationId = this.OrganizationId
                };
        }
    }
}
