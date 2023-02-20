namespace ValuedInBE.DataControls.Paging
{
    public class Page<TEntity>
    {
        public static Page<TEntity> Empty() => new(new(), 0, 0);
        public List<TEntity> Results { get; set; }
        public int Total { get; set; }
        public int PageNo { get; set; }
        public int Count => Results.Count;

        public Page(List<TEntity> results, int total, int pageNo)
        {
            Results = results;
            Total = total;
            PageNo = pageNo;
        }

    }
}
