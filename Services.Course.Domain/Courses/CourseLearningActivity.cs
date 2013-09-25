using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Entities;

namespace BpeProducts.Services.Course.Domain.Courses
{
    public class CourseLearningActivity : TenantEntity
    {
        [Required]
        public virtual string Name { get; set; }

        [Required]
        public virtual CourseLearningActivityType Type { get; set; }

        public virtual Boolean IsGradeable { get; set; }

        public virtual Boolean IsExtraCredit { get; set; }

        public virtual int MaxPoint { get; set; }

        public virtual int Weight { get; set; }

        public virtual Guid ObjectId { get; set; }

        public virtual int ActiveDate { get; set; }
        public virtual int InactiveDate { get; set; }
        public virtual int DueDate { get; set; }
    }

}
