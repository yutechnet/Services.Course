using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Common.NHibernate;

namespace BpeProducts.Services.Course.Domain.Entities
{
    public class Course : Entity
    {
        [Required]
        public virtual string Name { get; set; }
        [Required]
        public virtual string Code { get; set; }
        public virtual string Description { get; set; }
    }
}
