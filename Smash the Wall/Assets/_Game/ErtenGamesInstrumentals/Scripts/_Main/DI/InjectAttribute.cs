using System;

namespace DependencyInjection
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    public class InjectAttribute : Attribute
    {
        public string Id { get; }

        public InjectAttribute(string id = "")
        {
            Id = id;
        }
    }
}