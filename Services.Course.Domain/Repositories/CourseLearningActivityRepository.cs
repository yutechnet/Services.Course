using System;
using System.Collections.Generic;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Course.Domain.Courses;
using NHibernate;

namespace BpeProducts.Services.Course.Domain.Repositories
{

    public interface ICourseLearningActivityRepository : IRepository<CourseLearningActivity>
    {
        CourseLearningActivity Get(Guid id);
        //IList<CourseLearningActivity> Get(List<Guid> ids);
        //void Save(CourseLearningActivity courseLearning);
    }

    public class CourseLearningActivityRepository : RepositoryBase<CourseLearningActivity>, ICourseLearningActivityRepository
    {
        private readonly ISession _session;

        public CourseLearningActivityRepository(ISession session) : base(session)
        {
            _session = session;
        }

        public CourseLearningActivity Get(Guid id)
        {
            return _session.Get<CourseLearningActivity>(id);
        }
    }
}
