using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using BpeProducts.Common.Exceptions;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Asset.Contracts;
using Services.Assessment.Contract;

namespace BpeProducts.Services.Course.Domain.Courses
{
    public class LearningMaterial : TenantEntity
    {
        public virtual Guid AssetId { get; set; }
        public virtual CourseSegment CourseSegment { get; set; }
        public virtual string Instruction { get; set; }
        public virtual bool IsRequired { get; set; }
        public virtual string CustomAttribute { get; set; }
        public virtual Guid SourceLearningMaterialId { get; set; }
        public virtual Course Course { get; set; }

        public virtual void CloneOutcomes(IAssessmentClient assessmentClient)
        {
            assessmentClient.CloneEntityOutcomes(SupportingEntityType.LearningMaterial, SourceLearningMaterialId, new CloneEntityOutcomeRequest()
            {
                EntityId = Id,
                Type = SupportingEntityType.LearningMaterial
            });
        }
    }
}