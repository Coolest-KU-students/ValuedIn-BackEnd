using System.Net;
using ValuedInBE.System.Exceptions;

namespace ValuedInBE.Users.Exceptions
{
    public class CredentialsExistException : HttpStatusCarryingException
    {
        private const HttpStatusCode statusCode = HttpStatusCode.UnprocessableEntity;

        public CredentialsExistException(string credentialType, string? existingValue) 
            : base($"{credentialType} {$"'{existingValue}' " ?? ""}is already taken", statusCode) { }

    }
}
