using System;
using System.Collections.Generic;
using AutoMapper;
using BpeProducts.Common.Authorization;
using BpeProducts.Common.Exceptions;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Authorization.Contract;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Contract.Events;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using NServiceBus;

namespace BpeProducts.Services.Course.Domain.ProgramAggregates
{
    public class ProgramService : IProgramService
    {
        private readonly IRepository _repository;
	    private readonly IBus _bus;

	    public ProgramService(IRepository repository,IBus bus)
        {
	        _repository = repository;
	        _bus = bus;
        }

	    [AuthByAcl(Capability = Capability.ViewProgram, ObjectId = "programId", ObjectType = typeof(Program))]
        public ProgramResponse Search(Guid programId)
        {
            var program = _repository.Get<Program>(programId);
            if (program == null || program.IsDeleted)
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
            criteria.Add(Restrictions.Eq("IsDeleted", false));
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
			_bus.Publish(new ProgramCreated
				{
					Id=program.Id,
					OrganizationId = program.OrganizationId,
					Type="program"
				});
            return Mapper.Map<ProgramResponse>(program);
        }

        [AuthByAcl(Capability = Capability.EditProgram, ObjectId = "programId", ObjectType = typeof(Program))]
        public void Update(Guid programId, UpdateProgramRequest request)
        {
            var program = _repository.Get<Program>(programId);
            if (program == null || program.IsDeleted)
            {
                throw new NotFoundException(string.Format("Program {0} not found.", programId));
            }

            _repository.Save(program);
			Mapper.Map(request, program);
            
			_bus.Publish(new ProgramCreated
			{
				Id = program.Id,
				OrganizationId = program.OrganizationId,
				Type = "program"
			});
        }

        [AuthByAcl(Capability = Capability.EditProgram, ObjectId = "programId", ObjectType = typeof(Program))]
        public void Delete(Guid programId)
        {
            var program = _repository.Get<Program>(programId);
            if (program == null || program.IsDeleted)
            {
                throw new NotFoundException(string.Format("Program {0} not found.", programId));
            }
            program.IsDeleted = true;
            _repository.Save(program);
        }
    }
}