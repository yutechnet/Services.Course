﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BpeProducts.Services.Course.Contract
{
    public enum CourseLearningActivityType
    {
        Discussion = 1,
        Assignment = 2,
        Quiz = 3,
        Assessment = 4
    }

    public class CourseLearningActivityResponse
    {
        public Guid Id { get; set; }
        public int TenantId { get; set; }
        public string Name { get; set; }
        public CourseLearningActivityType Type { get; set; }
        public Boolean IsGradeable { get; set; }
        public Boolean IsExtraCredit { get; set; }
        public int MaxPoint { get; set; }
        public int Weight { get; set; }
        public Guid ObjectId { get; set; }
        public int ActiveDate { get; set; }
        public int InactiveDate { get; set; }
        public int DueDate { get; set; }
    }
}
