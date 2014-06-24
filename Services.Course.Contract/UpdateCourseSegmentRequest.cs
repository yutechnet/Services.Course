using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using BpeProducts.Common.Contract.Validation;

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
        [DisallowEmptyGuid]
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