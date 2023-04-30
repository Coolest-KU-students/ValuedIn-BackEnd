using ValuedInBE.PersonalValues.Models.DTOs.Pocos;

namespace ValuedInBE.Users.Services
{
    public interface IUserValueService
    {
        Task<IEnumerable<IdAndValue>> GetUserValuesAsync(string userId);

        Task AddValueToUserAsync(string userId, long valueId);

        Task RemoveValueFromUserAsync(string userId, long valueId);


    }
}
