using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Services.Course.Contract;

namespace BpeProducts.Services.Course.Domain
{
    public interface IProgramService
    {
        ProgramResponse Search(Guid programId);
        IEnumerable<ProgramResponse> Search(string queryString);

        ProgramResponse Create(SaveProgramRequest request);
        void Update(Guid programId, SaveProgramRequest request);
        void Delete(Guid programId);
    }
}
