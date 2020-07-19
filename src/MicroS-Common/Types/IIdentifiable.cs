using System;

namespace MicroS_Common.Types
{
    public interface IIdentifiable:IIdentifiable<Guid>
    {}

    public interface IIdentifiable<T>
    {
        T Id { get; }
    }
}
