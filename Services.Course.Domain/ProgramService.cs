using System;
using System.Collections.Generic;
using AutoMapper;
using BpeProducts.Common.Exceptions;
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
        private readonly IProgramRepository _programRepository;

        public ProgramService(IProgramRepository programRepository)
        {
            _programRepository = programRepository;
        }

        public ProgramResponse Search(Guid programId)
        {
            var program = _programRepository.Get(programId);
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
                _programRepository.ODataQuery(queryArray.Length > 1 ? queryArray[1] : "");
            criteria.Add(Restrictions.Eq("ActiveFlag", true));
            var programs = criteria.List<Domain.Entities.Program>();
            var programResponses = new List<ProgramResponse>();
            Mapper.Map(programs, programResponses);
            return programResponses;
        }

        public ProgramResponse Create(SaveProgramRequest request)
        {
            var program = Mapper.Map<Program>(request);
            _programRepository.Save(program);
            return Mapper.Map<ProgramResponse>(program);
        }

        public void Update(Guid programId, SaveProgramRequest request)
        {
            var program = _programRepository.Get(programId);
            if (program == null || !program.ActiveFlag)
            {
                throw new NotFoundException(string.Format("Program {0} not found.", programId));
            }
            Mapper.Map(request, program);
            _programRepository.Save(program);
        }

        public void Delete(Guid programId)
        {
            var program = _programRepository.Get(programId);
            if (program == null || !program.ActiveFlag)
            {
                throw new NotFoundException(string.Format("Program {0} not found.", programId));
            }
            program.ActiveFlag = false;
            _programRepository.Save(program);
        }
    }
}