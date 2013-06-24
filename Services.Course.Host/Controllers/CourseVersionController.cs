﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using BpeProducts.Common.WebApi.Attributes;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Host.Controllers
{
    public class CourseVersionController : ApiController
    {
        private readonly IRepository _courseRepository;
        private readonly IDomainEvents _domainEvents;
	    private readonly ICourseFactory _courseFactory;

        public CourseVersionController(IRepository courseRepository, IDomainEvents domainEvents, ICourseFactory courseFactory)
        {
            _courseRepository = courseRepository;
            _domainEvents = domainEvents;
	        _courseFactory = courseFactory;
        }


        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
        [HttpPut]
        public void PublishVersion(Guid id, CoursePublishRequest request)
        {
            // /courses/<id>/publish -- publish a course
            // /program/<id>/publish -- publish a program
            // /outcome/<id>/publish --
            // /sang/<id>

            // /course/version {parentEntityId} -- create a new version based on the parent version
            // /program/version {parentEntityId} -- create a new version based on the parent version
            // /outcome/version {parentEntityId} -- create a new version based on the parent version


            // Get the entity from Repository using IVersionable interface

            var courseInDb = _courseFactory.Reconstitute(id);

            if (courseInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            _domainEvents.Raise<CourseVersionPublished>(new CourseVersionPublished
            {
                AggregateId = id,
                PublishNote = request.PublishNote
            });
        }

        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
        [HttpPost]
        public HttpResponseMessage CreateVersion(CourseVersionRequest request)
        {
            var courseInDb = _courseRepository.Query<Domain.Entities.Course>().FirstOrDefault(c => c.Id.Equals(request.ParentVersionId) && c.ActiveFlag.Equals(true));
            if (courseInDb == null)
            {
                throw new HttpResponseException(new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        ReasonPhrase = string.Format("Parent version {0} not found.", request.ParentVersionId)
                    });
            }

            var versionExists =
                _courseRepository.Query<Domain.Entities.Course>()
                                 .Any(
                                     c =>
                                     c.OriginalEntityId.Equals(courseInDb.OriginalEntityId) && c.ActiveFlag.Equals(true) &&
                                     c.VersionNumber.Equals(request.VersionNumber));
            if (versionExists)
            {
                throw new HttpResponseException(new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.Conflict,
                        ReasonPhrase = string.Format("Version {0} for the course {1} already exists", request.VersionNumber, courseInDb.OriginalEntityId)
                    });
            }

            var courseId = Guid.NewGuid();
            _domainEvents.Raise<CourseVersionCreated>(new CourseVersionCreated
                {
                    AggregateId = courseId,
                    IsPublished = false,
                    OriginalCourseId = courseInDb.OriginalEntityId,
                    ParentCourseId = request.ParentVersionId,
                    VersionNumber = request.VersionNumber
                });

            var courseInfoResponse = Mapper.Map<CourseInfoResponse>(_courseRepository.Get<Domain.Entities.Course>(courseId));
            HttpResponseMessage response = base.Request.CreateResponse(HttpStatusCode.Created, courseInfoResponse);

            string uri = Url.Link("DefaultApi", new { controller = "courses", id = courseInfoResponse.Id });
            if (uri != null)
            {
                response.Headers.Location = new Uri(uri);
            }
            return response;
        }

    }
}
