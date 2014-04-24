using System.Configuration;
using AutoMapper;
using Autofac;
using Autofac.Extras.DynamicProxy2;
using BpeProducts.Common.Authorization;
using BpeProducts.Common.Ioc;
using BpeProducts.Common.Ioc.Validation;
using BpeProducts.Common.Log;
using BpeProducts.Services.Asset.Contracts;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Courses;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Handlers;
using BpeProducts.Services.Course.Domain.Repositories;
using BpeProducts.Services.Course.Domain.Validation;
using Services.Assessment.Contract;

namespace BpeProducts.Services.Course.Domain
{
	public class IocRegistrations : IRegister
	{
		public void Register(ContainerBuilder containerBuilder)
		{
			string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
			bool updateSchema = false;
			bool.TryParse(ConfigurationManager.AppSettings["UpdateSchema"], out updateSchema);

			Common.NHibernate.IocRegistrations.RegisterSessionFactory(containerBuilder, connectionString, dropSchema: false,
			                                                          updateSchema: updateSchema);

			containerBuilder
				.RegisterType<CourseRepository>().As<ICourseRepository>()
				.EnableInterfaceInterceptors().EnableUserInputValidation()
				.InterceptedBy(typeof (PublicInterfaceLoggingInterceptor));
			containerBuilder
				.RegisterType<ProgramRepository>().As<IProgramRepository>()
                .EnableInterfaceInterceptors().EnableUserInputValidation()
				.InterceptedBy(typeof (PublicInterfaceLoggingInterceptor));

			containerBuilder.RegisterType<CourseFactory>().As<ICourseFactory>();
			containerBuilder.RegisterType<CourseService>().As<ICourseService>().EnableInterfaceInterceptors().EnableAuthorization();

            containerBuilder.RegisterType<LearningMaterialService>().As<ILearningMaterialService>().EnableInterfaceInterceptors().EnableAuthorization();
            containerBuilder.RegisterType<AssetServiceClient>().As<IAssetServiceClient>();

			containerBuilder.RegisterType<AssessmentClient>().As<IAssessmentClient>();

			containerBuilder.RegisterType<CourseSegmentService>().As<ICourseSegmentService>();
			containerBuilder.RegisterType<CourseLearningActivityService>().As<ICourseLearningActivityService>().EnableInterfaceInterceptors().EnableAuthorization();
			containerBuilder.RegisterType<ProgramService>().As<IProgramService>();
			containerBuilder.RegisterType<VersionableEntityFactory>().As<IVersionableEntityFactory>();

			containerBuilder.RegisterType<DomainEvents>().As<IDomainEvents>();

			containerBuilder.RegisterType<UpdateModelOnAddingCourseSegment>().As<IHandle<CourseSegmentAdded>>();
			containerBuilder.RegisterType<UpdateModelOnUpdatingCourseSegment>().As<IHandle<CourseSegmentUpdated>>();
			containerBuilder.RegisterType<UpdateModelOnDeletingCourseSegment>().As<IHandle<CourseSegmentDeleted>>();
			containerBuilder.RegisterType<UpdateModelOnReorderingCourseSegment>().As<IHandle<CourseSegmentReordered>>();

			containerBuilder.RegisterType<UpdateModelOnAddingCoursePrerequisite>().As<IHandle<CoursePrerequisiteAdded>>();
			containerBuilder.RegisterType<UpdateModelOnRemovingCoursePrerequisite>().As<IHandle<CoursePrerequisiteRemoved>>();

            containerBuilder.RegisterType<CoursePublisher>().As<ICoursePublisher>();
			containerBuilder.RegisterType<CoursePublishValidator>().As<IValidator<Courses.Course>>();
            containerBuilder.RegisterType<LearningActivityPublishValidator>().As<IValidator<CourseLearningActivity>>();

		    RegisterMappings();
		}

	    private void RegisterMappings()
	    {
	        Mapper.CreateMap<LearningMaterial, LearningMaterialInfo>();
	        Mapper.CreateMap<CourseLearningActivity, CourseLearningActivityResponse>();
	    }
	}
}