using System;
using System.Runtime.Serialization;

namespace MicroS_Common.Consul
{
    internal class ConsulServiceNotFoundException : Exception
    {
        public string ServiceName { get; set; }

        public ConsulServiceNotFoundException(string serviceName) : this(string.Empty, serviceName)
        {
        }

        public ConsulServiceNotFoundException(string message, string serviceName) : base(message)
        {
            ServiceName = serviceName;
        }
    }

}