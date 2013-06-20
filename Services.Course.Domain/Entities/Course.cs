using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BpeProducts.Common.NHibernate;
using BpeProducts.Common.NHibernate.Version;
using BpeProducts.Services.Course.Contract;
using Newtonsoft.Json;

namespace BpeProducts.Services.Course.Domain.Entities
{
    public class Course : TenantEntity, IVersionable, IHaveOutcomes
    {
        private Dictionary<Guid, CourseSegment> _segmentIndex;

        [Required]
        public virtual string Name { get; set; }
        [Required]
        public virtual string Code { get; set; }
        public virtual string Description { get; set; }
        public virtual IList<Program> Programs { get; set; }
        public virtual string CourseSegmentJson { 
            get
            {
                var settings = new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Serialize
                    };
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(Segments,settings);
                return json;
            }
            set {
                Segments = String.IsNullOrEmpty(value) ? new List<CourseSegment>():
                    Newtonsoft.Json.JsonConvert.DeserializeObject<List<CourseSegment>>(value) ;
            }
        }

        public virtual List<CourseSegment> Segments { get; set; }
        public virtual Dictionary<Guid, CourseSegment> SegmentIndex { 
            get 
            { 
                if (_segmentIndex.Count == 0)  BuildIndex(Segments,ref _segmentIndex);
                
                return _segmentIndex;
            }
        }

        public virtual void BuildIndex(IList<CourseSegment> segments,ref Dictionary<Guid, CourseSegment> index )
        {
            foreach (var courseSegment in segments)
            {
                index.Add(courseSegment.Id,courseSegment);
                if (courseSegment.ChildrenSegments.Count > 0) BuildIndex(courseSegment.ChildrenSegments,ref index);
            }
        }

        public Course()
        {
            Programs = new List<Program>();
			Segments = new List<CourseSegment>();
			Outcomes = new List<LearningOutcome>();
            _segmentIndex = new Dictionary<Guid, CourseSegment>();
        }

        public Course(Course course)
        {
            Name = course.Name;
            Code = course.Code;
            Description = course.Description;

            Programs = new List<Program>(course.Programs);
            Segments = new List<CourseSegment>(course.Segments);
            Outcomes = new List<LearningOutcome>(course.Outcomes);

            _segmentIndex = new Dictionary<Guid, CourseSegment>();
            CourseSegmentJson = course.CourseSegmentJson;
            TenantId = course.TenantId;
        }

	    public virtual IList<LearningOutcome> Outcomes { get; set; }

        public virtual Guid OriginalEntityId { get; set; }
        public virtual Guid? ParentEntityId { get; set; }
        public virtual string VersionNumber { get; set; }
        public virtual string PublishNote { get; set; }
        public virtual bool IsPublished { get; set; }
    }
}
