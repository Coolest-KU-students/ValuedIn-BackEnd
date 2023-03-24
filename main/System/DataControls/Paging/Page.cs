using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ValuedInBE.System.DataControls.Paging
{
    public class Page<TEntity>
    {
        public static Page<TEntity> Empty() => new(new List<TEntity>(), 0, 0);
        [BindRequired]
        public IEnumerable<TEntity> Results { get; set; }
        [BindRequired]
        public int Total { get; set; }
        [BindRequired]
        public int PageNo { get; set; }

        public Page() { }

        protected Page(IEnumerable<TEntity> results, int total, int pageNo)
        {
            Results = results;
            Total = total;
            PageNo = pageNo;
        }
    }
}
