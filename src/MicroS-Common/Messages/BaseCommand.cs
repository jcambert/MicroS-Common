using Newtonsoft.Json;
using System;
using VaultSharp.V1.SecretsEngines.PKI;

namespace MicroS_Common.Messages
{
    public abstract class BaseCommand<TKey> : ICommand
    {
        [JsonConstructor]
        public BaseCommand()
        {

        }
        public abstract TKey Id { get; set; }

        public Type KeyType => typeof(TKey);
    }
    public abstract class BaseCommand :BaseCommand<Guid>
    {
        /// <summary>
        /// Do not Remove
        /// It's needed by Bind Method to set the Id of Model
        /// </summary>
        public override Guid Id { get; set; }

        [JsonConstructor]
        public BaseCommand()
        {
        }
    }
}
