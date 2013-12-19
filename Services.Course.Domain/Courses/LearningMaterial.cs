using System;
using BpeProducts.Common.NHibernate;

namespace BpeProducts.Services.Course.Domain.Courses
{
    public class LearningMaterial : TenantEntity
    {
        public virtual Guid AssetId { get; set; }
        public virtual Guid CourseSegmentId { get; set; }
        public virtual string Instruction { get; set; }
        public virtual bool IsRequired { get; set; }

    }
}