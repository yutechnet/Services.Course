using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Entities;
using Newtonsoft.Json;

namespace BpeProducts.Services.Course.Domain.Courses
{
    public class CourseSegment : TenantEntity, IHaveOutcomes
    {
        private IList<LearningOutcome> _supportingOutcomes = new List<LearningOutcome>();
        private IList<Content> _content = new List<Content>();
        private IList<CourseSegment> _childSegments = new List<CourseSegment>();
        private CourseSegment _parentSegment;

        [Required]
        public virtual string Name { get; set; }

        [Required]
        public virtual string Type { get; set; }

        public virtual string Description { get; set; }

        public virtual int DisplayOrder { get; set; }

        public virtual Course Course { get; set; }

        public virtual CourseSegment ParentSegment
        {
            get { return _parentSegment; }
            set { _parentSegment = value; }
        }

        public virtual IList<Content> Content
        {
            get { return _content; }
            protected internal set { _content = value; }
        }

        public virtual IList<LearningOutcome> SupportingOutcomes
        {
            get { return _supportingOutcomes; }
            protected internal set { _supportingOutcomes = value; }
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
                        Content
                    };

                var json = JsonConvert.SerializeObject(toSerialize, settings);
                return json;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    var serializedData = new
                    {
                        Name,
                        Description,
                        Type,
                        DisplayOrder,
                        Content
                    };

                    serializedData = JsonConvert.DeserializeAnonymousType(value, serializedData);

                    Name = serializedData.Name;
                    Description = serializedData.Description;
                    Type = serializedData.Type;
                    DisplayOrder = serializedData.DisplayOrder;
                    Content = serializedData.Content;
                }
            }
        }

        public virtual void AddSubSegment(CourseSegment segment)
        {
            segment.ParentSegment = this;
            ChildSegments.Add(segment);
        }

        public virtual LearningOutcome SupportOutcome(LearningOutcome outcome)
        {
            SupportingOutcomes.Add(outcome);

            return outcome;
        }
    }
}