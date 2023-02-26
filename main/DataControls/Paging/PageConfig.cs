using ValuedInBE.DataControls.Ordering;

namespace ValuedInBE.DataControls.Paging
{
    public class PageConfig
    {
        public int Page { get; set; }
        public int Size { get; set; }
        public OrderByColumnList OrderByColumns { get; set; }

        public PageConfig(int page, int size, OrderByColumnList orderByColumns)
        {
            Page = page;
            Size = size;
            OrderByColumns = orderByColumns;
        }
    }
}
