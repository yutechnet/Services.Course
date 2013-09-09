using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BpeProducts.Services.Course.Contract
{
    [DataContract]
    public class UpdateCourseSegmentRequest
    {
        public UpdateCourseSegmentRequest()
        {
            ChildrenSegments = new List<UpdateCourseSegmentRequest>();
        }

        [DataMember(IsRequired = true)]
        public Guid Id { get; set; }
        [DataMember]
        public Guid? ParentSegmentId { get; set; }

        [DataMember(IsRequired = true)]
        public String Name { get; set; }
        [DataMember(IsRequired = true)]
        public String Description { get; set; }
        [DataMember(IsRequired = true)]
        public String Type { get; set; }

        [DataMember]
        public IList<UpdateCourseSegmentRequest> ChildrenSegments { get; set; }
    }
}