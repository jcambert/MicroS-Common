using MicroS_Common.Messages;
using System;

namespace MicroS_Common.Domain
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited =false)]
    public class OnRejectedAttribute:Attribute
    {
        public OnRejectedAttribute(Type type)
        {
            var isubtype=type.IsSubclassOf(typeof(BaseRejectedEvent));
            if (!isubtype)
                throw new ArgumentException($"OnRejectedAttribute:{type.Name} is not a subclass of {typeof(BaseRejectedEvent).Name}");
            this.Type = type;
        }

        public Type Type { get; }
    }
}
