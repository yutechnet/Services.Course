using System;
using System.Collections.Generic;
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
        IList<Entities.Course> ODataQuery(string queryString);
    }
}
