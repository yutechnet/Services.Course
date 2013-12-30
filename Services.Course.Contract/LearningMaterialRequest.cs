using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BpeProducts.Services.Course.Contract
{
    [DataContract]
    public class LearningMaterialRequest
    {
        [DataMember(IsRequired = true)]
        public Guid AssetId { get; set; }
        [DataMember]
        public string Instruction { get; set; }
        [DataMember]
        public bool IsRequired { get; set; }
    }
}
