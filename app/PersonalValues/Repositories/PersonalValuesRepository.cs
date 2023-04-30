using Microsoft.EntityFrameworkCore;
using ValuedInBE.PersonalValues.Models.Entities;
using ValuedInBE.System.PersistenceLayer.Contexts;
using ValuedInBE.System.PersistenceLayer.Extensions;
using ValuedInBE.System.UserContexts.Accessors;
using ValuedInBE.Users.Models;
using ValuedInBE.Users.Models.Entities;

namespace ValuedInBE.PersonalValues.Repositories
{
    public class PersonalValuesRepository : IPersonalValuesRepository
    {
        private readonly ValuedInContext _context;
        private readonly IUserContextAccessor _userContextAccessor;

        public PersonalValuesRepository(ValuedInContext valuedInContext, IUserContextAccessor userContextAccessor)
        {
            _context = valuedInContext;
            _userContextAccessor = userContextAccessor;
        }

        public async Task CreateValueAsync(PersonalValue value)
        {
            _context.Values.Add(value);
            CheckEntityAuditing();
            await _context.SaveChangesAsync();
        }

        public async Task CreateValueGroupAsync(PersonalValueGroup valueGroup)
        {

            _context.ValueGroups.Add(valueGroup);
            CheckEntityAuditing();
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PersonalValue>> GetAllPersonalValuesAsync()
        {
            var allValues = from v in _context.Values select v;
            await allValues.LoadAsync();
            return allValues;
        }

        public async Task<PersonalValueGroup?> GetAllPersonalValuesByGroupIdAsync(long groupId)
        {
            var personalValueGroup = from g in _context.ValueGroups.Include(g => g.PersonalValues)
                                          where g.Id == groupId
                                          select g;

            return await personalValueGroup.FirstOrDefaultAsync();

        }

        public async Task<PersonalValue?> GetPersonalValueByIdAsync(long valueId)
        {
            var personalValuesById = from v in _context.Values 
                                     where v.Id == valueId
                                     select v;
            return await personalValuesById.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UserValue>> GetUserValuesWithDataAsync(string userId)
        {
            var userValues = from v in _context.UserValues.Include(u => u.Value)
                             where v.UserId == userId
                             select v;

            await userValues.LoadAsync();
            return userValues;
        }

        public async Task UpdateValueAsync(PersonalValue value)
        {
            _context.Update(value);
            CheckEntityAuditing();
            await _context.SaveChangesAsync();
        }

        private void CheckEntityAuditing()
        {
            UserContext userContext = _userContextAccessor.UserContext;
            _context.ChangeTracker.CheckAuditing(userContext);
        }
    }
}
