using System;
using BpeProducts.Common.Ioc.Validation;
using BpeProducts.Common.NHibernate;
using NHibernate;

namespace BpeProducts.Services.Course.Domain.Repositories
{
    [Validate]
    public class CourseRepository : RepositoryBase<Entities.Course>, ICourseRepository
    {
        public CourseRepository(ISession session)
            : base(session)
        {
        }

        public T Load<T>(Guid id) where T : TenantEntity
        {
            return Session.Load<T>(id);
        }
    }
}