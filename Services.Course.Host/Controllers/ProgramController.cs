using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using BpeProducts.Common.WebApi.Attributes;
using NHibernate;

namespace BpeProducts.Services.Course.Host.Controllers
{

    public class ProgramController : ApiController
    {
	    private ISession _session;
        public ProgramController(ISession session)
        {
	        _session = session;
        }

        // GET api/programs
        public IEnumerable<ProgramResponse> Get()
        {
			return new List<ProgramResponse>();
        }

        // GET api/programs/5
        public ProgramResponse Get(Guid id)
        {
	        var program = _session.Get<Program>(id);
			return new ProgramResponse();
        }

        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
        // POST api/programs
        public HttpResponseMessage Post(CreateProgramRequest request)
        {
	        var program = new Program {Name=request.Name};
            var id = (Guid) _session.Save(program);
			return new HttpResponseMessage();
        }

        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
        // PUT api/courses/5
        public void Put(Guid id, CreateProgramRequest request)
        {
            // We do not allow creation of a new resource by PUT.
            var programInDb = _session.Get<Program>(id);
            
            if(programInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);    
            }

            Mapper.Map(request, programInDb);
            _session.Update(programInDb);
        }

        [Transaction]
        // DELETE api/programs/5
        public void Delete(Guid id)
        {
            var programInDb = _session.Get<Program>(id);

            if (programInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            // logical delete
            programInDb.ActiveFlag = false;
            _session.Update(programInDb);
        }
    }

	public class CreateProgramRequest
	{
		public string Name { get; set; }
	}

	public class Program
	{
		public String Name
		{
			get ; 
			set ; 
		}

		public bool ActiveFlag
		{	
			get ; 
			set ;
		}
	}

	public class ProgramResponse
	{
	}
}

