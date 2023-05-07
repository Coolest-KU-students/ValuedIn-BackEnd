using Microsoft.AspNetCore.Routing;
using ValuedInBE.PersonalValues.Exceptions;
using ValuedInBE.PersonalValues.Models.DTOs.Pocos;
using ValuedInBE.PersonalValues.Models.DTOs.Requests;
using ValuedInBE.PersonalValues.Models.DTOs.Response;
using ValuedInBE.PersonalValues.Models.Entities;
using ValuedInBE.PersonalValues.Repositories;
using ValuedInBE.System.UserContexts.Accessors;
using ValuedInBE.Users.Models;
using ValuedInBE.Users.Models.Entities;

namespace ValuedInBE.PersonalValues.Service
{
    public class PersonalValueService : IPersonalValueService
    {
        private const short defaultModifier = 1;
        private readonly IPersonalValuesRepository _personalValuesRepository;
        private readonly IUserContextAccessor _userContextAccessor;

        public PersonalValueService(IPersonalValuesRepository personalValuesRepository, IUserContextAccessor userContextAccessor)
        {
            _personalValuesRepository = personalValuesRepository;
            _userContextAccessor = userContextAccessor;
        }

        public async Task<IEnumerable<PersonalValue>> GetAllValuesExceptUsers(string? search)
        {
            UserContext userContext = _userContextAccessor.UserContext;
            //TODO: request Redis, if empty, then poke DB
            Task<IEnumerable<UserValue>> currentUserValuesTask = _personalValuesRepository.GetUserValuesWithDataAsync(userContext.UserID);
            IEnumerable<UserValue> userValues = await currentUserValuesTask;
            Task<IEnumerable<PersonalValue>> allPersonalValuesTask = _personalValuesRepository.GetAllPersonalValuesAsync();
            IEnumerable<PersonalValue> personalValues = await allPersonalValuesTask;

            IEnumerable<PersonalValue> filteredValues = personalValues.Where(p => !userValues.Any(u => u.ValueId == p.Id));

            if (string.IsNullOrEmpty(search)) return filteredValues;
            return filteredValues.Where(f => f.Name.Contains(search));
        }

        public async Task CreateValueAsync(NewValue newValue)
        {
            PersonalValue personalValue = new()
            {
                GroupId = newValue.GroupId,
                Name = newValue.Name,
                Modifier = newValue.Modifier ?? defaultModifier
            };

            await _personalValuesRepository.CreateValueAsync(personalValue);
        }

        public async Task CreateValueGroupAsync(string name)
        {
            PersonalValueGroup personalValueGroup = new() { Name = name };
            await _personalValuesRepository.CreateValueGroupAsync(personalValueGroup);
        }

        public async Task UpdateValueAsync(UpdatedValue updatedValue)
        {
            PersonalValue personalValue =
                await _personalValuesRepository.GetPersonalValueByIdAsync(updatedValue.ValueId)
                ?? throw new ValueNotFoundException(updatedValue.ValueId);
            personalValue.Name = updatedValue.Name ?? personalValue.Name;
            personalValue.Modifier = updatedValue.Modifier ?? personalValue.Modifier;
            personalValue.GroupId = updatedValue.GroupId ?? personalValue.GroupId;
            await _personalValuesRepository.UpdateValueAsync(personalValue);
        }

        public async Task<ValuesInGroup> GetValuesFromGroupAsync(long groupId)
        {
            PersonalValueGroup personalValueGroup = 
                await _personalValuesRepository.GetAllPersonalValuesByGroupIdAsync(groupId)
                ?? throw new ValueGroupNotFoundException(groupId);
            return new ValuesInGroup()
            {
                GroupName = personalValueGroup.Name,
                Values = personalValueGroup.PersonalValues!.Select(value => new IdAndValue { Id = value.Id, Value = value.Name })
            };
        }

    }
}
