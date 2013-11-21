using System;
using BpeProducts.Common.NHibernate;

namespace BpeProducts.Services.Course.Domain.Courses
{
    public class LearningMaterial : TenantEntity
    {
        public virtual string Description { get; set; }
        public virtual Guid LibraryItemId { get; set; }
    }
}