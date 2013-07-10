using System;
using BpeProducts.Common.NHibernate.Version;
using NHibernate;

namespace BpeProducts.Services.Course.Domain.Repositories
{
    public interface ICourseRepository
    {
        Entities.Course Get(Guid courseId);
        void Save(Entities.Course course);
        void Delete(Entities.Course course);
        Entities.Course GetVersion(Guid originalCourseId, string versionNumber);
        ICriteria ODataQuery(string queryString);
    }
}
