using System;
using System.Collections.Generic;
using BpeProducts.Common.NHibernate;

namespace BpeProducts.Services.Course.Domain.Entities
{
    public class Program : TenantEntity, ISupportingEntity
    {
        private IList<LearningOutcome> _supportedOutcomes = new List<LearningOutcome>();
        private IList<Courses.Course> _courses = new List<Courses.Course>();

        [NotNullable]
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual Guid OrganizationId { get; set; }

        [NotNullable]
        public virtual string ProgramType { get; set; }

        public virtual IList<Courses.Course> Courses
        {
            get { return _courses; }
            set { _courses = value; }
        }

        public virtual IList<LearningOutcome> SupportedOutcomes
        {
            get { return _supportedOutcomes; }
            protected internal set { _supportedOutcomes = value; }
        }

        public virtual void SupportOutcome(LearningOutcome outcome)
        {
            _supportedOutcomes.Add(outcome);
            outcome.SupportingEntities.Add(this);
        }

        public virtual void UnsupportOutcome(LearningOutcome outcome)
        {
            _supportedOutcomes.Remove(outcome);
            outcome.SupportingEntities.Remove(this);
        }
    }
}