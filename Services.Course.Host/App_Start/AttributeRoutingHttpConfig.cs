using System.Web.Http;

namespace BpeProducts.Services.Course.Host.App_Start 
{
    public static class AttributeRoutingHttpConfig
	{
        public static void RegisterRoutes(HttpConfiguration httpConfiguration)
        {
            httpConfiguration.MapHttpAttributeRoutes();
        }
    }
}
