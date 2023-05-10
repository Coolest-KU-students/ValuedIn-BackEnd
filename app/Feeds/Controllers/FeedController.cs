using Microsoft.AspNetCore.Mvc;
using ValuedInBE.DataControls.Paging;
using ValuedInBE.Feeds.Enums;
using ValuedInBE.Feeds.Models.DTOs.Responses;

namespace ValuedInBE.Feeds.Controllers
{

    [Route("api/feeds")]
    [ApiController]
    public class FeedController : ControllerBase
    {
        [HttpGet("{type}")]
        public OffsetPage<IFeedItem, string> GetSpecificFeed(FeedType type) { 
            throw new NotImplementedException(); 
        }

        [HttpGet]
        public OffsetPage<IFeedItem, string> GetAllFeeds()
        {
            throw new NotImplementedException();
        }
        
    }
}
