using System;
using System.Collections.Generic;
using AutoMapper;
using BpeProducts.Common.Authorization;
using BpeProducts.Common.Exceptions;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Authorization.Contract;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Contract.Events;
using BpeProducts.Services.Course.Domain.Repositories;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using NServiceBus;

namespace BpeProducts.Services.Course.Domain.ProgramAggregates
{
    public class ProgramService : IProgramService
    {
        private readonly IProgramRepository _programrepository;
	    private readonly IBus _bus;

        public ProgramService(IProgramRepository repository, IBus bus)
        {
	        _programrepository = repository;
	        _bus = bus;
        }

	    [AuthByAcl(Capability = Capability.ViewProgram, ObjectId = "programId", ObjectType = typeof(Program))]
        public ProgramResponse Get(Guid programId)
        {
            var program = _programrepository.GetOrThrow(programId);
            return Mapper.Map<ProgramResponse>(program);
        }

		[AuthCollectionByAcl(Capability=Capability.ViewProgram,Type = "program")]
        public IEnumerable<ProgramResponse> Search(string queryString)
        {
            var queryArray = queryString.Split('?');
            ICriteria criteria =
                _programrepository.ODataQuery(queryArray.Length > 1 ? queryArray[1] : "");
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
            program.Id = Guid.NewGuid();
            _programrepository.Save(program);
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
            var program = _programrepository.Get(programId);
            if (program == null || program.IsDeleted)
            {
                throw new NotFoundException(string.Format("Program {0} not found.", programId));
            }
            program.Update(request);
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
            var program = _programrepository.Get(programId);
            if (program == null || program.IsDeleted)
            {
                throw new NotFoundException(string.Format("Program {0} not found.", programId));
            }
            program.Delete();
        }

        [AuthByAcl(Capability = Capability.EditProgram, ObjectId = "parentProgramId", ObjectType = typeof(Program))]
        public ProgramResponse CreateVersion(Guid parentProgramId, string versionNumber)
        {
            var parentVersion = _programrepository.Get(parentProgramId);

            if (parentVersion == null)
            {
                throw new BadRequestException(string.Format("Parent program {0} is not found.", parentProgramId));
            }
           
            if (_programrepository.GetVersion(parentVersion.OriginalEntity.Id, versionNumber) != null)
            {
                throw new BadRequestException(string.Format("Version {0} for Program {1} already exists.", versionNumber, parentVersion.OriginalEntity.Id));
            }

            var newVersion = parentVersion.CreateVersion(versionNumber) as Program;
            if (newVersion == null)
            {
                throw new Exception(string.Format("Failed to create a new version {0} from the parent version {1}", versionNumber, parentVersion.Id));
            }

            _programrepository.Save(newVersion);

            _bus.Publish(new ProgramCreated
            {
                Id = newVersion.Id,
                OrganizationId = newVersion.OrganizationId,
                Type = "program"
            });

            return Mapper.Map<ProgramResponse>(newVersion);
        }

        [AuthByAcl(Capability = Capability.EditProgram, ObjectId = "programId", ObjectType = typeof(Program))]
        public void PublishVersion(Guid programId, string publishNote)
        {
            var program = _programrepository.Get(programId);
            program.Publish(publishNote);
            _programrepository.Save(program);
        }

        [AuthByAcl(Capability = Capability.EditProgram, ObjectId = "programId", ObjectType = typeof(Program))]
        public void UpdateActiviationStatus(Guid programId, Common.Contract.ActivationRequest request)
        {
            var program = _programrepository.GetOrThrow(programId);
            program.UpdateActivationStatus(request);
            _programrepository.Save(program);
        }
    }
}