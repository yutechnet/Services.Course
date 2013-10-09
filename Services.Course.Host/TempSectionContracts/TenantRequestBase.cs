using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BpeProducts.Services.Course.Host.TempSectionContracts
{
    [DataContract]
    public abstract class TenantRequestBase
    {
        [Required]
        [DataMember(IsRequired = true)]
        public int? TenantId { get; set; }
    }
}
