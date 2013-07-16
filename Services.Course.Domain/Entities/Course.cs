﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BpeProducts.Common.NHibernate;
using BpeProducts.Common.NHibernate.Version;
using BpeProducts.Services.Course.Contract;
using Newtonsoft.Json;

namespace BpeProducts.Services.Course.Domain.Entities
{
    public class Course : VersionableEntity, IHaveOutcomes
    {
        private Dictionary<Guid, CourseSegment> _segmentIndex;
        public virtual Course Template { get; set; }
        [Required]
        public virtual string Name { get; set; }

        [Required]
        public virtual string Code { get; set; }

        public virtual string Description { get; set; }
        public virtual ECourseType CourseType { get; set; }
        public virtual bool IsTemplate { get; set; }
        public virtual IList<Program> Programs { get; set; }

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

        public virtual List<CourseSegment> Segments { get; set; }

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

        public Course()
        {
            Programs = new List<Program>();
            Segments = new List<CourseSegment>();
            Outcomes = new List<LearningOutcome>();
            _segmentIndex = new Dictionary<Guid, CourseSegment>();
        }

        public virtual IList<LearningOutcome> Outcomes { get; set; }

        public virtual void SetPrograms(IList<Program> programs)
        {
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
    }
}
