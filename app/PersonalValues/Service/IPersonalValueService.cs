using ValuedInBE.PersonalValues.Models.DTOs.Requests;
using ValuedInBE.PersonalValues.Models.DTOs.Response;
using ValuedInBE.PersonalValues.Models.Entities;

namespace ValuedInBE.PersonalValues.Service
{
    public interface IPersonalValueService
    {
        Task CreateValue(NewValue newValue);
        Task CreateValueGroup(string name);
        Task<IEnumerable<PersonalValue>> GetAllValuesExceptUsers(string? search);
        Task<ValuesInGroup> GetValuesFromGroupAsync(long groupId);
        Task UpdateValue(UpdatedValue updatedValue);
    }
}