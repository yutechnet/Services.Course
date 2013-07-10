using System;
using System.Linq;
using BpeProducts.Common.Ioc.Validation;
using BpeProducts.Common.NHibernate;
using NHibernate;
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
            return _session.Get<Entities.Course>(programId);
        }

        public void Save(Entities.Course course)
        {
            _session.SaveOrUpdate(course);
        }

        public void Delete(Entities.Course course)
        {
            course.ActiveFlag = false;
            _session.SaveOrUpdate(course);
        }

        public Entities.Course GetVersion(Guid originalEntityId, string versionNumber)
        {
            var version = (from c in _session.Query<Entities.Course>()
                           where c.OriginalEntity.Id == originalEntityId && c.VersionNumber == versionNumber
                         select c).FirstOrDefault();

            return version;
        }

        public ICriteria ODataQuery(string queryString)
        {
            return _session.ODataQuery<Entities.Course>(queryString);
        }

    }
}