using System.Net;

namespace ValuedInBE.System.Exceptions
{
    public interface IHttpStatusException
    {
        HttpStatusCode StatusCode { get; set; }
        string Message { get; }

    }
}
