using ValuedInBE.Feeds.Enums;

namespace ValuedInBE.Feeds.Models.DTOs.Responses
{
    public class UserFeedItem : IFeedItem
    {
        public FeedType Type => FeedType.USERS;

        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public byte[] Image { get; set; } = Array.Empty<byte>();

        public string Description { get; set; } = string.Empty;

        public List<string> Values { get; set; } = new();

        public long? Match { get; set; }
    }
}
