using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Course.Contract;
using Newtonsoft.Json;

namespace BpeProducts.Services.Course.Domain.Entities
{

    public class Course : TenantEntity
    {
        private Dictionary<Guid, CourseSegment> _segmentIndex;

        [Required]
        public virtual string Name { get; set; }
        [Required]
        public virtual string Code { get; set; }
        public virtual string Description { get; set; }
        public virtual IList<Program> Programs { get; set; }
        public virtual string CourseSegmentJson { 
            get
            {
                var settings = new JsonSerializerSettings
                    {
                        // ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        ReferenceLoopHandling = ReferenceLoopHandling.Serialize
                    };
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(Segments,settings);
                return json;
            } 
            set { Segments = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CourseSegment>>(value); }
        }

        public virtual List<CourseSegment> Segments { get; set; }
        public virtual Dictionary<Guid, CourseSegment> SegmentIndex { 
            get 
            { 
                if (_segmentIndex.Count == 0)  BuildIndex(Segments,ref _segmentIndex);
                
                return _segmentIndex;
            }
        }

        public virtual void BuildIndex(IList<CourseSegment> segments,ref Dictionary<Guid, CourseSegment> index )
        {
            foreach (var courseSegment in segments)
            {
                index.Add(courseSegment.Id,courseSegment);
                if (courseSegment.ChildrenSegments.Count > 0) BuildIndex(courseSegment.ChildrenSegments,ref index);
            }
        }

        public Course()
        {
            Programs = new List<Program>();
			Segments = new List<CourseSegment>();
            _segmentIndex = new Dictionary<Guid, CourseSegment>();
        }
       
    }
}
