using System.Net;
using ValuedInBE.System.Exceptions;

namespace ValuedInBE.Chats.Exceptions
{
    public class ChatParticipantsMissingException : HttpStatusCarryingException
    {
        private const HttpStatusCode statusCode = HttpStatusCode.UnprocessableEntity;
        public ChatParticipantsMissingException(string? message) : base(message, statusCode) { }
    }
}
