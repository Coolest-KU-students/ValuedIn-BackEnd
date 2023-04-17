using System.Security.Policy;
using ValuedInBE.Feeds.Enums;

namespace ValuedInBE.Feeds.Models.DTOs.Responses
{
    public class OrgFeedItem : IFeedItem
    {
        public FeedType Type => FeedType.ORGANIZATIONS;
        public string Id { get; set; } = string.Empty;
        public byte[] Image { get; set; } = Array.Empty<byte>();
        public List<string> Values { get; set; } = new();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public long? Match { get; set; }
    }
}
