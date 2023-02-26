using ValuedInBE.DataControls.Ordering;
using ValuedInBE.DataControls.Paging;

namespace ValuedInBE.Models.DTOs.Requests.Users
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
