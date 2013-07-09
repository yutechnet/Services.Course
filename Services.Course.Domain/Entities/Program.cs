using System;
using System.Collections.Generic;
using BpeProducts.Common.NHibernate;

namespace BpeProducts.Services.Course.Domain.Entities
{
    public class Program : TenantEntity, IHaveOutcomes
    {
        [NotNullable]
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual Guid OrganizationId { get; set; }

        [NotNullable]
        public virtual string ProgramType { get; set; }

        public Program()
        {
            Courses = new List<Course>();
            Outcomes = new List<LearningOutcome>();
        }

        public virtual IList<Course> Courses { get; set; }

        public virtual IList<LearningOutcome> Outcomes { get; set; }
    }
}