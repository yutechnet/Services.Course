﻿using System;

namespace BpeProducts.Services.Course.Host.Tests.Integration.Resources
{
    public class CourseSegmentResource : IResource
    {
        public Guid Id { get; set; }
        public Uri ResourceUri { get; set; }
    }
}