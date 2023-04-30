using Microsoft.AspNetCore.Authorization;
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
        private readonly ILogger<ValueController> _logger;

        public ValueController(IPersonalValueService personalValueService, ILogger<ValueController> logger)
        {
            _personalValueService = personalValueService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<PersonalValue>> ListAllValues([FromQuery] string? search)
        {
            _logger.LogTrace("Got a request to list all values with filter '{FilterSearch}'", search);
            return await _personalValueService.GetAllValuesExceptUsers(search);
        }

        [Authorize(Roles="SYS_ADMIN")]
        [HttpPost]
        public async Task CreateValue(NewValue value)
        {
            _logger.LogTrace("Got a request to register value {ValueName}", value.Name);
            await _personalValueService.CreateValue(value);
        }

        [Authorize(Roles = "SYS_ADMIN")]
        [HttpPut]
        public async Task UpdateValue(UpdatedValue value)
        {
            _logger.LogTrace("Got a request to update value with id {ValueId}", value.ValueId);
            await _personalValueService.UpdateValue(value);
        }
    }
}
