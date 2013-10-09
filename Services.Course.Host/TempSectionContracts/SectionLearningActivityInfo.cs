using System;

namespace BpeProducts.Services.Course.Host.TempSectionContracts
{
    public class SectionLearningActivityInfo
    {
        public Guid Id { get; set; }
        public int TenantId { get; set; }
        public string Name { get; set; }
        public SectionLearningActivityType Type { get; set; }
        public Boolean IsGradeable { get; set; }
        public Boolean IsExtraCredit { get; set; }
        public int MaxPoint { get; set; }
        public int Weight { get; set; }
        public Guid ObjectId { get; set; }
        public string CustomAttribute { get; set; }

        public DateTime? ActiveDate { get; set; }
        public DateTime? InactiveDate { get; set; }
        public DateTime? DueDate { get; set; }
    }

    public enum SectionLearningActivityType
    {
        Discussion = 1,
        Assignment = 2,
        Quiz = 3,
        Assessment = 4,
        Custom = 5
    }
}