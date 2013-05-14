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

namespace BpeProducts.Services.Course.Domain
{
    public class IocRegistrations : IRegister
    {
        public void Register(ContainerBuilder containerBuilder)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            Common.NHibernate.IocRegistrations.RegisterSessionFactory(containerBuilder, connectionString, dropSchema: false, updateSchema: true);

            containerBuilder
                .RegisterType<CourseRepository>().As<ICourseRepository>()
                .EnableInterfaceInterceptors().EnableValidation()
                .InterceptedBy(typeof(PublicInterfaceLoggingInterceptor));

            containerBuilder.RegisterType<DomainEvents>().As<IDomainEvents>();

            //there must be an easier way using register generics
            containerBuilder.RegisterType<CourseEventPersisterHandler>().As<IHandle<CourseAssociatedWithProgram>>();
            containerBuilder.RegisterType<CourseEventPersisterHandler>().As<IHandle<CourseDisassociatedWithProgram>>();
            containerBuilder.RegisterType<CourseEventPersisterHandler>().As<IHandle<CourseCreated>>();
            containerBuilder.RegisterType<CourseEventPersisterHandler>().As<IHandle<CourseInfoUpdated>>();
            containerBuilder.RegisterType<CourseEventPersisterHandler>().As<IHandle<CourseSegmentAdded>>();
            containerBuilder.RegisterType<CourseEventPersisterHandler>().As<IHandle<CourseSegmentUpdated>>();
           
            containerBuilder.RegisterType<CourseUpdatedHandler>().As<IHandle<CourseUpdated>>();


        }
    }

    
}
