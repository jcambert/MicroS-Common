﻿using RawRabbit.Configuration;

namespace MicroS_Common.RabbitMq
{
    public sealed class RabbitMqOptions : RawRabbitConfiguration
    {
        public const string SECTION = "rabbitMq";
        public string Namespace { get; set; }
        public int Retries { get; set; }
        public int RetryInterval { get; set; }
    }
}
