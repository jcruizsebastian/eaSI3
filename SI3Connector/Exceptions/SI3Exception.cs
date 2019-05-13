using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SI3Connector.Exceptions
{
    public class SI3Exception : Exception
    {
        public IEnumerable<string> errors;

        public SI3Exception(string message) : base(message)
        {
        }
        public SI3Exception(IEnumerable<string> errors)
        {
            this.errors = errors;
        }
        public SI3Exception(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SI3Exception(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
