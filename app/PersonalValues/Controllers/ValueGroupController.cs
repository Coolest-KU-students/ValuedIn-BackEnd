using Microsoft.AspNetCore.Mvc;
using ValuedInBE.PersonalValues.Models.DTOs.Requests;
using ValuedInBE.PersonalValues.Models.DTOs.Response;
using ValuedInBE.PersonalValues.Models.Entities;
using ValuedInBE.PersonalValues.Repositories;
using ValuedInBE.PersonalValues.Service;

namespace ValuedInBE.PersonalValues.Controllers
{

    [Route("api/values/groups")]
    [ApiController]
    public class ValueGroupController : ControllerBase
    {
        private readonly IPersonalValueService _personalValueService;
        private readonly ILogger<ValueGroupController> _logger;


        public ValueGroupController(IPersonalValueService personalValueService, ILogger<ValueGroupController> logger)
        {
            _personalValueService = personalValueService;
            _logger = logger;
        }


        [HttpGet("{id}")]
        public async Task<ValuesInGroup> GetPersonalValuesFromGroupAsync(long id)
        {
            _logger.LogDebug("Received request for values from group {id}", id);
            return await _personalValueService.GetValuesFromGroupAsync(id);
        }


    }
}
