using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Common.NHibernate;

namespace BpeProducts.Services.Course.Domain.Repositories
{
    public interface ICourseRepository
    {
        Entities.Course Load(Guid programId);
        void Save(Entities.Course course);
        void Delete(Entities.Course course);
    }
}
