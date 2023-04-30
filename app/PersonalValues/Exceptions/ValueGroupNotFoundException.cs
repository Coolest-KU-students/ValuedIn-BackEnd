using System.Net;
using ValuedInBE.System.Exceptions;

namespace ValuedInBE.PersonalValues.Exceptions
{
    public class ValueGroupNotFoundException : HttpStatusCarryingException
    {
        private const HttpStatusCode statusCode = HttpStatusCode.NotFound;
        private const string formattableErrorMessage = "Value group with id '{0}' was not found";
        public ValueGroupNotFoundException(long groupId) : base(string.Format(formattableErrorMessage, groupId), statusCode)
        {
        }
    }
}
