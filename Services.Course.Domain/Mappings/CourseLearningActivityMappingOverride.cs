﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Services.Course.Domain.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using BpeProducts.Services.Course.Domain.Courses;
using FluentNHibernate.Mapping;

namespace BpeProducts.Services.Course.Domain.Mappings
{
    public class CourseLearningActivityMappingOverride: IAutoMappingOverride<CourseLearningActivity>
    {
        public void Override(AutoMapping<CourseLearningActivity> mapping)
        {
            mapping.Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}