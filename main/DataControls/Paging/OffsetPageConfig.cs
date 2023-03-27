using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ValuedInBE.DataControls.Paging
{
    public class OffsetPageConfig<T>
    {
        public T Offset { get; set; }
        [BindRequired]
        public int Size { get; set; }
    }
}
