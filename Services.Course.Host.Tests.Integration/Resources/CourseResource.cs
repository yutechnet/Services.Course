﻿using System;

namespace BpeProducts.Services.Course.Host.Tests.Integration.Resources
{
    public class CourseResource : IResource
    {
        public Guid Id { get; set; }
        public Uri ResourceUri { get; set; }
    }
}