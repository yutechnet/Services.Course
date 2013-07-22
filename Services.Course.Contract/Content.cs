using System;

namespace BpeProducts.Services.Course.Contract
{
    [Serializable]
    public class Content
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
    }
}