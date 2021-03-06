﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Common.Contract.Validation;

namespace BpeProducts.Services.Course.Contract
{
    [DataContract]
    public class LearningMaterialRequest
    {
        [DataMember(IsRequired = true)]
        [DisallowEmptyGuid]
        public Guid AssetId { get; set; }
        [DataMember]
        public string Instruction { get; set; }
        [DataMember]
        public bool IsRequired { get; set; }
        [DataMember]
        [Obsolete("this will be removed")]
        public string CustomAttribute { get; set; }

        [DataMember]
        public string MetaData { get; set; }
    }
}
