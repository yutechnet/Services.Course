using System;
using System.Collections.Generic;
using AutoMapper;
using BpeProducts.Common.NHibernate;
using BpeProducts.Common.NHibernate.Version;

namespace BpeProducts.Services.Course.Domain.ProgramAggregates
{
    //[JsonObject(MemberSerialization.Fields)]
    //[Serializable]
    public class Program : VersionableEntity
    {
        private IList<CourseAggregates.Course> _courses = new List<CourseAggregates.Course>();

        [NotNullable]
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        [NotNullable]
        public virtual string ProgramType { get; set; }

        public virtual string GraduationRequirements { get; set; }

        public virtual IList<CourseAggregates.Course> Courses
        {
            get { return _courses; }
            set { _courses = value; }
        }

        protected override VersionableEntity Clone()
        {
            var program = Mapper.Map<Program>(this);
            program.Id = Guid.NewGuid();
            return program;
        }


        public virtual void Update(Contract.UpdateProgramRequest request)
        {
            CheckPublished();
            Mapper.Map(request, this);
        }

        public virtual void Delete()
        {
            CheckPublished();
            IsDeleted = true;
        }
    }
}