using Microsoft.AspNetCore.Mvc;
using ValuedInBE.DataControls.Paging;
using ValuedInBE.Jobs.Models.Entities;

namespace ValuedInBE.Jobs.Controllers
{
    [Route("api/jobs/applications")]
    [ApiController]
    public class JobApplicationController : ControllerBase
    {
        [HttpGet]
        public OffsetPage<Job, DateTime> GetJobsUserAppliedTo()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public OffsetPage<JobApplicants, DateTime> GetJobApplicants(long id)
        {
            throw new NotImplementedException();
        }

        [HttpPost("{id}")]
        public void ApplyToJob(long id)
        {
            throw new NotImplementedException();
        }

    }
}
