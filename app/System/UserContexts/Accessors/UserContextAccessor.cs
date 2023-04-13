using ValuedInBE.System.Exceptions;
using ValuedInBE.Users.Models;

namespace ValuedInBE.System.UserContexts.Accessors
{
    public class UserContextAccessor : IUserContextAccessor
    {

        private UserContext? _userContext = null;

        public bool Exists { get => _userContext != null;  }

        public UserContext UserContext { 
            get => _userContext ?? throw new UserContextMissingException(); 
            set => _userContext = value; 
        }
    }
}
