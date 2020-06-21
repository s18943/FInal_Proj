using System;
using System.Runtime.Serialization;

namespace AdvertApi.Services
{
    [Serializable]
    internal class RefreshTokenIncorrectException : Exception
    {
        public RefreshTokenIncorrectException()
        {
        }

        public RefreshTokenIncorrectException(string message) : base(message)
        {
        }

        public RefreshTokenIncorrectException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RefreshTokenIncorrectException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}