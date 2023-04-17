using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ValuedInBE.System.DataControls.Paging
{
    public class Page<TEntity>
    {
        public static Page<TEntity> Empty() => new();
        [BindRequired]
        public IEnumerable<TEntity> Results { get; set; } = Enumerable.Empty<TEntity>();
        [BindRequired]
        public int Total { get; set; } = 0;
        [BindRequired]
        public int PageNo { get; set; } = 0;
    }
}
