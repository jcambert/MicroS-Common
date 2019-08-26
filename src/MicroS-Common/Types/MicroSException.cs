using System;

namespace MicroS_Common.Types
{
    public class MicroSException:Exception
    {
        public string Code { get; }

        public MicroSException()
        {
        }

        public MicroSException(string code)
        {
            Code = code;
        }

        public MicroSException(string message, params object[] args)
            : this(string.Empty, message, args)
        {
        }

        public MicroSException(string code, string message, params object[] args)
            : this(null, code, message, args)
        {
        }

        public MicroSException(Exception innerException, string message, params object[] args)
            : this(innerException, string.Empty, message, args)
        {
        }

        public MicroSException(Exception innerException, string code, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
            Code = code;
        }
    }
}
