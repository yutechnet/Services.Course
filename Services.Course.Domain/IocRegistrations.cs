using System.Configuration;
using AutoMapper;
using Autofac;
using Autofac.Extras.DynamicProxy2;
using BpeProducts.Common.Authorization;
using BpeProducts.Common.Ioc;
using BpeProducts.Common.Ioc.Validation;
using BpeProducts.Common.Log;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.CourseAggregates;
using BpeProducts.Services.Course.Domain.ProgramAggregates;
using BpeProducts.Services.Course.Domain.Repositories;
using BpeProducts.Services.Course.Domain.Validation;

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

		    containerBuilder.RegisterType<CourseRepository>()
		                    .As<ICourseRepository>()
		                    .EnableInterfaceInterceptors()
		                    .EnableUserInputValidation()
		                    .EnableLogging();

		    containerBuilder.RegisterType<ProgramRepository>()
		                    .As<IProgramRepository>()
		                    .EnableInterfaceInterceptors()
		                    .EnableUserInputValidation()
                            .EnableLogging();

		    containerBuilder.RegisterType<CourseFactory>()
		                    .As<ICourseFactory>()
		                    .EnableInterfaceInterceptors()
		                    .EnableLogging();

		    containerBuilder.RegisterType<CourseService>()
		                    .As<ICourseService>()
		                    .EnableInterfaceInterceptors()
		                    .EnableAuthorization()
		                    .EnableLogging();

		    containerBuilder.RegisterType<LearningMaterialService>()
		                    .As<ILearningMaterialService>()
		                    .EnableInterfaceInterceptors()
		                    .EnableAuthorization()
		                    .EnableLogging();

            containerBuilder.RegisterType<AssetServiceClient>().As<IAssetServiceClient>();

			containerBuilder.RegisterType<AssessmentClient>().As<IAssessmentClient>();

		    containerBuilder.RegisterType<CourseSegmentService>()
		                    .As<ICourseSegmentService>()
		                    .EnableInterfaceInterceptors()
		                    .EnableLogging();

		    containerBuilder.RegisterType<CourseLearningActivityService>()
		                    .As<ICourseLearningActivityService>()
		                    .EnableInterfaceInterceptors()
		                    .EnableAuthorization()
		                    .EnableLogging();

		    containerBuilder.RegisterType<ProgramService>()
		                    .As<IProgramService>()
		                    .EnableInterfaceInterceptors()
		                    .EnableAuthorization()
		                    .EnableLogging();

			containerBuilder.RegisterType<VersionableEntityFactory>().As<IVersionableEntityFactory>();

            containerBuilder.RegisterType<CoursePublisher>().As<ICoursePublisher>();
			containerBuilder.RegisterType<CoursePublishValidator>().As<IValidator<CourseAggregates.Course>>();
            containerBuilder.RegisterType<LearningActivityPublishValidator>().As<IValidator<CourseLearningActivity>>();

		    RegisterMappings();

            MapperConfiguration.Configure();
		}

	    private void RegisterMappings()
	    {
	        Mapper.CreateMap<LearningMaterial, LearningMaterialInfo>()
	              .ForMember(x => x.CustomAttribute, opt => opt.MapFrom(src => src.MetaData));
	        Mapper.CreateMap<CourseLearningActivity, CourseLearningActivityResponse>()
	              .ForMember(x => x.CustomAttribute, opt => opt.MapFrom(src => src.MetaData));
	    }
	}
}