using System;
using System.Collections.Generic;
using AutoMapper;
using BpeProducts.Common.Exceptions;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;
using NHibernate;
using NHibernate.Criterion;

namespace BpeProducts.Services.Course.Domain
{
    public class CourseService : ICourseService, IHandleVersioning<Entities.Course>
    {
        private readonly ICourseFactory _courseFactory;
        private readonly IDomainEvents _domainEvents;
        private readonly ICourseRepository _courseRepository;
        private readonly IRepository _repository;

        public CourseService(ICourseFactory courseFactory, IDomainEvents domainEvents, ICourseRepository courseRepository, IRepository repository)
        {
            _courseFactory = courseFactory;
            _domainEvents = domainEvents;
            _courseRepository = courseRepository;
            _repository = repository;
        }

        public CourseInfoResponse Create(SaveCourseRequest request)
        {
            var course = _courseFactory.Create(request);
            _domainEvents.Raise<CourseCreated>(new CourseCreated
            {
                AggregateId = course.Id,
                Course = course
            });

            return Mapper.Map<CourseInfoResponse>(course);
        }

        public void Update(Guid courseId, SaveCourseRequest request)
        {
            var course = _courseFactory.Reconstitute(courseId);

            if (course == null || !course.ActiveFlag)
            {
                throw new NotFoundException(string.Format("Course {0} not found.", courseId));
            }

            if (course.IsPublished)
            {
                throw new ForbiddenException(string.Format("Course {0} is published and cannot be modified.", courseId));
            }

            _domainEvents.Raise<CourseUpdated>(new CourseUpdated
            {
                AggregateId = courseId,
                Old = course,
                Request = request
            });

        }

        public CourseInfoResponse Get(Guid courseId)
        {
            var course = _repository.Get<Entities.Course>(courseId);
            // var course = _courseRepository.Get(courseId);
            if (course == null || !course.ActiveFlag)
            {
                throw new NotFoundException(string.Format("Course {0} not found.", courseId));
            }
            var courseInfoResponse = Mapper.Map<CourseInfoResponse>(course);
            // TODO Is this necessary? Should be done in the AutoMapper
            courseInfoResponse.Segments = course.Segments;

            return courseInfoResponse;
        }

        public IEnumerable<CourseInfoResponse> Search(string queryString)
        {
            var queryArray = queryString.Split('?');
            var criteria =_repository.ODataQuery<Entities.Course>(queryArray.Length > 1 ? queryArray[1] : "");
            criteria.Add(Restrictions.Eq("ActiveFlag", true));
            var courses = criteria.List<Domain.Entities.Course>();
            var courseResponses = new List<CourseInfoResponse>();
            Mapper.Map(courses, courseResponses);
            return courseResponses;
        }

        public void Delete(Guid courseId)
        {
            var course = _courseFactory.Reconstitute(courseId);

            if (course == null || !course.ActiveFlag)
            {
                throw new NotFoundException(string.Format("Course {0} not found.", courseId));
            }

            if (course.IsPublished)
            {
                throw new ForbiddenException(string.Format("Course {0} is published and cannot be deleted.", courseId));
            }

            _domainEvents.Raise<CourseDeleted>(new CourseDeleted
            {
                AggregateId = courseId,
            });
        }

        public void PublishVersion(Guid entityId, string publishNote)
        {
            var course = _courseFactory.Reconstitute(entityId);

            if (course == null)
            {
                throw new NotFoundException(string.Format("Course {0} not found.", entityId));
            }

            _domainEvents.Raise<CourseVersionPublished>(new CourseVersionPublished
            {
                AggregateId = entityId,
                PublishNote = publishNote
            });
        }

        public Entities.Course CreateVersion(VersionRequest request)
        {
            var course = _courseFactory.Reconstitute(request.ParentVersionId);
            if (course == null)
            {
                throw new NotFoundException(string.Format("Parent course {0} not found.", request.ParentVersionId));
            }

            var newVersion = _courseFactory.BuildNewVersion(course, request.VersionNumber);

            _domainEvents.Raise<CourseVersionCreated>(new CourseVersionCreated
            {
                AggregateId = newVersion.Id,
                NewVersion = newVersion
            });

            return newVersion;
        }
    }
}