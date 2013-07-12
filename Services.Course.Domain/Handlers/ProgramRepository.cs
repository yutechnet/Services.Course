using System;
using System.Collections.Generic;
using BpeProducts.Common.Exceptions;
using BpeProducts.Services.Course.Domain.Entities;
using NHibernate;
using NHibernate.OData;

namespace BpeProducts.Services.Course.Domain.Handlers
{
    public class ProgramRepository : IProgramRepository
    {
        private readonly ISession _session;

        public ProgramRepository(ISession session)
        {
            _session = session;
        }

        public Program Get(Guid id)
        {
            return _session.Get<Program>(id);
        }

        public IList<Program> Get(List<Guid> ids)
        {
            var programs = new List<Program>();
            foreach (var id in ids)
            {
                var program = _session.Get<Program>(id);

                if(program == null)
                    throw new BadRequestException(string.Format("Program {0} does not exist", id));

                programs.Add(program);
            }

            return programs;
        }

        public void Save(Program program)
        {
            _session.SaveOrUpdate(program);
        }

        public ICriteria ODataQuery(string queryString)
        {
            return _session.ODataQuery<Entities.Program>(queryString);
        }
    }
}