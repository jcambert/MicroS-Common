using System;

namespace MicroS_Common.Types
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false,
                   Inherited = true)]
    public class UniqueAttribute : Attribute
    {
    }
}
