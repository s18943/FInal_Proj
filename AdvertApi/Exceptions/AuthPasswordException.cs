using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertApi.Exceptions
{
    public class AuthPasswordException : Exception
    {
        public AuthPasswordException()
        {
        }

        public AuthPasswordException(string message) : base(message)
        {
        }

        public AuthPasswordException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
