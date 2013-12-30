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
        public virtual Guid SourceLearningMaterialId { get; set; }


        public virtual void PublishAsset(IAssetServiceClient assetServiceClient)
        {
            if (!CheckAssetIsPublished(assetServiceClient, AssetId))
                assetServiceClient.PublishAsset(AssetId, string.Empty);
        }
        private bool CheckAssetIsPublished(IAssetServiceClient assetServiceClient, Guid assetId)
        {
            var asset = assetServiceClient.GetAsset(assetId);
            return asset.IsPublished;
        }


        public virtual void CloneLearningMaterialOutcomes(IAssessmentClient assessmentClient)
        {
            var supportingOutcomes = GetSupportingOutcomes(assessmentClient,SourceLearningMaterialId);
            if (supportingOutcomes != null)
                supportingOutcomes.ForEach(supportingOutcome => assessmentClient.SupportsOutcome("learningmaterial", Id, supportingOutcome.Id));
        }

        public virtual List<Guid> GetOutcomes(IAssessmentClient assessmentClient)
        {
            var supportingOutcomes = GetSupportingOutcomes(assessmentClient,Id);
            var outcomeIds = new List<Guid>();
            if (supportingOutcomes != null)
                supportingOutcomes.ForEach(supportingOutcome => outcomeIds.Add(supportingOutcome.Id));
            return outcomeIds;
        }

        private List<OutcomeInfo> GetSupportingOutcomes(IAssessmentClient assessmentClient,Guid supportingEntityId)
        {
            var supportingOutcomes = new List<OutcomeInfo>();
            //when there is no learning material outcomes,it will throw exception
            try
            {
                supportingOutcomes = assessmentClient.GetSupportingOutcomes(supportingEntityId, "learningmaterial");
            }
            catch (InternalServerErrorException)
            {
            }
            return supportingOutcomes;
        }
    }
}