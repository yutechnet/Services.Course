using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BpeProducts.Common.Exceptions;
using BpeProducts.Common.NHibernate.Version;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Entities;

namespace BpeProducts.Services.Course.Domain.Courses
{
    public class Course : VersionableEntity, IHaveOutcomes
    {
        #region Properties

        private string _name;
        private string _code;
        private string _description;
        private ECourseType _courseType;
        private IList<CourseSegment> _segments = new List<CourseSegment>();
        private IList<Program> _programs = new List<Program>();
        private IList<LearningOutcome> _supportingOutcomes = new List<LearningOutcome>();
        private IList<Course> _prerequisites = new List<Course>();

        public virtual Course Template { get; protected internal set; }
        public virtual bool IsTemplate { get; protected internal set; }

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

        public virtual IList<Program> Programs
        {
            get { return _programs; }
            protected internal set { _programs = value; }
        }

        public virtual IList<CourseSegment> Segments
        {
            get { return _segments; }
            protected internal set { _segments = value; }
        }

        public virtual IList<LearningOutcome> SupportingOutcomes
        {
            get { return _supportingOutcomes; }
            protected internal set { _supportingOutcomes = value; }
        }

        public virtual IList<Course> Prerequisites
        {
            get { return _prerequisites; }
            protected internal set { _prerequisites = value; }
        }

        #endregion

        public virtual LearningOutcome SupportOutcome(LearningOutcome outcome)
        {
            CheckPublished();

            _supportingOutcomes.Add(outcome);
            return outcome;
        }

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

        public virtual void SetPrerequisites(IList<Course> prerequisites)
        {
            CheckPublished();

            Prerequisites.Clear();
            Prerequisites = prerequisites;
        }

        public virtual CourseSegment AddSegment(Guid segmentId, Guid parentSegmentId, SaveCourseSegmentRequest request)
        {
            CheckPublished();

            CourseSegment parentSegment = null;
            if (parentSegmentId != Guid.Empty)
            {
                parentSegment = _segments.FirstOrDefault(s => s.Id == parentSegmentId);

                if (parentSegment == null)
                    throw new BadRequestException(
                        string.Format(
                            "Cannot add segment to Course {0} with parent Segment SegmentId {1}. Parent segment SegmentId does not exists",
                            Id, parentSegmentId));
            }

            var newSegment = new CourseSegment
                {
                    Course = this,
                    Id = segmentId,
                    Name = request.Name,
                    Description = request.Description,
                    Type = request.Type,
                    Content = request.Content ?? new List<Content>(),
                    TenantId = TenantId
                };

            if (parentSegment != null)
            {
                parentSegment.AddSubSegment(newSegment);
            }

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
                    SupportingOutcomes = new List<LearningOutcome>(this.SupportingOutcomes),
                    Prerequisites = new List<Course>(this.Prerequisites),

                    TenantId = this.TenantId,
                    OrganizationId = this.OrganizationId,

                    Template = this.Template,
                    ActiveFlag = true
                };
        }
    }
}
