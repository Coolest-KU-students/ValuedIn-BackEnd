using System.Net;

namespace ValuedInBE.System.Exceptions
{
    public class UserContextMissingException : HttpStatusCarryingException
    {
        const string message = "User Context could not be accessed, might be an authentication error";
        const HttpStatusCode httpStatusCode = HttpStatusCode.Unauthorized;
        public UserContextMissingException() : base(message, httpStatusCode) { }

    }
}
