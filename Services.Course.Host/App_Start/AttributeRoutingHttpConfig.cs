using System.Web.Http;
using AttributeRouting.Web.Http.WebHost;

//[assembly: WebActivator.PreApplicationStartMethod(typeof(BpeProducts.Services.Course.Host.AttributeRoutingHttpConfig), "Start")]

namespace BpeProducts.Services.Course.Host 
{
    public static class AttributeRoutingHttpConfig
	{
        public static void RegisterRoutes(HttpConfiguration httpConfiguration)
        {
            // See http://github.com/mccalltd/AttributeRouting/wiki for more options.
            // To debug routes locally using the built in ASP.NET development server, go to /routes.axd

            httpConfiguration.Routes.MapHttpAttributeRoutes(config =>
            {
                config.AutoGenerateRouteNames = true;
                config.InMemory = true;
                config.AddRoutesFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());
            });
        }
    }
}
