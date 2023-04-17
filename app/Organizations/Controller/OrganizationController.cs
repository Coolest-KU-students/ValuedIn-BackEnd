using Microsoft.AspNetCore.Mvc;
using ValuedInBE.DataControls.Paging;
using ValuedInBE.Feeds.Models.DTOs.Responses;
using ValuedInBE.Jobs.Models.Entities;
using ValuedInBE.Organizations.Models.DTOs.Requests;
using ValuedInBE.Organizations.Models.Entitites;
using ValuedInBE.PersonalValues.Models.Entities;

namespace ValuedInBE.Organizations.Controller
{

    [Route("api/organizations")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        [HttpPost]
        public long CreateNewOrganization(NewOrganization newOrganization)
        {
            throw new NotImplementedException();
        } 
        [HttpGet("{id}")]
        public Organization GetOrganization(int id) { throw new NotImplementedException();}

        [HttpPut("{id}")]
        public void UpdateOrganization(Organization newOrganization) 
        { 
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public void ArchiveOrganization(int id)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}/valuehistory")]
        public OffsetPage<PersonalValue, DateTime> GetValueHistory() { throw new NotImplementedException(); }

        [HttpGet("{id}/jobs")]
        public OffsetPage<Job, DateTime> GetOrganizationJobs(int id) { throw new NotImplementedException(); }
    }
}
