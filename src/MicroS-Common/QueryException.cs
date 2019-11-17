using System;

namespace MicroS_Common
{
    public class QueryException : Exception
    {
        public QueryException(string message) : base(message)
        {
        }
    }
}
