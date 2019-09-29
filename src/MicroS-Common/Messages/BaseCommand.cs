using Newtonsoft.Json;
using System;

namespace MicroS_Common.Messages
{
    public abstract class BaseCommand:ICommand
    {
        public abstract Guid Id { get;}

        [JsonConstructor]
        public BaseCommand()
        {
        }
    }
}
