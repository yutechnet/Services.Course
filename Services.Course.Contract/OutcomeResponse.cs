using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Common.WebApi;

namespace BpeProducts.Services.Course.Contract
{
    public class OutcomeResponse : ResponseBase
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
    }
}
