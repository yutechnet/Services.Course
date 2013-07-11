using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Common.NHibernate;
using BpeProducts.Common.NHibernate.Version;
using BpeProducts.Services.Course.Contract;

namespace BpeProducts.Services.Course.Domain
{
    public interface IHandleVersioning<T> where T : TenantEntity
    {
        void PublishVersion(Guid entityId, string publishNote);
        T CreateVersion(VersionRequest request);
    }
}
