using System;
using System.Collections.Generic;
using BpeProducts.Common.NHibernate;

namespace BpeProducts.Services.Course.Domain.ProgramAggregates
{
	//[JsonObject(MemberSerialization.Fields)]
	//[Serializable]
    public class Program : TenantEntity
    {
        private IList<CourseAggregates.Course> _courses = new List<CourseAggregates.Course>();

        [NotNullable]
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual Guid OrganizationId { get; set; }

        [NotNullable]
        public virtual string ProgramType { get; set; }

        public virtual string GraduationRequirements { get; set; }

        public virtual IList<CourseAggregates.Course> Courses
        {
            get { return _courses; }
            set { _courses = value; }
        }
    }
}