using System;
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
using BpeProducts.Services.Course.Domain.Repositories;
using EventStore;

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

            containerBuilder
                .RegisterType<CourseRepository>().As<ICourseRepository>()
                .EnableInterfaceInterceptors().EnableValidation()
                .InterceptedBy(typeof(PublicInterfaceLoggingInterceptor));
            containerBuilder
                .RegisterType<LearningOutcomeRepository>().As<ILearningOutcomeRepository>()
                .EnableInterfaceInterceptors().EnableValidation()
                .InterceptedBy(typeof(PublicInterfaceLoggingInterceptor));


            containerBuilder.RegisterType<CourseEventStore>().As<IStoreCourseEvents>();
			
			containerBuilder.RegisterType<CourseFactory>().As<ICourseFactory>();
				

			containerBuilder.RegisterType<PlayCourseCreated>().Keyed<IPlayEvent>(typeof(CourseCreated).Name);
			containerBuilder.RegisterType<PlayCourseAssociatedWithProgram>().Keyed<IPlayEvent>(typeof(CourseAssociatedWithProgram).Name);
			containerBuilder.RegisterType<PlayCourseDeleted>().Keyed<IPlayEvent>(typeof(CourseDeleted).Name);
			containerBuilder.RegisterType<PLayCourseDisassociatedWithProgram>().Keyed<IPlayEvent>(typeof(CourseDisassociatedWithProgram).Name);
			containerBuilder.RegisterType<PlayCourseInfoUpated>().Keyed<IPlayEvent>(typeof(CourseInfoUpdated).Name);
			containerBuilder.RegisterType<PLayCourseSegmentAdded>().Keyed<IPlayEvent>(typeof(CourseSegmentAdded).Name);
			containerBuilder.RegisterType<PlayCourseSegmentUpdated>().Keyed<IPlayEvent>(typeof(CourseSegmentUpdated).Name);
			 

			containerBuilder.RegisterType<DomainEvents>().As<IDomainEvents>();

			containerBuilder.Register<IStoreEvents>(x => new CourseEventStore());

            //there must be an easier way using register generics
            containerBuilder.RegisterType<CourseEventPersisterHandler>().As<IHandle<CourseAssociatedWithProgram>>();
            containerBuilder.RegisterType<CourseEventPersisterHandler>().As<IHandle<CourseDisassociatedWithProgram>>();

            containerBuilder.RegisterType<UpdateModelOnCourseCreation>().As<IHandle<CourseCreated>>();
            containerBuilder.RegisterType<CourseEventPersisterHandler>().As<IHandle<CourseCreated>>();
            
            containerBuilder.RegisterType<CourseEventPersisterHandler>().As<IHandle<CourseInfoUpdated>>();

            containerBuilder.RegisterType<CourseEventPersisterHandler>().As<IHandle<CourseSegmentAdded>>();
            containerBuilder.RegisterType<UpdateModelOnAddingCourseSegment>().As<IHandle<CourseSegmentAdded>>();
            
            containerBuilder.RegisterType<CourseEventPersisterHandler>().As<IHandle<CourseSegmentUpdated>>();

			containerBuilder.RegisterType<UpdateModelOnCourseUpdating>().As<IHandle<CourseUpdated>>();
			containerBuilder.RegisterType<CourseUpdatedHandler>().As<IHandle<CourseUpdated>>();


			containerBuilder.RegisterType<UpdateModelOnCourseDeletion>().As<IHandle<CourseDeleted>>();
			containerBuilder.RegisterType<CourseEventPersisterHandler>().As<IHandle<CourseDeleted>>();
            

            


        }
    }

    
}
