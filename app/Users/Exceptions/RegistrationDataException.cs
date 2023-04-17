using System.Net;
using ValuedInBE.System.Exceptions;

namespace ValuedInBE.Users.Exceptions
{
    public class RegistrationDataException : HttpStatusCarryingException
    {
        private const HttpStatusCode statusCode = HttpStatusCode.UnprocessableEntity;
        public RegistrationDataException(string? message) : base(message, statusCode)
        {
        }

    }
}
