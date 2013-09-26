using System;
using System.Collections.Generic;
using AutoMapper;
using BpeProducts.Common.Exceptions;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Handlers;
using BpeProducts.Services.Course.Domain.Repositories;
using NHibernate;
using NHibernate.Criterion;

namespace BpeProducts.Services.Course.Domain
{
    public class ProgramService : IProgramService
    {
        private readonly IRepository _repository;

        public ProgramService(IRepository repository)
        {
            _repository = repository;
        }

        public ProgramResponse Search(Guid programId)
        {
            var program = _repository.Get<Program>(programId);
            if (program == null || !program.ActiveFlag)
            {
                throw new NotFoundException(string.Format("Program {0} not found.", programId));
            }
            return Mapper.Map<ProgramResponse>(program);
        }

        public IEnumerable<ProgramResponse> Search(string queryString)
        {
            var queryArray = queryString.Split('?');
            ICriteria criteria =
                _repository.ODataQuery<Program>(queryArray.Length > 1 ? queryArray[1] : "");
            criteria.Add(Restrictions.Eq("ActiveFlag", true));
            criteria.SetFetchMode("Courses", FetchMode.Join);  //for eager loading
            var programs = criteria.List<Program>();
            var programResponses = new List<ProgramResponse>();
            Mapper.Map(programs, programResponses);
            return programResponses;
        }

        public ProgramResponse Create(SaveProgramRequest request)
        {
            var program = Mapper.Map<Program>(request);
            _repository.Save(program);
            return Mapper.Map<ProgramResponse>(program);
        }

        public void Update(Guid programId, UpdateProgramRequest request)
        {
            var program = _repository.Get<Program>(programId);
            if (program == null || !program.ActiveFlag)
            {
                throw new NotFoundException(string.Format("Program {0} not found.", programId));
            }

            Mapper.Map(request, program);
            _repository.Save(program);
        }

        public void Delete(Guid programId)
        {
            var program = _repository.Get<Program>(programId);
            if (program == null || !program.ActiveFlag)
            {
                throw new NotFoundException(string.Format("Program {0} not found.", programId));
            }
            program.ActiveFlag = false;
            _repository.Save(program);
        }
    }
}