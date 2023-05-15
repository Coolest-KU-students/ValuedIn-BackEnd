using System.Net;
using ValuedInBE.System.Exceptions;

namespace ValuedInBE.PersonalValues.Exceptions
{
    public class ValueNotFoundException : HttpStatusCarryingException
    {
        private const HttpStatusCode statusCode = HttpStatusCode.NotFound;
        private const string formattableErrorMessage = "Value with id '{0}' was not found";
        public ValueNotFoundException(long valueId) : base(string.Format(formattableErrorMessage, valueId), statusCode)
        {
        }
    }
}
