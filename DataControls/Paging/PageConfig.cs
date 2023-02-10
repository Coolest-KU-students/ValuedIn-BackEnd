namespace ValuedInBE.DataControls.Paging
{
    public class PageConfig
    {

        public int Skip { get; set; }
        public int Size { get; set; }
        public List<OrderByColumn> OrderByColumns { get; set; }

        public PageConfig(int skip, int size, List<OrderByColumn> orderByColumns)
        {
            Skip = skip;
            Size = size;
            OrderByColumns = orderByColumns;
        }
    }
}
