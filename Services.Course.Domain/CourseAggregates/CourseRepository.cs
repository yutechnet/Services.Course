using System;
using System.Collections.Generic;
using System.Linq;
using BpeProducts.Common.Exceptions;
using BpeProducts.Common.Ioc.Validation;
using BpeProducts.Common.NHibernate;
using NHibernate.Criterion;

namespace BpeProducts.Services.Course.Domain.CourseAggregates
{
    [Validate]
    public class CourseRepository : ICourseRepository
    {
        private readonly IRepository _repository;

        public CourseRepository(IRepository repository)
        {
            _repository = repository;
        }

        public Course Get(Guid courseId)
        {
            var course = _repository.Get<Course>(courseId);
            return course;
        }

        public CourseSegment GetSegment(Guid courseId, Guid segmentId)
        {
            var segment = _repository.Query<CourseSegment>().Single(s => s.Course.Id == courseId && s.Id == segmentId);
            return segment;
        }

        public LearningMaterial GetLearningMaterial(Guid learningMaterialId)
        {
            var learningMaterial = _repository.Get<LearningMaterial>(learningMaterialId);
            if (learningMaterial == null || !learningMaterial.ActiveFlag)
            {
                throw new NotFoundException(string.Format("LearningMaterial {0} not found.", learningMaterialId));
            }
            return learningMaterial;
        }

        public IEnumerable<Course> GetPublishedCourses(Guid organizationId)
        {
            return
                _repository.Query<Course>()
                           .Where(c => c.ActiveFlag == true && c.IsPublished && c.OrganizationId == organizationId).AsEnumerable();
        }

        public IList<Course> Get(List<Guid> ids)
        {
            var courses = new List<Course>();
            foreach (var id in ids)
            {
                var course = _repository.Get<Course>(id);

                if (course == null)
                    throw new BadRequestException(string.Format("Course {0} does not exist", id));

                courses.Add(course);
            }

            return courses;
        }

        public Course GetOrThrow(Guid courseId)
        {
            var course = Get(courseId);

            if (course == null || !course.ActiveFlag)
            {
                throw new NotFoundException(string.Format("Course {0} not found.", courseId));
            }

            return course;
        }

        public void Save(Course course)
        {
            _repository.SaveOrUpdate(course);
        }

        public void Delete(Course course)
        {
            course.ActiveFlag = false;
            _repository.SaveOrUpdate(course);
        }

        public Course GetVersion(Guid originalEntityId, string versionNumber)
        {
            var version = (from c in _repository.Query<Course>()
                           where c.OriginalEntity.Id == originalEntityId && c.VersionNumber == versionNumber
                           select c).FirstOrDefault();

            return version;
        }

        public IList<Course> ODataQuery(string queryString)
        {
            var criteria = _repository.ODataQuery<Course>(queryString);
            criteria.Add(Restrictions.Eq("ActiveFlag", true));

            var courses = criteria.List<Course>();

            return courses;
        }
    }
}
