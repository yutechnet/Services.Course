﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BpeProducts.Services.Course.Contract
{
    public enum ECourseType
    {
        Traditional,
        Competency
    }

    public class CourseInfoResponse
    {
        public Guid Id { get; set; }
        public Guid? TemplateCourseId { get; set; }

        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public ECourseType CourseType { get; set; }
        public bool IsTemplate { get; set; }
        public List<Guid> ProgramIds { get; set; } 
        public DateTime DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }
        public Guid AddedBy { get; set; }
        public Guid UpdatedBy { get; set; }
		public List<CourseSegmentInfo> Segments { get; set; }
        public List<LearningMaterialInfo> LearningMaterials { get; set; }

        public string VersionNumber { get; set; }
        public bool IsPublished { get; set; }
        public string PublishNote { get; set; }
        public DateTime? PublishDate { get; set; }
        public bool IsActivated { get; set; }

        public Guid OrganizationId { get; set; }
		public List<Guid> PrerequisiteCourseIds { get; set; }
        public decimal Credit { get; set; }
        public string MetaData { get; set; }
        public List<Guid> ExtensionAssets { get; set; }
        public string CorrelationId { get; set; }
    }
}
