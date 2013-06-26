using System;
using BpeProducts.Common.NHibernate.Version;

namespace BpeProducts.Services.Course.Domain.Repositories
{
    public interface ICourseRepository
    {
        Entities.Course Load(Guid programId);
        void Save(Entities.Course course);
        void Delete(Entities.Course course);
        Entities.Course GetVersion(Guid originalEntityId, string versionNumber);
    }
}
