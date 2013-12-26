using System;
using System.ComponentModel.DataAnnotations.Schema;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Asset.Contracts;

namespace BpeProducts.Services.Course.Domain.Courses
{
    public class LearningMaterial : TenantEntity
    {
        public virtual Guid AssetId { get; set; }
        public virtual CourseSegment CourseSegment { get; set; }
        public virtual string Instruction { get; set; }
        public virtual bool IsRequired { get; set; }
       

        public virtual void PublishAsset(IAssetServiceClient assetServiceClient)
        {
            if (!CheckAssetIsPublished(assetServiceClient,AssetId))
                assetServiceClient.PublishAsset(AssetId, string.Empty);
        }
        private bool CheckAssetIsPublished(IAssetServiceClient assetServiceClient,Guid assetId)
        {
            var asset = assetServiceClient.GetAsset(assetId);
            return asset.IsPublished;
        }

    }
}