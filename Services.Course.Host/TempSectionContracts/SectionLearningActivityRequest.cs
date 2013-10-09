using System;

namespace BpeProducts.Services.Course.Host.TempSectionContracts
{
    public class SectionLearningActivityRequest
    {
        public string Name { get; set; }
        public SectionLearningActivityType Type { get; set; }
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