using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BpeProducts.Common.Exceptions;
using BpeProducts.Common.NHibernate;
using BpeProducts.Common.NHibernate.Version;
using BpeProducts.Services.Course.Contract;
using Newtonsoft.Json;

namespace BpeProducts.Services.Course.Domain.Entities
{
    public class Course : VersionableEntity, IHaveOutcomes
    {
        // move this to Entity in common project
        [NotPersisted]
        public virtual bool IsBuilt { get; internal protected set; }

        #region Properties

        private Dictionary<Guid, CourseSegment> _segmentIndex;
        private bool _isTemplate;
        private Course _template;
        private string _name;
        private string _code;
        private string _description;
        private ECourseType _courseType;

        public virtual Course Template
        {
            get { return _template; }
            set
            {
                CheckPublished();
                _template = value;
            }
        }

        [Required]
        public virtual string Name
        {
            get { return _name; }
            set
            {
                CheckPublished();
                _name = value;
            }
        }

        [Required]
        public virtual string Code
        {
            get { return _code; }
            set
            {
                CheckPublished();
                _code = value;
            }
        }

        public virtual string Description
        {
            get { return _description; }
            set
            {
                CheckPublished();
                _description = value;
            }
        }

        public virtual ECourseType CourseType
        {
            get { return _courseType; }
            set
            {
                CheckPublished();
                _courseType = value;
            }
        }

        public virtual bool IsTemplate
        {
            get { return _isTemplate; }
            set
            {
                CheckPublished();
                _isTemplate = value;
            }
        }
        #endregion

        public virtual IList<Program> Programs { get; set; }

        public virtual List<CourseSegment> Segments { get; set; }

        public virtual IList<LearningOutcome> Outcomes { get; set; }

		// TODO: This is ignored right now. Need to work on mapping table
		public virtual List<Course> PrerequisiteCourses { get; set; }

        public Course()
        {
            Programs = new List<Program>();
            Segments = new List<CourseSegment>();
            Outcomes = new List<LearningOutcome>();
            _segmentIndex = new Dictionary<Guid, CourseSegment>();
        }

        public virtual string CourseSegmentJson
        {
            get
            {
                var settings = new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Serialize
                    };
                var json = JsonConvert.SerializeObject(Segments, settings);
                return json;
            }
            set
            {
                Segments = String.IsNullOrEmpty(value)
                               ? new List<CourseSegment>()
                               : JsonConvert.DeserializeObject<List<CourseSegment>>(value);
            }
        }

        public virtual Dictionary<Guid, CourseSegment> SegmentIndex
        {
            get
            {
                if (_segmentIndex.Count == 0) BuildIndex(Segments, ref _segmentIndex);

                return _segmentIndex;
            }
        }

        public virtual void BuildIndex(IList<CourseSegment> segments, ref Dictionary<Guid, CourseSegment> index)
        {
            foreach (var courseSegment in segments)
            {
                index.Add(courseSegment.Id, courseSegment);
                if (courseSegment.ChildrenSegments.Count > 0) BuildIndex(courseSegment.ChildrenSegments, ref index);
            }
        }

        public virtual void SetPrograms(IList<Program> programs)
        {
            CheckPublished();

            Programs.Clear();
            Programs = programs;
        }

        protected override VersionableEntity Clone()
        {
            return new Course
                {
                    Id = Guid.NewGuid(),
                    Name = this.Name,
                    Code = this.Code,
                    Description = this.Description,

                    Programs = new List<Program>(this.Programs),
                    Segments = new List<CourseSegment>(this.Segments),
                    Outcomes = new List<LearningOutcome>(this.Outcomes),

                    CourseSegmentJson = this.CourseSegmentJson,
                    TenantId = this.TenantId,
                    OrganizationId = this.OrganizationId,

                    Template = this.Template,
                    ActiveFlag = true
                };
        }

        private void CheckPublished()
        {
            if (IsBuilt && IsPublished)
            {
                throw new ForbiddenException(string.Format("Course {0} is published and cannot be modified.", Id));
            }
        }
    }
}
