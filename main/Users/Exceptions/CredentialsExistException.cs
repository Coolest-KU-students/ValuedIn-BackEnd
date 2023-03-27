using System.Net;
using ValuedInBE.System.Exceptions;

namespace ValuedInBE.Users.Exceptions
{
    public class CredentialsExistException : Exception, IHttpStatusException
    {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.UnprocessableEntity;

        public CredentialsExistException(string credentialType, string? existingValue) 
            : base($"{credentialType} {$"'{existingValue}' " ?? ""}is already taken") { }

    }
}
