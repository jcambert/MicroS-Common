using System;

namespace MicroS_Common
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DisableSearchFilterAttribute : Attribute
    {
    }
}
