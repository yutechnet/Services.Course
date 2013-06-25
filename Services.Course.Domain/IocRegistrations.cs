﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.DynamicProxy2;
using BpeProducts.Common.Ioc;
using BpeProducts.Common.Log;
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

	        containerBuilder.RegisterType<Repository>().As<IRepository>()
	                        .EnableInterfaceInterceptors()
	                        .EnableValidation()
				.InterceptedBy(typeof(PublicInterfaceLoggingInterceptor)); ;

            containerBuilder.RegisterType<CourseEventStore>().As<IStoreCourseEvents>();
            containerBuilder.RegisterType<CourseFactory>().As<ICourseFactory>();
            containerBuilder.RegisterType<OutcomeFactory>().As<IOutcomeFactory>();

			containerBuilder.RegisterType<PlayCourseCreated>().Keyed<IPlayEvent>(typeof(CourseCreated).Name);
			containerBuilder.RegisterType<PlayCourseAssociatedWithProgram>().Keyed<IPlayEvent>(typeof(CourseAssociatedWithProgram).Name);
			containerBuilder.RegisterType<PlayCourseDeleted>().Keyed<IPlayEvent>(typeof(CourseDeleted).Name);
			containerBuilder.RegisterType<PlayCourseDisassociatedWithProgram>().Keyed<IPlayEvent>(typeof(CourseDisassociatedWithProgram).Name);
			containerBuilder.RegisterType<PlayCourseInfoUpated>().Keyed<IPlayEvent>(typeof(CourseInfoUpdated).Name);
			containerBuilder.RegisterType<PlayCourseSegmentAdded>().Keyed<IPlayEvent>(typeof(CourseSegmentAdded).Name);
            containerBuilder.RegisterType<PlayCourseSegmentUpdated>().Keyed<IPlayEvent>(typeof(CourseSegmentUpdated).Name);
            containerBuilder.RegisterType<PlayCourseVersionCreated>().Keyed<IPlayEvent>(typeof(CourseVersionCreated).Name);
            containerBuilder.RegisterType<PlayCourseVersionPublished>().Keyed<IPlayEvent>(typeof(CourseVersionPublished).Name);

            containerBuilder.RegisterType<PlayOutcomeCreated>().Keyed<IPlayEvent>(typeof(OutcomeCreated).Name);
            containerBuilder.RegisterType<PlayOutcomeVersionCreated>()
                            .Keyed<IPlayEvent>(typeof(OutcomeVersionCreated).Name);
            containerBuilder.RegisterType<PlayOutcomeVersionPublished>()
                            .Keyed<IPlayEvent>(typeof(OutcomeVersionPublished).Name);

			containerBuilder.RegisterType<DomainEvents>().As<IDomainEvents>();

			containerBuilder.Register<IStoreEvents>(x => new CourseEventStore());

            //there must be an easier way using register generics
            containerBuilder.RegisterType<EventPersisterHandler>().As<IHandle<CourseAssociatedWithProgram>>();
            containerBuilder.RegisterType<EventPersisterHandler>().As<IHandle<CourseDisassociatedWithProgram>>();

            containerBuilder.RegisterType<UpdateModelOnCourseCreation>().As<IHandle<CourseCreated>>();
            containerBuilder.RegisterType<EventPersisterHandler>().As<IHandle<CourseCreated>>();
            
            containerBuilder.RegisterType<EventPersisterHandler>().As<IHandle<CourseInfoUpdated>>();

            containerBuilder.RegisterType<EventPersisterHandler>().As<IHandle<CourseSegmentAdded>>();
            containerBuilder.RegisterType<UpdateModelOnAddingCourseSegment>().As<IHandle<CourseSegmentAdded>>();
            containerBuilder.RegisterType<EventPersisterHandler>().As<IHandle<CourseSegmentUpdated>>();

			containerBuilder.RegisterType<UpdateModelOnCourseUpdating>().As<IHandle<CourseUpdated>>();
			containerBuilder.RegisterType<CourseUpdatedHandler>().As<IHandle<CourseUpdated>>();

			containerBuilder.RegisterType<UpdateModelOnCourseDeletion>().As<IHandle<CourseDeleted>>();
			containerBuilder.RegisterType<EventPersisterHandler>().As<IHandle<CourseDeleted>>();

            containerBuilder.RegisterType<EventPersisterHandler>().As<IHandle<CourseVersionCreated>>();
            containerBuilder.RegisterType<UpdateModelOnCourseVersionCreation>().As<IHandle<CourseVersionCreated>>();

            containerBuilder.RegisterType<EventPersisterHandler>().As<IHandle<CourseVersionPublished>>();
            containerBuilder.RegisterType<UpdateModelOnCourseVersionPublish>().As<IHandle<CourseVersionPublished>>();

            containerBuilder.RegisterType<UpdateModelOnOutcomeCreation>().As<IHandle<OutcomeCreated>>();
            containerBuilder.RegisterType<EventPersisterHandler>().As<IHandle<OutcomeCreated>>();

            containerBuilder.RegisterType<EventPersisterHandler>().As<IHandle<OutcomeVersionCreated>>();
            containerBuilder.RegisterType<UpdateModelOnOutcomeVersionCreation>().As<IHandle<OutcomeVersionCreated>>();

            containerBuilder.RegisterType<EventPersisterHandler>().As<IHandle<OutcomeVersionPublished>>();
            containerBuilder.RegisterType<UpdateModelOnOutcomeVersionPublished>().As<IHandle<OutcomeVersionPublished>>();


        }
    }
}
