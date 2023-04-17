namespace ValuedInBE.DataControls.Paging
{
    public class OffsetPage<TEntity, TOffsetType>
    {
        public IEnumerable<TEntity> Results { get; set; } = null!;
        public TOffsetType? NextOffset { get; set; } 
        public bool Last { get; set; }
    }
}