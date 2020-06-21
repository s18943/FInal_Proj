using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertApi.Exceptions
{
    public class AuthLoginException : Exception
    {
        public AuthLoginException()
        {
        }

        public AuthLoginException(string message) : base(message)
        {
        }

        public AuthLoginException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
