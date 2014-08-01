using System;
using System.Collections.Generic;
using BpeProducts.Common.NHibernate;
using BpeProducts.Common.NHibernate.Version;

namespace BpeProducts.Services.Course.Domain.ProgramAggregates
{
    //[JsonObject(MemberSerialization.Fields)]
    //[Serializable]
    public class Program : VersionableEntity
    {
        private IList<CourseAggregates.Course> _courses = new List<CourseAggregates.Course>();

        private string _name;
        private string _description;
        private string _programType;
        private string _graduationRequirements;

        [NotNullable]
        public virtual string Name
        {
            get { return _name; }
            set
            {
                //CheckPublished();
                _name = value;
            }
        }

        public virtual string Description
        {
            get { return _description; }
            set
            {
                //CheckPublished();
                _description = value;
            }
        }

        [NotNullable]
        public virtual string ProgramType
        {
            get { return _programType; }
            set
            {
                //CheckPublished();
                _programType = value;
            }
        }

        public virtual string GraduationRequirements
        {
            get { return _graduationRequirements; }
            set
            {
                //CheckPublished();
                _graduationRequirements = value;
            }
        }

        public virtual IList<CourseAggregates.Course> Courses
        {
            get { return _courses; }
            set { _courses = value; }
        }

        protected override VersionableEntity Clone()
        {
            throw new NotImplementedException("this overload is not supported for program");
        }
        
    }
}