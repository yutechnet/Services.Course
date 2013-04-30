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
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static void ConfigureWebApi(HttpConfiguration configuration)
        {
	        var container = ContainerInstanceProvider.GetContainerInstance();
			WebApiConfig.Register(configuration);
            Configuration.Configure(configuration,container);		
            MapperConfig.ConfigureMappers();
        }

        protected void Application_Start()
        {			
            ConfigureWebApi(GlobalConfiguration.Configuration);
        }
    }
}