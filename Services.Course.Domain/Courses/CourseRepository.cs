using System;
using System.Collections.Generic;
using System.Linq;
using BpeProducts.Common.Ioc.Validation;
using BpeProducts.Common.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.OData;

namespace BpeProducts.Services.Course.Domain.Repositories
{
    [Validate]
    // TODO Opportunity to use Repository base class.
    public class CourseRepository : ICourseRepository
    {
        private readonly ISession _session;

        public CourseRepository(ISession session)
        {
            _session = session;
        }

        public Courses.Course Get(Guid programId)
        {
            var course = _session.Get<Courses.Course>(programId);
            course.IsBuilt = true;

            return course;
        }

        public void Save(Courses.Course course)
        {
            course.IsBuilt = false;
            _session.SaveOrUpdate(course);
        }

        public void Delete(Courses.Course course)
        {
            course.IsBuilt = false;
            course.ActiveFlag = false;
            _session.SaveOrUpdate(course);
        }

        public Courses.Course GetVersion(Guid originalEntityId, string versionNumber)
        {
            var version = (from c in _session.Query<Courses.Course>()
                           where c.OriginalEntity.Id == originalEntityId && c.VersionNumber == versionNumber
                         select c).FirstOrDefault();

            if (version != null) 
                version.IsBuilt = true;

            return version;
        }

        public IList<Courses.Course> ODataQuery(string queryString)
        {
            var criteria = _session.ODataQuery<Courses.Course>(queryString);
            criteria.Add(Restrictions.Eq("ActiveFlag", true));
            var courses = criteria.List<Courses.Course>();

            foreach (var course in courses)
            {
                course.IsBuilt = true;
            }

            return courses;
        }
    }
}