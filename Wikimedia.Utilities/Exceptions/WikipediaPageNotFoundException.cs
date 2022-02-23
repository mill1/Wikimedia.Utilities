using System;
using System.Runtime.Serialization;

namespace Wikimedia.Utilities.Exceptions
{
    [Serializable]
    public class WikipediaPageNotFoundException : Exception
    {
        public WikipediaPageNotFoundException()
        {
        }

        public WikipediaPageNotFoundException(string message) : base(message)
        {
        }

        public WikipediaPageNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WikipediaPageNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}