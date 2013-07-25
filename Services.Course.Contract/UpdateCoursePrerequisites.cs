using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BpeProducts.Services.Course.Contract
{
	[DataContract]
	public class UpdateCoursePrerequisites
	{
		public UpdateCoursePrerequisites()
		{
			PrerequisiteIds = new List<Guid>();
		}

		[DataMember(IsRequired = true)]
		public List<Guid> PrerequisiteIds { get; set; }
	}
}