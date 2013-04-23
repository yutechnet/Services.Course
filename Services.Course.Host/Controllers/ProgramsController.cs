using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using BpeProducts.Common.WebApi.Attributes;
using BpeProducts.Services.Course.Domain.Entities;
using NHibernate;
using NHibernate.Linq;

namespace BpeProducts.Services.Course.Host.Controllers
{

    public class ProgramsController : ApiController
    {
	    private ISession _session;
        public ProgramsController(ISession session)
        {
	        _session = session;
        }

        // GET api/programs
        public IEnumerable<ProgramResponse> Get()
        {
			var programs = _session.Query<Program>();
	        var response = new List<ProgramResponse>();
	        Mapper.Map(programs, response);
	        return response;
        }

        // GET api/programs/5
        public ProgramResponse Get(Guid id)
        {
	        var program = _session.Get<Program>(id);
			var programResponse= new ProgramResponse
				{
					Id = program.Id,
					Name = program.Name,
					Description = program.Name
				};
	        return programResponse;
        }

        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
        // POST api/programs
        public HttpResponseMessage Post(CreateProgramRequest request)
        {
	        var program = new Program();
	        Mapper.Map(request, program);
            var id = (Guid) _session.Save(program);
	        var programResponse = new ProgramResponse();
	        Mapper.Map(program, programResponse);
			var response = base.Request.CreateResponse<ProgramResponse>(HttpStatusCode.Created, programResponse);
			var uri = Url.Link("DefaultApi", new { id = programResponse.Id });
			if (uri != null)
			{
				response.Headers.Location = new Uri(uri);
			}
			return response;
        }

        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
		// PUT api/programs/5
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
	        try
	        {
				var programInDb = _session.Load<Program>(id);
				// logical delete
				programInDb.ActiveFlag = false;
				_session.Update(programInDb);
	        }
	        catch (Exception)
	        {
				throw new HttpResponseException(HttpStatusCode.NotFound);
	        }		
        }
    }
}

