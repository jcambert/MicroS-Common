using System;

namespace MicroS_Common.Repository
{
    [AttributeUsage(AttributeTargets.Class,Inherited =false)]
    public class RepositoryAttribute:Attribute
    {
        public RepositoryAttribute(Type type)
        {
            this.Type = type;
        }

        public Type Type { get; }
    }
}
