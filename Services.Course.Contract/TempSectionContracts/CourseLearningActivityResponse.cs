﻿using System;

namespace BpeProducts.Services.Course.Host.TempSectionContracts
{
    public enum CourseLearningActivityType
    {
        Discussion = 1,
        Assignment = 2,
        Quiz = 3,
        Assessment = 4,
        Custom = 5
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
        public string CustomAttribute { get; set; }
        public int ActiveDate { get; set; }
        public int InactiveDate { get; set; }
        public int DueDate { get; set; }
    }
}