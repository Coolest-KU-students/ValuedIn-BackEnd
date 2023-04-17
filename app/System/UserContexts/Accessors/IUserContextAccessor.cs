using ValuedInBE.Users.Models;

namespace ValuedInBE.System.UserContexts.Accessors
{
    public interface IUserContextAccessor
    {
        UserContext UserContext { get; set; }
        bool Exists { get; }
    }
}
