using System.Net;
using ValuedInBE.System.Exceptions;

namespace ValuedInBE.Chats.Exceptions
{
    public class ChatParticipantsMissingException : Exception, IHttpStatusException
    {
        public ChatParticipantsMissingException(string? message) : base(message)
        {
        }

        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.UnprocessableEntity;


    }
}
