using System;
using System.Runtime.Serialization;

namespace AdvertApi.Services
{
    [Serializable]
    internal class NotEnoughBuildingsException : Exception
    {
        public NotEnoughBuildingsException()
        {
        }

        public NotEnoughBuildingsException(string message) : base(message)
        {
        }

        public NotEnoughBuildingsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotEnoughBuildingsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}