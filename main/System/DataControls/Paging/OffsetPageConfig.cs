using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ValuedInBE.System.DataControls.Paging
{
    public class OffsetPageConfig<T>
    {
        public T Offset { get; set; } //The offset might be based on different types
        [BindRequired]
        public int Size { get; set; }
    }
}
