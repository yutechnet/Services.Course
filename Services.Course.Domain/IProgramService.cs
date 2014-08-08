using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Common.Contract;
using BpeProducts.Services.Course.Contract;

namespace BpeProducts.Services.Course.Domain
{
    public interface IProgramService
    {
        ProgramResponse Get(Guid programId);
        IEnumerable<ProgramResponse> Search(string queryString);

        ProgramResponse Create(SaveProgramRequest request);
        void Update(Guid programId, UpdateProgramRequest request);
        void Delete(Guid programId);
        ProgramResponse CreateVersion(Guid parentVersionId, string versionNumber);
        void PublishVersion(Guid courseId, string publishNote);
        void UpdateActiviationStatus(Guid courseId, ActivationRequest request);
    }
}
