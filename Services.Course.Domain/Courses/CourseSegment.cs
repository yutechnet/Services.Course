using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Course.Contract;
using Newtonsoft.Json;

namespace BpeProducts.Services.Course.Domain.Courses
{
    public class CourseSegment : TenantEntity
    {
        public CourseSegment()
        {
            ChildSegments = new List<CourseSegment>();
            Content = new List<Content>();
        }

        public virtual Entities.Course Course { get; set; }

        public virtual List<CourseSegment> ChildSegments { get; set; }

        public virtual CourseSegment ParentSegment { get; set; }

        [Required]
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        [Required]
        public virtual string Type { get; set; }

        public virtual int DisplayOrder { get; set; }

        public virtual List<Content> Content { get; set; }

        public virtual string SerializedData
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
    }
}