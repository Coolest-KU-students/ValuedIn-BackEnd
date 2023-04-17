using Microsoft.AspNetCore.Mvc.ModelBinding;
using ValuedInBE.DataControls.Ordering;

namespace ValuedInBE.DataControls.Paging
{
    public class PageConfig
    {
        [BindRequired]
        public int Page { get; set; }

        [BindRequired]
        public int Size { get; set; }

        [BindRequired]
        public OrderByColumnList OrderByColumns { get; set; } = null!;

        public PageConfig() { }

        protected PageConfig(int page, int size, OrderByColumnList orderByColumns)
        {
            Page = page;
            Size = size;
            OrderByColumns = orderByColumns;
        }
    }
}
