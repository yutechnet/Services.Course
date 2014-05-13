using System;
using System.Collections.Generic;
using AutoMapper;
using BpeProducts.Common.Authorization;
using BpeProducts.Common.Exceptions;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Authorization.Contract;
using BpeProducts.Services.Course.Contract;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace BpeProducts.Services.Course.Domain.ProgramAggregates
{
    public class ProgramService : IProgramService
    {
        private readonly IRepository _repository;

        public ProgramService(IRepository repository)
        {
            _repository = repository;
        }

        [AuthByAcl(Capability = Capability.ViewProgram, ObjectId = "programId", ObjectType = typeof(Program))]
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
            criteria.SetResultTransformer(Transformers.DistinctRootEntity);
            var programs = criteria.List<Program>();//Distinct();
            var programResponses = new List<ProgramResponse>();
            Mapper.Map(programs, programResponses);
            return programResponses;
        }

        [AuthByAcl(Capability = Capability.EditProgram, OrganizationObject = "request")]
        public ProgramResponse Create(SaveProgramRequest request)
        {
            var program = Mapper.Map<Program>(request);
            _repository.Save(program);
            return Mapper.Map<ProgramResponse>(program);
        }

        [AuthByAcl(Capability = Capability.EditProgram, ObjectId = "programId", ObjectType = typeof(Program))]
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

        [AuthByAcl(Capability = Capability.EditProgram, ObjectId = "programId", ObjectType = typeof(Program))]
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