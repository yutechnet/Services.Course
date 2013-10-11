using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using Autofac.Extras.DynamicProxy2;
using BpeProducts.Common.Authorization;
using BpeProducts.Common.Ioc;
using BpeProducts.Common.Log;
using BpeProducts.Services.Course.Domain;
using BpeProducts.Services.Course.Domain.Repositories;
using BpeProducts.Services.Course.Host.TempSectionContracts;

namespace BpeProducts.Services.Course.Host
{
    public class IocRegistrations : IRegister
    {
        public void Register(ContainerBuilder containerBuilder)
        {
            Common.WebApi.IocRegistrations.RegisterWebApi(containerBuilder);
			
            containerBuilder.RegisterType<SectionClient>().As<ISectionClient>()
                .EnableInterfaceInterceptors().EnableValidation()
                .InterceptedBy(typeof(PublicInterfaceLoggingInterceptor));

            containerBuilder.RegisterType<BootstrapContextSamlTokenExtractor>().As<ISamlTokenExtractor>();
        }
    }
}