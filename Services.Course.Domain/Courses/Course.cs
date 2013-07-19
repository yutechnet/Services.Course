using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BpeProducts.Common.Exceptions;
using BpeProducts.Common.NHibernate;
using BpeProducts.Common.NHibernate.Version;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Events;
using Newtonsoft.Json;
using CourseSegment = BpeProducts.Services.Course.Domain.Courses.CourseSegment;

namespace BpeProducts.Services.Course.Domain.Entities
{
    public class Course : VersionableEntity, IHaveOutcomes
    {
        // move this to Entity in common project
        [NotPersisted]
        public virtual bool IsBuilt { get; internal protected set; }

        #region Properties

        private bool _isTemplate;
        private Course _template;
        private string _name;
        private string _code;
        private string _description;
        private ECourseType _courseType;
        private List<CourseSegment> _segments = new List<CourseSegment>();
        private IList<Program> _programs = new List<Program>();
        private IList<LearningOutcome> _outcomes = new List<LearningOutcome>();

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
        

        public virtual IList<Program> Programs
        {
            get { return _programs; }
            set
            {
                CheckPublished();
                _programs = value;
            }
        }

        public virtual List<CourseSegment> Segments
        {
            get { return _segments; }
            set
            {
                CheckPublished();
                _segments = value;
            }
        }

        public virtual IList<LearningOutcome> Outcomes
        {
            get { return _outcomes; }
            set
            {
                CheckPublished();
                _outcomes = value;
            }
        }
        
        #endregion

        public virtual void BuildIndex(IList<CourseSegment> segments, ref Dictionary<Guid, CourseSegment> index)
        {
            foreach (var courseSegment in segments)
            {
                index.Add(courseSegment.Id, courseSegment);
                if (courseSegment.ChildSegments.Count > 0) BuildIndex(courseSegment.ChildSegments, ref index);
            }
        }

        public virtual void SetPrograms(IList<Program> programs)
        {
            CheckPublished();

            Programs.Clear();
            Programs = programs;
        }

        public virtual CourseSegment AddSegment(Guid segmentId, Guid parentSegmentId, SaveCourseSegmentRequest request)
        {
            CourseSegment parentSegment = null;
            if (parentSegmentId != Guid.Empty)
            {
                parentSegment = _segments.FirstOrDefault(s => s.Id == parentSegmentId);

                if (parentSegment == null) 
                    throw new BadRequestException(string.Format("Cannot add segment to Course {0} with parent Segment SegmentId {1}. Parent segment SegmentId does not exists", Id, parentSegmentId));
            }

            var newSegment = new CourseSegment
            {
                Course = this,
                ParentSegment = parentSegment,
                Id = segmentId,
                Name = request.Name,
                Description = request.Description,
                Type = request.Type,
                Content = request.Content ?? new List<Content>()
            };

            if(parentSegment != null)
                parentSegment.AddSubSegment(newSegment);

            _segments.Add(newSegment);

            return newSegment;
        }

        public virtual CourseSegment UpdateSegment(Guid segmentId, SaveCourseSegmentRequest request)
        {
            var segment = _segments.FirstOrDefault(s => s.Id == segmentId);

            if (segment == null)
                throw new NotFoundException(string.Format("Segment {0} for Course {1} is not found.", segmentId, Id));

            segment.Name = request.Name;
            segment.Description = request.Description;
            segment.Type = request.Type;
            segment.Content = request.Content ?? new List<Content>();

            return segment;
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
