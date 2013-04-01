﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Common.NHibernate;
using NHibernate;

namespace BpeProducts.Services.Course.Domain.Repositories
{
    public class CourseRepository : RepositoryBase<Entities.Course>, ICourseRepository
    {
        public CourseRepository(ISession session)
            : base(session)
        {
            
        }
    }
}
