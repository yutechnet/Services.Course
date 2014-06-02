using System.Web.Http;
using Autofac;
using BpeProducts.Common.Ioc;
using BpeProducts.Common.NServiceBus;
using BpeProducts.Common.WebApi;
using BpeProducts.Services.Course.Host.App_Start;
using NServiceBus;

namespace BpeProducts.Services.Course.Host
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var container = ContainerInstanceProvider.GetContainerInstance();
            ConfigureWebApi(GlobalConfiguration.Configuration, container);

			NServiceBusConfiguration.ConfigureNServiceBus(container, configure =>
			{
				//uncomment this and run if you get table creation error. TODO: fix it
				//Configure.Instance.ForInstallationOn<Windows>().Install();
				configure.SendOnly();

			});
        }

        public static void ConfigureWebApi(HttpConfiguration httpConfiguration, IContainer container)
        {
            AttributeRoutingHttpConfig.RegisterRoutes(httpConfiguration);
            Configuration.Configure(httpConfiguration, container);
        }
    }
}