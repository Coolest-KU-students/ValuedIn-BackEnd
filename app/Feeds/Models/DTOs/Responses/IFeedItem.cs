
using ValuedInBE.Feeds.Enums;

namespace ValuedInBE.Feeds.Models.DTOs.Responses
{
    public interface IFeedItem
    {
        FeedType Type { get; }
        string Id { get; }
        string Name { get;}
        byte[] Image { get; }
        string Description { get; }
        List<string> Values { get; }
        public long? Match { get; }
        
    }
}
