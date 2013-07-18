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

        public Entities.Course Get(Guid programId)
        {
            var course = _session.Get<Entities.Course>(programId);
            course.IsBuilt = true;

            return course;
        }

        public void Save(Entities.Course course)
        {
            course.IsBuilt = false;
            _session.SaveOrUpdate(course);
        }

        public void Delete(Entities.Course course)
        {
            course.IsBuilt = false;
            course.ActiveFlag = false;
            _session.SaveOrUpdate(course);
        }

        public Entities.Course GetVersion(Guid originalEntityId, string versionNumber)
        {
            var version = (from c in _session.Query<Entities.Course>()
                           where c.OriginalEntity.Id == originalEntityId && c.VersionNumber == versionNumber
                         select c).FirstOrDefault();

            if (version != null) 
                version.IsBuilt = true;

            return version;
        }

        public IList<Entities.Course> ODataQuery(string queryString)
        {
            var criteria = _session.ODataQuery<Entities.Course>(queryString);
            criteria.Add(Restrictions.Eq("ActiveFlag", true));
            var courses = criteria.List<Entities.Course>();

            foreach (var course in courses)
            {
                course.IsBuilt = true;
            }

            return courses;
        }
    }
}