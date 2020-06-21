using System;
using System.Runtime.Serialization;

namespace AdvertApi.Services
{
    [Serializable]
    internal class BuildingsTooFarException : Exception
    {
        public BuildingsTooFarException()
        {
        }

        public BuildingsTooFarException(string message) : base(message)
        {
        }

        public BuildingsTooFarException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BuildingsTooFarException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}