using System;
using BpeProducts.Common.WebApiTest.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Integration.Resources.Account
{
    public class OrganizationResource : IResource
    {
        public Guid Id { get; set; }
        public Uri ResourceUri { get; set; }
    }
}