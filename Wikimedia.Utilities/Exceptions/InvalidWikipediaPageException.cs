using System;
using System.Runtime.Serialization;

namespace Wikimedia.Utilities.Exceptions
{
    [Serializable]
    public class InvalidWikipediaPageException : Exception
    {
        public InvalidWikipediaPageException()
        {
        }

        public InvalidWikipediaPageException(string message) : base(message)
        {
        }

        public InvalidWikipediaPageException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidWikipediaPageException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
