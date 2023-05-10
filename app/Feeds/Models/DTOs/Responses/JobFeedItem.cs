
using ValuedInBE.Feeds.Enums;

namespace ValuedInBE.Feeds.Models.DTOs.Responses
{
    public class JobFeedItem : IFeedItem
    {
        public FeedType Type => FeedType.JOBS;
        public string Name => Title;
        public byte[] Image => Avatar;
        public string Id { get; set; } = string.Empty;
        public byte[] Avatar { get; set; } = Array.Empty<byte>();
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Values { get; set; } = new();
        public long? Match { get; set; }
        public long OrganizationId { get; set; }
        public byte[]? OrganizationBanner { get; set; }
        public string OrganizationTitle { get; set; } = string.Empty;
    }
}
