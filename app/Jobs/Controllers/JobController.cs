using Microsoft.AspNetCore.Mvc;
using ValuedInBE.DataControls.Paging;
using ValuedInBE.Feeds.Models.DTOs.Responses;
using ValuedInBE.Jobs.Models.Entities;

namespace ValuedInBE.Jobs.Controllers
{
    [Route("api/jobs")]
    [ApiController]
    public class JobController : ControllerBase
    {
        [HttpGet("feed")]
        public OffsetPage<JobFeedItem, DateTime> GetJobFeed()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public Job GetJob(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost("{id}")]
        public Job CreateJob(Job job)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        public Job UpdateJob(Job job)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public void ArchiveJob(int id)
        {
            throw new NotImplementedException();
        }



    }
}
