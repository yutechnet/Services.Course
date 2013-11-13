using System.IdentityModel.Services;
using System.Security.Claims;
using System.Web.Http;
using Autofac;
using BpeProducts.Common.Ioc;
using BpeProducts.Common.WebApi;
using BpeProducts.Services.Course.Host.App_Start;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace BpeProducts.Services.Course.Host
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var container = ContainerInstanceProvider.GetContainerInstance();
            ConfigureWebApi(GlobalConfiguration.Configuration, container);
        }

        public static void ConfigureWebApi(HttpConfiguration httpConfiguration, IContainer container)
        {
            WebApiConfig.Register(httpConfiguration);
            MapperConfiguration.Configure();
            Configuration.Configure(httpConfiguration, container);
            AttributeRoutingHttpConfig.RegisterRoutes(httpConfiguration);
        }
    }
}