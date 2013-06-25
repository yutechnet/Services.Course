using System;
using BpeProducts.Common.Ioc.Validation;
using BpeProducts.Common.NHibernate;
using NHibernate;

namespace BpeProducts.Services.Course.Domain.Repositories
{
    [Validate]
    public class CourseRepository : ICourseRepository
    {
        private readonly ISession _session;

        public CourseRepository(ISession session)
        {
            _session = session;
        }

        public Entities.Course Load(Guid programId)
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
    }
}