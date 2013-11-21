﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BpeProducts.Common.Authorization;
using BpeProducts.Common.Capabilities;
using BpeProducts.Common.Exceptions;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Domain
{
    public class CourseService : ICourseService
    {
        private readonly ICourseFactory _courseFactory;
        private readonly IDomainEvents _domainEvents;
        private readonly ICourseRepository _courseRepository;

        public CourseService(ICourseFactory courseFactory, IDomainEvents domainEvents, ICourseRepository courseRepository)
        {
            _courseFactory = courseFactory;
            _domainEvents = domainEvents;
            _courseRepository = courseRepository;
        }

		[AuthByAcl(Capability = Capability.CourseCreate, OrganizationObject = "request")]
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

        public void Update(Guid courseId, UpdateCourseRequest request)
        {
            var course = _courseFactory.Reconstitute(courseId);

            if (course == null || !course.ActiveFlag)
            {
                throw new NotFoundException(string.Format("Course {0} not found.", courseId));
            }

            _domainEvents.Raise<CourseUpdated>(new CourseUpdated
            {
                AggregateId = courseId,
                Old = course,
                Request = request
            });
        }

		[AuthByAcl(Capability = Capability.CourseView, ObjectIdArgument = "courseId", ObjectType = typeof(Courses.Course))]
        public CourseInfoResponse Get(Guid courseId)
        {
            var course = _courseRepository.GetOrThrow(courseId);

            var courseInfo = Mapper.Map<CourseInfoResponse>(course);

            courseInfo.Segments = (from s in courseInfo.Segments 
                                   where s.ParentSegmentId == Guid.Empty 
                                   select s).ToList();
            return courseInfo;
        }

        public IEnumerable<CourseInfoResponse> Search(string queryString)
        {
            var queryArray = queryString.Split('?');
            var courses = _courseRepository.ODataQuery(queryArray.Length > 1 ? queryArray[1] : "");

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

        public void UpdatePrerequisiteList(Guid courseId, List<Guid> newPrerequisiteIds)
        {
            var course = _courseFactory.Reconstitute(courseId);
            if (course == null || !course.ActiveFlag)
            {
                throw new NotFoundException(string.Format("Course {0} not found.", courseId));
            }

            var existing = (from prereq in course.Prerequisites
                            select prereq.Id).ToList();

            var removed = (from e in existing
                           where !newPrerequisiteIds.Contains(e)
                           select e).ToList();

            var added = (from n in newPrerequisiteIds
                         where !existing.Contains(n)
                         select n).ToList();

            foreach (var toAdd in added)
            {
                _domainEvents.Raise<CoursePrerequisiteAdded>(new CoursePrerequisiteAdded
                    {
                        AggregateId = courseId,
                        PrerequisiteCourseId = toAdd,
                    });
            }

            foreach (var toRemove in removed)
            {
                _domainEvents.Raise<CoursePrerequisiteRemoved>(new CoursePrerequisiteRemoved
                {
                    AggregateId = courseId,
                    PrerequisiteCourseId = toRemove,
                });
            }
        }
    }
}
