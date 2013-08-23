using System;

namespace BpeProducts.Services.Course.Host.Tests.Integration.Resources
{
    public class OrganizationResource : IResource
    {
        public Guid Id { get; set; }
        public Uri ResourceUri { get; set; }
    }

    public class RoleResource : IResource
    {
        public Guid Id { get; set; }
        public Uri ResourceUri { get; set; }
    }
}