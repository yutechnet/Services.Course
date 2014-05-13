using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Common.WebApiTest.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Integration.Resources
{
    public static class ResourceFactory
    {
        public static IResource Get(string resourceType, string resourceName)
        {
            var type = resourceType.Trim().ToLower();

            switch (type)
            {
                case "course":
                    return Resources<CourseResource>.Get(resourceName);
                case "program":
                    return Resources<ProgramResource>.Get(resourceName);
            }

            throw new ApplicationException(string.Format("Unknow Resource type '{0}'. Add a case statement to the factory.", resourceType));
        }
    }
}
