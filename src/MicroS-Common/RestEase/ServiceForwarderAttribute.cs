using System;

namespace MicroS_Common.RestEase
{
    [AttributeUsage(AttributeTargets.Interface,AllowMultiple =false,Inherited =true)]
    public class ServiceForwarderAttribute:Attribute
    {
        /// <summary>
        /// Service Forwarder @see RestEase, Fabio, Consul
        /// </summary>
        /// <param name="serviceName">the name pust be appear in appsettings.json file restease section</param>
        public ServiceForwarderAttribute(string serviceName)
        {
            this.Name = serviceName;
        }

        public string Name { get; }
    }
}
