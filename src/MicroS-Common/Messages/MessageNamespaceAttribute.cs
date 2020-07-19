using System;

namespace MicroS_Common.Messages
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple = false,Inherited = true)]
    public class MessageNamespaceAttribute : Attribute
    {
        public string Namespace { get; }

        public MessageNamespaceAttribute(string @namespace)
        {
            Namespace = @namespace?.ToLowerInvariant();
        }
    }
}
