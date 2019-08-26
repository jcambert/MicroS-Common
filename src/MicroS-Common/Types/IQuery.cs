using System;
using System.Collections.Generic;
using System.Text;

namespace MicroS_Common.Types
{
    //Marker
    public interface IQuery
    {
    }

    public interface IQuery<T> : IQuery
    {
    }
}
