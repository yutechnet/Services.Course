using System;
using System.Collections.Generic;

namespace BpeProducts.Services.Course.Contract
{
	public class SaveCourseSegmentRequest
	{
        public SaveCourseSegmentRequest()
        {
            ChildrenSegments = new List<SaveCourseSegmentRequest>();
        }

        public Guid? ParentSegmentId { get; set; }

		public String Name { get; set; }
		public String Description { get; set; }
		public String Type { get; set; }
	    public long DiscussionId { get; set; }
        public int TenantId { get; set; }

        public int DisplayOrder { get; set; }

        public IList<SaveCourseSegmentRequest> ChildrenSegments { get; set; }
	}
}
