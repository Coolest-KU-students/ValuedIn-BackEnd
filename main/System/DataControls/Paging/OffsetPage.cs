namespace ValuedInBE.DataControls.Paging
{
    public class OffsetPage<TEntity, T>
    {
        public IEnumerable<TEntity> Results { get; set; }
        public T NextOffset { get; set; }
        public bool Last { get; set; }
    }
}