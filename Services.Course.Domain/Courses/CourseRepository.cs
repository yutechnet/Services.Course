using System;
using System.Collections.Generic;
using System.Linq;
using BpeProducts.Common.Exceptions;
using BpeProducts.Common.Ioc.Validation;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Course.Domain.Courses;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;

namespace BpeProducts.Services.Course.Domain.Repositories
{
    [Validate]
    public class CourseRepository : ICourseRepository
    {
        private readonly IRepository _repository;

        public CourseRepository(IRepository repository)
        {
            _repository = repository;
        }

        public Courses.Course Get(Guid courseId)
        {
            var course = _repository.Get<Courses.Course>(courseId);
            return course;
        }

		public CourseSegment GetSegment(Guid courseId, Guid segmentId)
		{
			var segment = _repository.Query<CourseSegment>().Single(s => s.Course.Id == courseId && s.Id == segmentId);
			return segment;
		}

        public LearningMaterial GetLearningMaterial(Guid learningMaterialId)
        {
            var learningMaterial = _repository.Query<LearningMaterial>().Single(c => c.Id == learningMaterialId);
            return learningMaterial;
        }

		public RubricAssociation GetRubricAssociation(Guid rubricAssociationId)
		{
			var rubricAssociation = _repository.Query<RubricAssociation>().Single(c => c.Id == rubricAssociationId);
			return rubricAssociation;
		}

        public IList<Courses.Course> Get(List<Guid> ids)
        {
            var courses = new List<Courses.Course>();
            foreach (var id in ids)
            {
                var course = _repository.Get<Courses.Course>(id);

                if (course == null)
                    throw new BadRequestException(string.Format("Course {0} does not exist", id));

                courses.Add(course);
            }

            return courses;
        }
        
        public Courses.Course GetOrThrow(Guid courseId)
        {
            var course = Get(courseId);

            if (course == null || !course.ActiveFlag)
            {
                throw new NotFoundException(string.Format("Course {0} not found.", courseId));
            }

            return course;
        }

        public void Save(Courses.Course course)
        {
            _repository.SaveOrUpdate(course);
        }

        public void Delete(Courses.Course course)
        {
            course.ActiveFlag = false;
            _repository.SaveOrUpdate(course);
        }

        public Courses.Course GetVersion(Guid originalEntityId, string versionNumber)
        {
            var version = (from c in _repository.Query<Courses.Course>()
                           where c.OriginalEntity.Id == originalEntityId && c.VersionNumber == versionNumber
                         select c).FirstOrDefault();

            return version;
        }

        public IList<Courses.Course> ODataQuery(string queryString)
        {
			
            var criteria = _repository.ODataQuery<Courses.Course>(queryString);
            criteria.Add(Restrictions.Eq("ActiveFlag", true));
			
            var courses = criteria.List<Courses.Course>();

            return courses;
        }
    }
}
