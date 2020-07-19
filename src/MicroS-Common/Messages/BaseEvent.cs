using System;
namespace MicroS_Common.Messages
{
    public abstract class BaseEvent<TKey> : IEvent
    {
        public virtual TKey Id { get; }
    }
    public abstract class BaseEvent :BaseEvent<Guid>
    {
    }
}
