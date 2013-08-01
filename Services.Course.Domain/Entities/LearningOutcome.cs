using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Common.Exceptions;
using BpeProducts.Common.NHibernate.Version;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Domain.Entities
{
    public class LearningOutcome : VersionableEntity
    {
        private IList<LearningOutcome> _supportedOutcomes = new List<LearningOutcome>();
        private IList<LearningOutcome> _supportingOutcomes = new List<LearningOutcome>();

        private IList<ISupportingEntity> _supportingEntities = new List<ISupportingEntity>();

        [Required]
        public virtual string Description { get; set; }

        public virtual IList<LearningOutcome> SupportedOutcomes
        {
            get { return _supportedOutcomes; }
            protected internal set { _supportedOutcomes = value; }
        }

        public virtual IList<LearningOutcome> SupportingOutcomes
        {
            get { return _supportingOutcomes; }
            protected internal set { _supportingOutcomes = value; }
        }

        public virtual IList<ISupportingEntity> SupportingEntities
        {
            get { return _supportingEntities; }
            protected internal set { _supportingEntities = value; }
        }

        public virtual LearningOutcome SupportOutcome(IGraphValidator graphValidator, LearningOutcome outcome)
        {
            graphValidator.CheckCircularReferences(Id, outcome.Id);

            if (this == outcome)
                throw new ForbiddenException("Outcome cannot support itself");

            _supportedOutcomes.Add(outcome);
            outcome.SupportingOutcomes.Add(this);

            return this;
        }

        public virtual void UnsupportOutcome(LearningOutcome supportingOutcome)
        {
            _supportedOutcomes.Remove(supportingOutcome);
            supportingOutcome.SupportingOutcomes.Remove(this);
        }

        protected override VersionableEntity Clone()
        {
            return new LearningOutcome
            {
                Id = Guid.NewGuid(),
                Description = this.Description,
                _supportedOutcomes = new List<LearningOutcome>(_supportedOutcomes),
                _supportingOutcomes = new List<LearningOutcome>(_supportingOutcomes),
                _supportingEntities = new List<ISupportingEntity>(_supportingEntities),
                ActiveFlag = true,
                TenantId = this.TenantId,
                OrganizationId = this.OrganizationId
            };
        }
    }

    public interface IGraphValidator
    {
        void CheckCircularReferences(Guid first, Guid second);
    }

    class GraphValidator : IGraphValidator
    {
        private readonly ILearningOutcomeRepository _repository;

        public GraphValidator(ILearningOutcomeRepository repository)
        {
            _repository = repository;
        }

        public void CheckCircularReferences(Guid first, Guid second)
        {
            // check to make sure that the outcome graph does not have any circular dependencies
            return;
        }
    }
}
