using System;
using System.Collections.Generic;
using System.Text;

namespace MicroS_Common.Mvc
{
    public sealed class ServiceId : IServiceId
    {
        private static readonly string UniqueId = $"{Guid.NewGuid():N}";

        public string Id => UniqueId;
    }
}
