using System.Net;
using System.Runtime.Serialization;

namespace ValuedInBE.System.Exceptions
{
    public class HttpStatusCarryingException : Exception
    {
        public HttpStatusCode StatusCode { get; init; }

        public HttpStatusCarryingException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }


        public HttpStatusCarryingException(string? message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCarryingException(string? message, Exception? innerException, HttpStatusCode statusCode) : base(message, innerException)
        {
            StatusCode = statusCode;
        }

        protected HttpStatusCarryingException(SerializationInfo info, StreamingContext context, HttpStatusCode statusCode) : base(info, context)
        {
            StatusCode = statusCode;
        }


    }
}
