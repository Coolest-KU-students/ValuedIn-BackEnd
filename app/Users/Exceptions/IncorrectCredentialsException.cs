using System.Net;
using ValuedInBE.System.Exceptions;

namespace ValuedInBE.Users.Exceptions
{
    public class IncorrectCredentialsException : HttpStatusCarryingException
    {
        private const HttpStatusCode statusCode = HttpStatusCode.Unauthorized;

        public IncorrectCredentialsException(string? message) : base(message, statusCode)
        {
        }

        public IncorrectCredentialsException() : this("Incorrect Credentials") { }
    }
}
