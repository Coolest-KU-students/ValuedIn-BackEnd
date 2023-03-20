namespace ValuedInBE.System.DataControls.Paging
{
    public class Page<TEntity>
    {
        public static Page<TEntity> Empty() => new(new List<TEntity>(), 0, 0);
        public IEnumerable<TEntity> Results { get; set; }
        public int Total { get; set; }
        public int PageNo { get; set; }
        public int Count => Results.Count();

        public Page(IEnumerable<TEntity> results, int total, int pageNo)
        {
            Results = results;
            Total = total;
            PageNo = pageNo;
        }
    }
}
