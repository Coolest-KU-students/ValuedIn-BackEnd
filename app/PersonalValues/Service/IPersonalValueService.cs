using ValuedInBE.PersonalValues.Models.DTOs.Requests;
using ValuedInBE.PersonalValues.Models.DTOs.Response;
using ValuedInBE.PersonalValues.Models.Entities;

namespace ValuedInBE.PersonalValues.Service
{
    public interface IPersonalValueService
    {
        Task CreateValueAsync(NewValue newValue);
        Task CreateValueGroupAsync(string name);
        Task<IEnumerable<PersonalValue>> GetAllValuesExceptUsers(string? search);
        Task<ValuesInGroup> GetValuesFromGroupAsync(long groupId);
        Task UpdateValueAsync(UpdatedValue updatedValue);
    }
}