using System;
using System.Collections.Generic;
using BpeProducts.Common.NHibernate;

namespace BpeProducts.Services.Course.Domain.Entities
{
    public class Program : TenantEntity, IHaveOutcomes
    {
        private IList<LearningOutcome> _outcomes = new List<LearningOutcome>();
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

        public virtual IList<LearningOutcome> SupportingOutcomes
        {
            get { return _outcomes; }
            protected internal set { _outcomes = value; }
        }

        public virtual LearningOutcome SupportOutcome(LearningOutcome outcome)
        {
            _outcomes.Add(outcome);
            return outcome;
        }
    }
}