using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using BpeProducts.Common.WebApi;
using BpeProducts.Services.Course.Host.App_Start;

namespace BpeProducts.Services.Course.Host
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static void ConfigureWebApi(HttpConfiguration configuration)
        {
            WebApiConfig.Register(configuration);
            Configuration.Configure(configuration);
            MapperConfig.ConfigureMappers();
        }

        protected void Application_Start()
        {
            ConfigureWebApi(GlobalConfiguration.Configuration);
        }
    }
}