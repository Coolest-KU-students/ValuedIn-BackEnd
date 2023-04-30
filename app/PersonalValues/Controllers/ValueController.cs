using Microsoft.AspNetCore.Mvc;
using ValuedInBE.PersonalValues.Models.DTOs.Requests;
using ValuedInBE.PersonalValues.Models.Entities;
using ValuedInBE.PersonalValues.Service;

namespace ValuedInBE.PersonalValues.Controllers
{
    [Route("api/values")]
    [ApiController]
    public class ValueController : ControllerBase
    {
        private readonly IPersonalValueService _personalValueService;

        public ValueController(IPersonalValueService personalValueService)
        {
            _personalValueService = personalValueService;
        }

        [HttpGet]
        public async Task<IEnumerable<PersonalValue>> ListAllValues([FromQuery] string? search)
        {
            return await _personalValueService.GetAllValuesExceptUsers(search);
        }

        [HttpPost]
        public async Task CreateValue(NewValue value)
        {
            await _personalValueService.CreateValue(value);
        }

        [HttpPut]
        public async Task UpdateValue(UpdatedValue value)
        {
            await _personalValueService.UpdateValue(value);
        }


    }
}
