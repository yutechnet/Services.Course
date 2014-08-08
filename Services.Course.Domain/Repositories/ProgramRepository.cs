using System;
using System.Collections.Generic;
using System.Linq;
using BpeProducts.Common.Exceptions;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Course.Domain.ProgramAggregates;
using NHibernate;
using NHibernate.OData;

namespace BpeProducts.Services.Course.Domain.Repositories
{
    public class ProgramRepository : IProgramRepository
    {
        private readonly IRepository _repository;

        public ProgramRepository(IRepository repository)
        {
            _repository = repository;
        }

        public Program Get(Guid id)
        {
            return _repository.Get<Program>(id);
        }



        public IList<Program> Get(List<Guid> ids)
        {
            var programs = new List<Program>();
            foreach (var id in ids)
            {
                var program = _repository.Get<Program>(id);

                if(program == null)
                    throw new BadRequestException(string.Format("Program {0} does not exist", id));

                programs.Add(program);
            }

            return programs;
        }

        public void Save(Program program)
        {
            _repository.SaveOrUpdate(program);
        }

        public ICriteria ODataQuery(string queryString)
        {
            return _repository.ODataQuery<Program>(queryString);
        }

        public Program GetVersion(Guid originalEntityId, string versionNumber)
        {
            var version = (from c in _repository.Query<Program>()
                           where c.OriginalEntity.Id == originalEntityId && c.VersionNumber == versionNumber
                           select c).FirstOrDefault();
            return version;
        }


        public Program GetOrThrow(Guid programId)
        {
            var pargram = Get(programId);
            if (pargram == null || pargram.IsDeleted)
            {
                throw new NotFoundException(string.Format("pargram {0} not found.", programId));
            }

            return pargram;
        }
    }
}