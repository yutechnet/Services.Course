using System;

namespace BpeProducts.Services.Course.Contract
{
    public class LearningMaterialInfo
    {
        public Guid Id { get; set; }
        public string Instruction { get; set; }
        public bool IsRequired { get; set; }
        public Guid? CourseId { get; set; }
        public Guid? CourseSegmentId { get; set; }
        public Guid AssetId { get; set; }
        [Obsolete("this will be removed")]
        public string CustomAttribute { get; set; }
        public string MetaData { get; set; }
    }
}