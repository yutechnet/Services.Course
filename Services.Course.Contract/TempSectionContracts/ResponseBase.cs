using System;

namespace BpeProducts.Services.Course.Host.TempSectionContracts
{
    public abstract class ResponseBase
    {
        public Guid Id { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateUpdated { get; set; }

        public Guid AddedBy { get; set; }

        public Guid UpdatedBy { get; set; }
    }
}