﻿using System;
using System.Collections.Generic;

namespace BpeProducts.Services.Course.Contract
{
	public class SaveCourseSegmentRequest
	{
		public String Name { get; set; }
		public String Description { get; set; }
		public String Type { get; set; }
		public int TenantId { get; set; }
	}
}