using Microsoft.AspNetCore.Mvc;
using ValuedInBE.DataControls.Paging;
using ValuedInBE.Organizations.Models.DTOs.Responses;
using ValuedInBE.System.DataControls.Paging;
using ValuedInBE.Users.Models.Entities;

namespace ValuedInBE.Organizations.Controller
{
    [Route("api/organizations/{organizationId}/employees")]
    [ApiController]
    public class OrganizationEmployeeController : ControllerBase
    {
        [HttpGet]
        public Page<Employee> GetEmployees(int organizationId, PageConfig pageConfig)
        {
            throw new NotImplementedException();
        }

        [HttpPost("{userId}")]
        public void AddEmployee(int organizationId, string userId) 
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{userId}")]
        public void RemoveEmployee(int organizationId, string userId)
        {
            throw new NotImplementedException();
        }
    }
}
