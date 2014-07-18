using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using BpeProducts.Common.Exceptions;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Course.Contract;
using Castle.Core.Internal;
using Newtonsoft.Json;
using Services.Assessment.Contract;

namespace BpeProducts.Services.Course.Domain.CourseAggregates
{
    public class CourseSegment : TenantEntity
    {
        private IList<LearningMaterial> _learningMaterials = new List<LearningMaterial>();
        private IList<CourseSegment> _childSegments = new List<CourseSegment>();
        private CourseSegment _parentSegment;

        private IList<CourseLearningActivity> _courseLearningActivities = new List<CourseLearningActivity>();

        [Required]
        public virtual string Name { get; set; }

        [Required]
        public virtual string Type { get; set; }

        public virtual string Description { get; set; }

        public virtual int DisplayOrder { get; set; }

        public virtual int? ActiveDate { get; set; }
        public virtual int? InactiveDate { get; set; }

        public virtual Guid SourceCourseSegmentId { get; set; }

        [NotNullable]
        public virtual Course Course { get; set; }

        public virtual CourseSegment ParentSegment
        {
            get { return _parentSegment; }
            set { _parentSegment = value; }
        }

        public virtual IList<CourseLearningActivity> CourseLearningActivities
        {
            get { return _courseLearningActivities; }
            protected internal set { _courseLearningActivities = value; }
        }

        public virtual IList<LearningMaterial> LearningMaterials
        {
            get { return _learningMaterials; }
            set { _learningMaterials = value; }
        }

        public virtual IList<CourseSegment> ChildSegments
        {
            get { return _childSegments; }
            protected internal set { _childSegments = value; }
        }

        protected internal virtual string SerializedData
        {
            get
            {
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Serialize
                };

                var toSerialize = new
                {
                    Name,
                    Description,
                    Type,
                    DisplayOrder,
                };

                var json = JsonConvert.SerializeObject(toSerialize, settings);
                return json;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value)) return;

                var serializedData = new
                {
                    Name,
                    Description,
                    Type,
                    DisplayOrder,
                };

                serializedData = JsonConvert.DeserializeAnonymousType(value, serializedData);

                Name = serializedData.Name;
                Description = serializedData.Description;
                Type = serializedData.Type;
                DisplayOrder = serializedData.DisplayOrder;
            }
        }

        public virtual void AddSubSegment(CourseSegment segment)
        {
            segment.ParentSegment = this;
            ChildSegments.Add(segment);
        }

        public virtual void Delete()
        {
            IsDeleted = true;

            foreach (var childSegment in _childSegments)
            {
                childSegment.Delete();
            }
        }

        public virtual CourseLearningActivity GetLearningActivityOrThrow(Guid learningActivityId)
        {
            var learningActivity = _courseLearningActivities.SingleOrDefault(l => l.Id == learningActivityId);

            if (learningActivity == null)
                throw new NotFoundException(string.Format("Learning Activity {0} for Course {1} is not found.", learningActivityId, Id));

            return learningActivity;
        }

        public virtual LearningMaterial AddLearningMaterial(LearningMaterialRequest learningMaterialRequest)
        {
            var learningMaterial = Mapper.Map<LearningMaterial>(learningMaterialRequest);
            learningMaterial.TenantId = TenantId;
            learningMaterial.Id = Guid.NewGuid();
            learningMaterial.CourseSegment = this;
            learningMaterial.Course = Course; 
            _learningMaterials.Add(learningMaterial);
            return learningMaterial;
        }
        public virtual void UpdateLearningMaterial(Guid learningMaterialId, UpdateLearningMaterialRequest updatelearningMaterialRequest)
        {
            var learningMaterial = GetLearningMaterialOrThrow(learningMaterialId);
            Mapper.Map(updatelearningMaterialRequest, learningMaterial);
        }

        public virtual void DeleteLearningMaterial(Guid learningMaterialId)
        {
            var learningMaterial = GetLearningMaterialOrThrow(learningMaterialId);
            learningMaterial.IsDeleted = true;
        }

        private LearningMaterial GetLearningMaterialOrThrow(Guid learningMaterialId)
        {
            var learningMaterial = _learningMaterials.SingleOrDefault(l => l.Id == learningMaterialId);

            if (learningMaterial == null)
                throw new NotFoundException(string.Format("Learning Material {0} for Course Learning Material {1} is not found.", learningMaterialId, Id));

            return learningMaterial;
        }
        
        public virtual void CloneOutcomes(IAssessmentClient assessmentClient)
        {
            assessmentClient.CloneEntityOutcomes(SupportingEntityType.Segment, SourceCourseSegmentId, new CloneEntityOutcomeRequest()
            {
                EntityId = Id,
                Type = SupportingEntityType.Segment
            });
            CourseLearningActivities.ForEach(learningActivity => learningActivity.CloneOutcomes(assessmentClient));
            LearningMaterials.ForEach(learningMaterial => learningMaterial.CloneOutcomes(assessmentClient));
            ChildSegments.ForEach(childSegment => childSegment.CloneOutcomes(assessmentClient));
        }
    }
}
