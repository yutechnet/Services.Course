using System;
using System.Collections.Generic;
using BpeProducts.Common.NHibernate;
using Newtonsoft.Json;

namespace BpeProducts.Services.Course.Domain.Entities
{
	//[JsonObject(MemberSerialization.Fields)]
	//[Serializable]
    public class Program : TenantEntity
    {
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
    }
}