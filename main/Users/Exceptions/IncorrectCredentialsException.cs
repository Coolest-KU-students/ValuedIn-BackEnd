using System.Net;
using ValuedInBE.System.Exceptions;

namespace ValuedInBE.Users.Exceptions
{
    public class IncorrectCredentialsException : Exception, IHttpStatusException
    {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.Unauthorized;
        public IncorrectCredentialsException(string? message) : base(message)
        {
        }

        public IncorrectCredentialsException() : base("Incorrect Credentials") { }


    }
}
