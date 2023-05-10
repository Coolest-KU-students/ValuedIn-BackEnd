using Microsoft.AspNetCore.Mvc;
using ValuedInBE.PersonalValues.Models.Entities;

namespace ValuedInBE.PersonalValues.Controllers
{
    [Route("api/values")]
    [ApiController]
    public class ValueController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<PersonalValue> ListAllValues()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public void CreateValue(PersonalValue value)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public void UpdateValue(PersonalValue value)
        {
            throw new NotImplementedException();
        }
    }
}
