using System;

namespace MicroS_Common.Types
{
    public class NotFoundException : MicroSException
    {
        public NotFoundException()
        {
        }

        public NotFoundException(string code) : base(code)
        {
        }

        public NotFoundException(string message, params object[] args)
            : this(string.Empty, message, args)
        {
        }

        public NotFoundException(string code, string message, params object[] args)
            : this(null, code, message, args)
        {
        }

        public NotFoundException(Exception innerException, string message, params object[] args)
            : this(innerException, string.Empty, message, args)
        {
        }

        public NotFoundException(Exception innerException, string code, string message, params object[] args)
            : base(code, string.Format(message, args), innerException)
        {
        }
    }
}
