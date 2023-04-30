using Microsoft.EntityFrameworkCore;
using ValuedInBE.System.PersistenceLayer.Contexts;
using ValuedInBE.System.PersistenceLayer.Extensions;
using ValuedInBE.System.UserContexts.Accessors;
using ValuedInBE.Users.Models;
using ValuedInBE.Users.Models.Entities;

namespace ValuedInBE.Users.Repositories
{
    public class UserValueRepository : IUserValueRepository
    {
        private readonly ValuedInContext _valuedInContext;
        private readonly IUserContextAccessor _userContextAccessor;

        public UserValueRepository(ValuedInContext valuedInContext, IUserContextAccessor userContextAccessor)
        {
            _valuedInContext = valuedInContext;
            _userContextAccessor = userContextAccessor;
        }

        public async Task CreateUserValueAsync(UserValue userValue)
        {
            _valuedInContext.UserValues.Add(userValue);
            CheckEntityAuditing();
            await _valuedInContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserValue>> GetAllUserPersonalValuesForUserAsync(string userId)
        {
            var values = from uv in _valuedInContext.UserValues.Include(uv => uv.Value)
                         where uv.UserId == userId
                         select uv;
            await values.LoadAsync();
            return values;
        }                

        public async Task RemoveUserValueAsync(UserValue userValue)
        {
            _valuedInContext.UserValues.Remove(userValue);
            CheckEntityAuditing();
            await _valuedInContext.SaveChangesAsync(); 
        }

        private void CheckEntityAuditing()
        {
            UserContext userContext = _userContextAccessor.UserContext;
            _valuedInContext.ChangeTracker.CheckAuditing(userContext);
        }
    }
}
