using ValuedInBE.PersonalValues.Models.DTOs.Pocos;
using ValuedInBE.PersonalValues.Service;
using ValuedInBE.Users.Models.Entities;
using ValuedInBE.Users.Repositories;

namespace ValuedInBE.Users.Services.Implementations
{
    public class UserValueSevice : IUserValueService
    {
        private readonly IUserValueRepository _userValueRepository;

        public UserValueSevice( IUserValueRepository userValueRepository)
        {
            _userValueRepository = userValueRepository;
        }

        public async Task AddValueToUserAsync(string userId, long valueId)
        {
            UserValue userValue = new()
            {
                UserId = userId,
                ValueId = valueId
            };
            await _userValueRepository.CreateUserValueAsync(userValue);
        }

        public async Task<IEnumerable<IdAndValue>> GetUserValuesAsync(string userId)
        {
            IEnumerable<UserValue> userValues = await _userValueRepository.GetAllUserPersonalValuesForUserAsync(userId);
            return userValues.Select(uv => new IdAndValue() { Id = uv.ValueId, Value = uv.Value!.Name });
        }

        public async Task RemoveValueFromUserAsync(string userId, long valueId)
        {
            UserValue userValue = new()
            {
                UserId = userId,
                ValueId = valueId
            };
            await _userValueRepository.RemoveUserValueAsync(userValue);

        }
    }
}
