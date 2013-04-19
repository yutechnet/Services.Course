using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Http;

namespace BpeProducts.Services.Course.Host
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            Trace.TraceInformation("Entering Register");
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            Trace.TraceInformation("Exiting Register");
        }
    }
}
