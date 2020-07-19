using System;

namespace MicroS_Common.Types
{
    public interface ICreatable
    {
        DateTime CreatedDate { get; set; }
    }
    public interface IUpdatable
    {
        DateTime UpdatedDate { get; set; }
    }
}
