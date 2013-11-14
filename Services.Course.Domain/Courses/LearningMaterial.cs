using BpeProducts.Common.NHibernate;

namespace BpeProducts.Services.Course.Domain.Courses
{
    public class LearningMaterial : TenantEntity
    {
        public virtual string Name { get; set; }
    }
}