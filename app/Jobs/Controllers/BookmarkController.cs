using Microsoft.AspNetCore.Mvc;
using ValuedInBE.DataControls.Paging;
using ValuedInBE.Jobs.Models.Entities;

namespace ValuedInBE.Jobs.Controllers
{

    [Route("api/jobs/bookmarks")]
    [ApiController]
    public class BookmarkController : ControllerBase
    {
        [HttpGet]
        public OffsetPage<Job, DateTime> GetBookmarkedJobs()
        {
            throw new NotImplementedException();
        }

        [HttpPost("{id}")]
        public void BookmarkJob(long id)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public void DeleteJobBookmark(long id) 
        {
            throw new NotImplementedException();
        }
    }
}