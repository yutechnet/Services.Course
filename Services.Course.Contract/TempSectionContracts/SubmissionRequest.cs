using System;

namespace BpeProducts.Services.Course.Host.TempSectionContracts
{
    public class SubmissionRequest
    {
        public Guid UserId { get; set; }
        public Uri SubmissionUri { get; set; }
    }

    public class SubmissionResponse : SubmissionRequest
    {
    }
}
