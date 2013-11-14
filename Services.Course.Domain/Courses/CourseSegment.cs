using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BpeProducts.Common.Exceptions;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Entities;
using Newtonsoft.Json;

namespace BpeProducts.Services.Course.Domain.Courses
{
    public class CourseSegment : TenantEntity, ISupportingEntity
    {
        private IList<LearningOutcome> _supportedOutcomes = new List<LearningOutcome>();
        private IList<CourseSegment> _childSegments = new List<CourseSegment>();
        private CourseSegment _parentSegment;

        private IList<CourseLearningActivity> _courseLearningActivities = new List<CourseLearningActivity>();

        [Required]
        public virtual string Name { get; set; }

        [Required]
        public virtual string Type { get; set; }

        public virtual string Description { get; set; }

        public virtual int DisplayOrder { get; set; }

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


        public virtual IList<LearningOutcome> SupportedOutcomes
        {
            get { return _supportedOutcomes; }
            protected internal set { _supportedOutcomes = value; }
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

        public virtual void SupportOutcome(LearningOutcome outcome)
        {
            _supportedOutcomes.Add(outcome);
            outcome.SupportingEntities.Add(this);
        }

        public virtual void UnsupportOutcome(LearningOutcome outcome)
        {
            _supportedOutcomes.Remove(outcome);
            outcome.SupportingEntities.Remove(this);
        }

        public virtual void Delete()
        {
            ActiveFlag = false;

            foreach (var childSegment in _childSegments)
            {
                childSegment.Delete();
            }
        }

        public virtual CourseLearningActivity GetLearningActivityOrThrow(Guid learningActivityId)
        {
            var learningActivity = _courseLearningActivities.FirstOrDefault();
            
            if (learningActivity == null)
                throw new NotFoundException(string.Format("Learning Activity {0} for Course {1} is not found.", learningActivityId, Id));

            return learningActivity;
        }

        public virtual LearningMaterial AddLearningMaterial(Guid learningActivityId, LearningMaterialRequest request)
        {
            var learningActivity = GetLearningActivityOrThrow(learningActivityId);
            return learningActivity.AddLearningMaterial(request);
        }
    }
}
