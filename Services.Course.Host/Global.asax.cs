using System;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Tracing;
using System.Web.Mvc;
using System.Web.Routing;
using BpeProducts.Common.WebApi;
using BpeProducts.Services.Course.Host.App_Start;

namespace BpeProducts.Services.Course.Host
{
    public class SimpleTracer : ITraceWriter
    {
        public void Trace(HttpRequestMessage request, string category, TraceLevel level,
            Action<TraceRecord> traceAction)
        {
            var rec = new TraceRecord(request, category, level);
            traceAction(rec);
            WriteTrace(rec);
        }

        protected void WriteTrace(TraceRecord rec)
        {
            var message = string.Format("{0};{1};{2}",
                rec.Operator, rec.Operation, rec.Message);
            System.Diagnostics.Trace.WriteLine(message, rec.Category);
        }
    }

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static void ConfigureWebApi(HttpConfiguration configuration)
        {
            configuration.Services.Replace(typeof(ITraceWriter), new SimpleTracer());
            System.Diagnostics.Trace.TraceInformation("Entering ConfigureWebApi");
            WebApiConfig.Register(configuration);
            Configuration.Configure(configuration);
            MapperConfig.ConfigureMappers();
            System.Diagnostics.Trace.TraceInformation("Exiting ConfigureWebApi");
        }

        protected void Application_Start()
        {
            System.Diagnostics.Trace.TraceInformation("Entering Application_Start");
            ConfigureWebApi(GlobalConfiguration.Configuration);
            System.Diagnostics.Trace.TraceInformation("Exiting Application_Start");
        }
    }
}