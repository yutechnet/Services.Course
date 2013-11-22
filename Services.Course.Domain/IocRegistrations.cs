using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using AutoMapper;
using Autofac;
using Autofac.Extras.DynamicProxy2;
using BpeProducts.Common.Authorization;
using BpeProducts.Common.Exceptions;
using BpeProducts.Common.Ioc;
using BpeProducts.Common.Log;
using BpeProducts.Services.Asset.Contracts;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Courses;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Handlers;
using BpeProducts.Services.Course.Domain.Outcomes;
using BpeProducts.Services.Course.Domain.Repositories;

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
				.EnableInterfaceInterceptors().EnableValidation()
				.InterceptedBy(typeof (PublicInterfaceLoggingInterceptor));
			containerBuilder
				.RegisterType<ProgramRepository>().As<IProgramRepository>()
				.EnableInterfaceInterceptors().EnableValidation()
				.InterceptedBy(typeof (PublicInterfaceLoggingInterceptor));
			containerBuilder
				.RegisterType<LearningOutcomeRepository>().As<ILearningOutcomeRepository>()
				.EnableInterfaceInterceptors().EnableValidation()
				.InterceptedBy(typeof (PublicInterfaceLoggingInterceptor));
            
			containerBuilder.RegisterType<CourseFactory>().As<ICourseFactory>();
			containerBuilder.RegisterType<OutcomeFactory>().As<IOutcomeFactory>();
			containerBuilder.RegisterType<CourseService>().As<ICourseService>().EnableInterfaceInterceptors().EnableAuthorization();

            containerBuilder.RegisterType<LearningMaterialService>().As<ILearningMaterialService>().EnableInterfaceInterceptors().EnableAuthorization();

		    containerBuilder.Register(context =>
		        {
		            var uriBaseAddress = new Uri(ConfigurationManager.AppSettings["AssetServiceBaseUrl"]);
		            var tokenExtractor = context.Resolve<ISamlTokenExtractor>();
		            return new AssetServiceClient(tokenExtractor, uriBaseAddress);
		        }).As<IAssetServiceClient>();

			containerBuilder.RegisterType<CourseSegmentService>().As<ICourseSegmentService>();
			containerBuilder.RegisterType<CourseLearningActivityService>().As<ICourseLearningActivityService>();
			containerBuilder.RegisterType<LearningOutcomeService>().As<ILearningOutcomeService>();
			containerBuilder.RegisterType<ProgramService>().As<IProgramService>();
			containerBuilder.RegisterType<VersionHandler>().As<IVersionHandler>();
			containerBuilder.RegisterType<VersionableEntityFactory>().As<IVersionableEntityFactory>();
			containerBuilder.RegisterType<GraphValidator>().As<IGraphValidator>();


			containerBuilder.RegisterType<DomainEvents>().As<IDomainEvents>();


			containerBuilder.RegisterType<UpdateModelOnCourseCreation>().As<IHandle<CourseCreated>>();
			containerBuilder.RegisterType<UpdateModelOnAddingCourseSegment>().As<IHandle<CourseSegmentAdded>>();
			containerBuilder.RegisterType<UpdateModelOnUpdatingCourseSegment>().As<IHandle<CourseSegmentUpdated>>();
			containerBuilder.RegisterType<UpdateModelOnDeletingCourseSegment>().As<IHandle<CourseSegmentDeleted>>();
			containerBuilder.RegisterType<UpdateModelOnReorderingCourseSegment>().As<IHandle<CourseSegmentReordered>>();

			containerBuilder.RegisterType<UpdateModelOnAddingCourseLearningActivity>().As<IHandle<CourseLearningActivityAdded>>();
			containerBuilder.RegisterType<UpdateModelOnUpdatingCourseLearningActivity>().As<IHandle<CourseLearningActivityUpdated>>();
			containerBuilder.RegisterType<UpdateModelOnDeletingCourseLearningActivity>().As<IHandle<CourseLearningActivityDeleted>>();

			containerBuilder.RegisterType<UpdateModelOnCourseUpdating>().As<IHandle<CourseUpdated>>();
			containerBuilder.RegisterType<CourseUpdatedHandler>().As<IHandle<CourseUpdated>>();

			containerBuilder.RegisterType<UpdateModelOnAddingCoursePrerequisite>().As<IHandle<CoursePrerequisiteAdded>>();

			containerBuilder.RegisterType<UpdateModelOnRemovingCoursePrerequisite>().As<IHandle<CoursePrerequisiteRemoved>>();

			containerBuilder.RegisterType<UpdateModelOnCourseDeletion>().As<IHandle<CourseDeleted>>();
			containerBuilder.RegisterType<UpdateModelOnCourseVersionCreation>().As<IHandle<VersionCreated>>();

			containerBuilder.RegisterType<UpdateModelOnCourseVersionPublish>().As<IHandle<VersionPublished>>();
			containerBuilder.RegisterType<UpdateModelOnOutcomeCreation>().As<IHandle<OutcomeCreated>>();
			containerBuilder.RegisterType<UpdateModelOnOutcomeUpdate>().As<IHandle<OutcomeUpdated>>();

			containerBuilder.RegisterType<UpdateModelOnOutcomeDeletion>().As<IHandle<OutcomeDeleted>>();

			containerBuilder.RegisterType<UpdateModelOnOutcomeVersionCreation>().As<IHandle<OutcomeVersionCreated>>();

			containerBuilder.RegisterType<UpdateModelOnOutcomeVersionPublished>().As<IHandle<OutcomeVersionPublished>>();

		    RegisterMappings();
		}

	    private void RegisterMappings()
	    {
	        Mapper.CreateMap<LearningMaterial, LearningMaterialInfo>();
	    }
	}

    public interface IAssetServiceClient
    {
        LibraryInfo AddAssetToLibrary(string ownerType, Guid ownerId, Guid assetId);
    }

    public class AssetServiceClient : IAssetServiceClient
    {
        private readonly ISamlTokenExtractor _tokenExtractor;
        public Uri BaseAddress { get; private set; }

        public AssetServiceClient(ISamlTokenExtractor tokenExtractor)
        {
            _tokenExtractor = tokenExtractor;
        }

        public AssetServiceClient(ISamlTokenExtractor tokenExtractor, Uri baseAddress)
        {
            _tokenExtractor = tokenExtractor;
            BaseAddress = baseAddress;
        }

        public LibraryInfo AddAssetToLibrary(string ownerType, Guid ownerId, Guid assetId)
        {
            var samlToken = _tokenExtractor.GetSamlToken();

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("SAML", samlToken);

            var uri = string.Format("{0}/library/{1}/{2}/asset/{3}", BaseAddress, ownerType, ownerId, assetId);
            var result = client.PutAsJsonAsync(uri, new { }).Result;

            CheckForErrors(result);

            return result.Content.ReadAsAsync<LibraryInfo>().Result;
        }

        private void CheckForErrors(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return;

            var message = string.Format("Unexpected StatusCode {0} resluted from {1} call to {2}", response.StatusCode,
                                        response.RequestMessage.Method, response.RequestMessage.RequestUri);

            if ((int)response.StatusCode <= 199)
            {
                throw new InternalServerErrorException(message);
            }
            if ((int)response.StatusCode <= 399)
            {
                throw new InternalServerErrorException(message);
            }
            if ((int)response.StatusCode <= 499)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                throw new BadRequestException(content);
            }

            throw new InternalServerErrorException(message);
        }
    }
}