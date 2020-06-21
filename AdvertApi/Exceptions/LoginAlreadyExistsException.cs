using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertApi.Exceptions
{
    public class LoginAlreadyExistsException : Exception
    {
        public LoginAlreadyExistsException()
        {
        }

        public LoginAlreadyExistsException(string message) : base(message)
        {
        }

        public LoginAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
