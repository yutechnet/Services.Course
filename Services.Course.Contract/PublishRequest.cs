using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BpeProducts.Services.Course.Contract
{
    [DataContract]
    public class PublishRequest
    {
        [Required, DataMember(IsRequired = true)]
        public string PublishNote { get; set; }
    }
}
