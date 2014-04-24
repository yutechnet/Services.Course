using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Common.WebApiTest;
using Owin;

namespace BpeProducts.Services.Course.Host.Tests.Integration
{
    public class Startup : StartupBase
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            Configuration(appBuilder, WebApiApplication.ConfigureWebApi);
        }
    }
}
