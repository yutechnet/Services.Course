using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BpeProducts.Services.Course.Contract
{
    public class CoursePublishRequest
    {
        public string PublishNote { get; set; }
        public string VersionNumber { get; set; }
    }
}
