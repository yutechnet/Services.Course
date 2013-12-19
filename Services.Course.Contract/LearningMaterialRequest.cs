using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BpeProducts.Services.Course.Contract
{
    public class LearningMaterialRequest
    {
        public Guid AssetId { get; set; }
        public string Instruction { get; set; }
        public bool IsRequired { get; set; }
    }
}
