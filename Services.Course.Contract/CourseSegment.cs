using System;
using System.Collections.Generic;

namespace BpeProducts.Services.Course.Contract
{
	public class CourseSegment
	{
		public CourseSegment() 
		{
			Segments = new List<CourseSegment>();
		}

		public String Name { get; set; }
		public String Description { get; set; }
		public String Type { get; set; }
		public List<CourseSegment> Segments { get; set; }
		public int TenantId { get; set; }
	}
}