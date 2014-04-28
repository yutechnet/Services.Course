using System;
using System.Collections.Generic;
using BpeProducts.Services.Course.Domain.ProgramAggregates;
using NHibernate;

namespace BpeProducts.Services.Course.Domain.Repositories
{
    public interface IProgramRepository
    {
        Program Get(Guid id);
        IList<Program> Get(List<Guid> ids);
        void Save(Program program);
        ICriteria ODataQuery(string queryString);
    }
}