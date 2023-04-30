using ValuedInBE.PersonalValues.Models.Entities;
using ValuedInBE.Users.Models.Entities;

namespace ValuedInBE.PersonalValues.Repositories
{
    public interface IPersonalValuesRepository
    {
        Task<IEnumerable<PersonalValue>> GetAllPersonalValuesAsync();
        Task<IEnumerable<UserValue>> GetUserValuesWithDataAsync(string userId);
        Task<PersonalValue?> GetPersonalValueByIdAsync(long valueId);
        Task CreateValueAsync(PersonalValue value);
        Task UpdateValueAsync(PersonalValue value);
        Task CreateValueGroupAsync(PersonalValueGroup valueGroup);
        Task<PersonalValueGroup?> GetAllPersonalValuesByGroupIdAsync(long groupId);
    }
}
