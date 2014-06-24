﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Common.Contract.Validation;

namespace BpeProducts.Services.Course.Contract
{
    public class UpdateLearningMaterialRequest
    {
        public string Instruction { get; set; }
        public bool IsRequired { get; set; }
        [DisallowEmptyGuid]
        public Guid AssetId { get; set; }
        public string CustomAttribute { get; set; }
    }
}
