using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.DynamicProxy2;
using BpeProducts.Common.Authorization;
using BpeProducts.Common.Ioc;
using BpeProducts.Common.Log;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Acl.Client;
using BpeProducts.Services.Course.Domain.Courses;
using BpeProducts.Services.Course.Domain.Courses.Events;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Handlers;
using BpeProducts.Services.Course.Domain.Outcomes;
using BpeProducts.Services.Course.Domain.Repositories;
using EventStore;
using log4net;

namespace BpeProducts.Services.Course.Domain
{
    public class IocRegistrations : IRegister
    {
        public void Register(ContainerBuilder containerBuilder)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var updateSchema = false;
            bool.TryParse(ConfigurationManager.AppSettings["UpdateSchema"], out updateSchema);

            Common.NHibernate.IocRegistrations.RegisterSessionFactory(containerBuilder, connectionString, dropSchema: false, updateSchema: updateSchema);
			containerBuilder.Register<ILog>(c => LogManager.GetLogger("CourseBuilder"));
            containerBuilder
                .RegisterType<CourseRepository>().As<ICourseRepository>()
                .EnableInterfaceInterceptors().EnableValidation()
                .InterceptedBy(typeof(PublicInterfaceLoggingInterceptor));
            containerBuilder
                .RegisterType<ProgramRepository>().As<IProgramRepository>()
                .EnableInterfaceInterceptors().EnableValidation()
                .InterceptedBy(typeof(PublicInterfaceLoggingInterceptor));
            containerBuilder
                .RegisterType<LearningOutcomeRepository>().As<ILearningOutcomeRepository>()
                .EnableInterfaceInterceptors().EnableValidation()
                .InterceptedBy(typeof(PublicInterfaceLoggingInterceptor));

            containerBuilder.RegisterType<CourseEventStore>().As<IStoreCourseEvents>();
            containerBuilder.RegisterType<CourseFactory>().As<ICourseFactory>();
            containerBuilder.RegisterType<OutcomeFactory>().As<IOutcomeFactory>();
            containerBuilder.RegisterType<CourseService>().As<ICourseService>()
							.EnableInterfaceInterceptors()
							.EnableAuthorization();
            containerBuilder.RegisterType<CourseSegmentService>().As<ICourseSegmentService>();
            containerBuilder.RegisterType<CourseLearningActivityService>().As<ICourseLearningActivityService>();
            containerBuilder.RegisterType<LearningOutcomeService>().As<ILearningOutcomeService>();
            containerBuilder.RegisterType<ProgramService>().As<IProgramService>();
            containerBuilder.RegisterType<VersionHandler>().As<IVersionHandler>();
            containerBuilder.RegisterType<VersionableEntityFactory>().As<IVersionableEntityFactory>();
            containerBuilder.RegisterType<GraphValidator>().As<IGraphValidator>();

			containerBuilder.RegisterType<PlayCourseCreated>().Keyed<IPlayEvent>(typeof(CourseCreated).Name);
			containerBuilder.RegisterType<PlayCourseAssociatedWithProgram>().Keyed<IPlayEvent>(typeof(CourseAssociatedWithProgram).Name);
			containerBuilder.RegisterType<PlayCourseDeleted>().Keyed<IPlayEvent>(typeof(CourseDeleted).Name);
			containerBuilder.RegisterType<PlayCourseDisassociatedWithProgram>().Keyed<IPlayEvent>(typeof(CourseDisassociatedWithProgram).Name);
			containerBuilder.RegisterType<PlayCourseInfoUpated>().Keyed<IPlayEvent>(typeof(CourseInfoUpdated).Name);
			containerBuilder.RegisterType<PlayCourseSegmentAdded>().Keyed<IPlayEvent>(typeof(CourseSegmentAdded).Name);
            containerBuilder.RegisterType<PlayCourseSegmentUpdated>().Keyed<IPlayEvent>(typeof(CourseSegmentUpdated).Name);
            containerBuilder.RegisterType<PlayCourseSegmentDeleted>().Keyed<IPlayEvent>(typeof(CourseSegmentDeleted).Name);
            containerBuilder.RegisterType<PlayCourseSegmentReordered>().Keyed<IPlayEvent>(typeof(CourseSegmentReordered).Name);
            containerBuilder.RegisterType<PlayCourseLearningActivityAdded>().Keyed<IPlayEvent>(typeof(CourseLearningActivityAdded).Name);
            containerBuilder.RegisterType<PlayCourseLearningActivityUpdated>().Keyed<IPlayEvent>(typeof(CourseLearningActivityUpdated).Name);
            containerBuilder.RegisterType<PlayCourseLearningActivityDeleted>().Keyed<IPlayEvent>(typeof(CourseLearningActivityDeleted).Name);
            containerBuilder.RegisterType<PlayVersionCreated>().Keyed<IPlayEvent>(typeof(VersionCreated).Name);
            containerBuilder.RegisterType<PlayVersionPublished>().Keyed<IPlayEvent>(typeof(VersionPublished).Name);
            containerBuilder.RegisterType<PlayCoursePrerequisiteAdded>().Keyed<IPlayEvent>(typeof(CoursePrerequisiteAdded).Name);
            containerBuilder.RegisterType<PlayCoursePrerequisiteRemoved>().Keyed<IPlayEvent>(typeof(CoursePrerequisiteRemoved).Name);

            containerBuilder.RegisterType<PlayOutcomeCreated>().Keyed<IPlayEvent>(typeof(OutcomeCreated).Name);
            containerBuilder.RegisterType<PlayOutcomeUpdated>().Keyed<IPlayEvent>(typeof(OutcomeUpdated).Name);
            containerBuilder.RegisterType<PlayOutcomeDeleted>().Keyed<IPlayEvent>(typeof(OutcomeDeleted).Name);
            containerBuilder.RegisterType<PlayOutcomeVersionCreated>()
                            .Keyed<IPlayEvent>(typeof(OutcomeVersionCreated).Name);
            containerBuilder.RegisterType<PlayOutcomeVersionPublished>()
                            .Keyed<IPlayEvent>(typeof(OutcomeVersionPublished).Name);

			containerBuilder.RegisterType<DomainEvents>().As<IDomainEvents>();

			containerBuilder.Register<IStoreEvents>(x => new CourseEventStore());

            //there must be an easier way using register generics
            //containerBuilder.RegisterType<EventPersisterHandler>().As<IHandle<CourseAssociatedWithProgram>>();
            //containerBuilder.RegisterType<EventPersisterHandler>().As<IHandle<CourseDisassociatedWithProgram>>();

            containerBuilder.RegisterType<UpdateModelOnCourseCreation>().As<IHandle<CourseCreated>>();
            //containerBuilder.RegisterType<EventPersisterHandler>().As<IHandle<CourseCreated>>();
            
            //containerBuilder.RegisterType<EventPersisterHandler>().As<IHandle<CourseInfoUpdated>>();

            //containerBuilder.RegisterType<EventPersisterHandler>().As<IHandle<CourseSegmentAdded>>();
            containerBuilder.RegisterType<UpdateModelOnAddingCourseSegment>().As<IHandle<CourseSegmentAdded>>();
            //containerBuilder.RegisterType<EventPersisterHandler>().As<IHandle<CourseSegmentUpdated>>();
            //containerBuilder.RegisterType<EventPersisterHandler>().As<IHandle<CourseSegmentDeleted>>();
            //containerBuilder.RegisterType<EventPersisterHandler>().As<IHandle<CourseSegmentReordered>>();
            containerBuilder.RegisterType<UpdateModelOnUpdatingCourseSegment>().As<IHandle<CourseSegmentUpdated>>();
            containerBuilder.RegisterType<UpdateModelOnDeletingCourseSegment>().As<IHandle<CourseSegmentDeleted>>();
            containerBuilder.RegisterType<UpdateModelOnReorderingCourseSegment>().As<IHandle<CourseSegmentReordered>>();

            //containerBuilder.RegisterType<EventPersisterHandler>().As<IHandle<CourseLearningActivityAdded>>();
            containerBuilder.RegisterType<UpdateModelOnAddingCourseLearningActivity>().As<IHandle<CourseLearningActivityAdded>>();
            //containerBuilder.RegisterType<EventPersisterHandler>().As<IHandle<CourseLearningActivityUpdated>>();
            containerBuilder.RegisterType<UpdateModelOnUpdatingCourseLearningActivity>().As<IHandle<CourseLearningActivityUpdated>>();
            //containerBuilder.RegisterType<EventPersisterHandler>().As<IHandle<CourseLearningActivityDeleted>>();
            containerBuilder.RegisterType<UpdateModelOnDeletingCourseLearningActivity>().As<IHandle<CourseLearningActivityDeleted>>();
            
            containerBuilder.RegisterType<UpdateModelOnCourseUpdating>().As<IHandle<CourseUpdated>>();
			containerBuilder.RegisterType<CourseUpdatedHandler>().As<IHandle<CourseUpdated>>();

			containerBuilder.RegisterType<UpdateModelOnAddingCoursePrerequisite>().As<IHandle<CoursePrerequisiteAdded>>();
			//containerBuilder.RegisterType<EventPersisterHandler>().As<IHandle<CoursePrerequisiteAdded>>();

			containerBuilder.RegisterType<UpdateModelOnRemovingCoursePrerequisite>().As<IHandle<CoursePrerequisiteRemoved>>();
			//containerBuilder.RegisterType<EventPersisterHandler>().As<IHandle<CoursePrerequisiteRemoved>>();

			containerBuilder.RegisterType<UpdateModelOnCourseDeletion>().As<IHandle<CourseDeleted>>();
			//containerBuilder.RegisterType<EventPersisterHandler>().As<IHandle<CourseDeleted>>();

            //containerBuilder.RegisterType<EventPersisterHandler>().As<IHandle<VersionCreated>>();
            containerBuilder.RegisterType<UpdateModelOnCourseVersionCreation>().As<IHandle<VersionCreated>>();

            //containerBuilder.RegisterType<EventPersisterHandler>().As<IHandle<VersionPublished>>();
            containerBuilder.RegisterType<UpdateModelOnCourseVersionPublish>().As<IHandle<VersionPublished>>();
            containerBuilder.RegisterType<UpdateModelOnOutcomeCreation>().As<IHandle<OutcomeCreated>>();
            //containerBuilder.RegisterType<EventPersisterHandler>().As<IHandle<OutcomeCreated>>();
            containerBuilder.RegisterType<UpdateModelOnOutcomeUpdate>().As<IHandle<OutcomeUpdated>>();
            //containerBuilder.RegisterType<EventPersisterHandler>().As<IHandle<OutcomeUpdated>>();

            containerBuilder.RegisterType<UpdateModelOnOutcomeDeletion>().As<IHandle<OutcomeDeleted>>();
            //containerBuilder.RegisterType<EventPersisterHandler>().As<IHandle<OutcomeDeleted>>();

            //containerBuilder.RegisterType<EventPersisterHandler>().As<IHandle<OutcomeVersionCreated>>();
            containerBuilder.RegisterType<UpdateModelOnOutcomeVersionCreation>().As<IHandle<OutcomeVersionCreated>>();

            //containerBuilder.RegisterType<EventPersisterHandler>().As<IHandle<OutcomeVersionPublished>>();
            containerBuilder.RegisterType<UpdateModelOnOutcomeVersionPublished>().As<IHandle<OutcomeVersionPublished>>();

        }
    }
}
