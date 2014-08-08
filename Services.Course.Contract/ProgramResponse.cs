using System;
using System.Collections.Generic;

namespace BpeProducts.Services.Course.Contract
{
	public class ProgramResponse
	{
        public Guid OrganizationId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public Guid Id { get; set; }
        public string ProgramType { get; set; }
        public string GraduationRequirements { get; set; }
        public bool IsPublished { get; set; }
        public string VersionNumber { get; set; }
        public string PublishNote { get; set; }
        public bool IsActivated { get; set; }

        public List<SimpleCourseInfoResponse> Courses { get; set; } 
	}
}