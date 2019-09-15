using System;
using System.Collections.Generic;
using System.Text;

namespace MicroS_Common.Types
{
    public class ValidationException:MicroSException
    {
        public ValidationException()
        {
        }

        public ValidationException(string code):base(code)
        {
        }

        public ValidationException(string message, params object[] args)
            : this(string.Empty, message, args)
        {
        }

        public ValidationException(string code, string message, params object[] args)
            : this(null, code, message, args)
        {
        }

        public ValidationException(Exception innerException, string message, params object[] args)
            : this(innerException, string.Empty, message, args)
        {
        }

        public ValidationException(Exception innerException, string code, string message, params object[] args)
            : base(code,string.Format(message, args), innerException)
        {
        }
    }
}
