using ValuedInBE.DataControls.Ordering;

namespace ValuedInBE.DataControls.Paging
{
    public class PageConfig
    {
        public int Skip { get; set; }
        public int Size { get; set; }
        public OrderByColumnList OrderByColumns { get; set; }

        public PageConfig(int skip, int size, OrderByColumnList orderByColumns)
        {
            Skip = skip;
            Size = size;
            OrderByColumns = orderByColumns;
        }
    }
}
