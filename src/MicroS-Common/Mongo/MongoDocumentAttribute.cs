using System;

namespace MicroS_Common.Mongo
{
    [AttributeUsage(AttributeTargets.Class,Inherited =true)]
    public class MongoDocumentAttribute:Attribute
    {
        public MongoDocumentAttribute(string name)
        {
            if (string.IsNullOrEmpty($"{nameof(name)} cannot be null nor empty to specify a mongo document")) ;
            this.Name = name;
        }

        public string Name { get; }
    }
}
