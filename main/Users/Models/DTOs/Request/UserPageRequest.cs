using ValuedInBE.DataControls.Ordering;
using ValuedInBE.DataControls.Paging;

namespace ValuedInBE.Users.Models.DTOs.Request
{
    public class UserPageRequest : PageConfig
    {
        public bool ShowExpired { get; init; }
        public UserPageRequest(int page, int size, OrderByColumnList orderByColumns, bool showExpired) : base(page, size, orderByColumns)
        {
            ShowExpired = showExpired;
        }
    }
}
