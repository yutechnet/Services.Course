using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using BpeProducts.Common.Ioc;

namespace BpeProducts.Services.Course.Host
{
    public class IocRegistrations : IRegister
    {
        public void Register(ContainerBuilder containerBuilder)
        {
            Common.WebApi.IocRegistrations.RegisterWebApi(containerBuilder);
			
        }
    }
}