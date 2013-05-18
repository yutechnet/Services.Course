using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BpeProducts.Services.Course.Contract
{
    public class OutcomeRequest
    {
        public string Description { get; set; }

        public int TenantId { get; set; }
    }
}
