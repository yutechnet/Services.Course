using Autofac;
using Autofac.Extras.DynamicProxy2;
using BpeProducts.Common.Authorization;
using BpeProducts.Common.Ioc;
using BpeProducts.Common.Ioc.Validation;
using BpeProducts.Common.Log;
using BpeProducts.Services.Course.Domain;
using BpeProducts.Services.Section.Contracts;

namespace BpeProducts.Services.Course.Host
{
    public class IocRegistrations : IRegister
    {
        public void Register(ContainerBuilder containerBuilder)
        {
            Common.WebApi.IocRegistrations.RegisterWebApi(containerBuilder);

            containerBuilder.RegisterType<SectionClient>()
                            .As<ISectionClient>()
                            .EnableInterfaceInterceptors()
                            .EnableValidation()
                            .EnableLogging();

            containerBuilder.RegisterType<BootstrapContextSamlTokenExtractor>().As<ISamlTokenExtractor>();
        }
    }
}