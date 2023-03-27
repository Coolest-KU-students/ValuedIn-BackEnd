using System.Net;
using ValuedInBE.System.Exceptions;

namespace ValuedInBE.Users.Exceptions
{
    public class RegistrationDataException : Exception, IHttpStatusException
    {
        public RegistrationDataException(string? message) : base(message)
        {
        }

        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.UnprocessableEntity;
    }
}
