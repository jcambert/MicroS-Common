using Newtonsoft.Json;
using System;

namespace MicroS_Common.Messages
{
    public abstract class BaseCommand:ICommand
    {
        /// <summary>
        /// Do not Remove
        /// It's needed by Bind Method to set the Id of Model
        /// </summary>
        public abstract Guid Id { get;  set; }

        [JsonConstructor]
        public BaseCommand()
        {
        }
    }
}
