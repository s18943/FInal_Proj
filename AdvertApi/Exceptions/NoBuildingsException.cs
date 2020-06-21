using System;
using System.Runtime.Serialization;

namespace AdvertApi.Services
{
    [Serializable]
    internal class NoBuildingsException : Exception
    {
        public NoBuildingsException()
        {
        }

        public NoBuildingsException(string message) : base(message)
        {
        }

        public NoBuildingsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoBuildingsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}