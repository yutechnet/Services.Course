
using System;
using System.Net;
using BpeProducts.Common.Exceptions;
using NHibernate;

namespace BpeProducts.Services.Course.Domain.Courses
{
    public interface ICourseLearningActivityFactory
    {
        CourseLearningActivity Build(CourseLearningActivityDto courseLearningActivityDto);
    }

    public class CourseLearningActivityFactory : ICourseLearningActivityFactory
    {
        private readonly ISession _session;

        public CourseLearningActivityFactory(
            ISession session)
        {
            _session = session;
        }


        public CourseLearningActivity Build(CourseLearningActivityDto courseLearningActivityDto)
        {
            //some checks first?
            return new CourseLearningActivity
                {
                    TenantId = courseLearningActivityDto.TenantId,
                    Name = courseLearningActivityDto.Name,
                    Type = courseLearningActivityDto.Type,
                    IsExtraCredit = courseLearningActivityDto.IsExtraCredit,
                    IsGradeable = courseLearningActivityDto.IsGradeable,
                    MaxPoint = courseLearningActivityDto.MaxPoint,
                    Weight = courseLearningActivityDto.Weight,
                    ObjectId = courseLearningActivityDto.ObjectId
                };
        }
    }
}
