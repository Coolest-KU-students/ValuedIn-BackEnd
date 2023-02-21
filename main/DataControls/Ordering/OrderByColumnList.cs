namespace ValuedInBE.DataControls.Ordering
{
    public class OrderByColumnList : List<OrderByColumn>
    {
        public string AsLinqOrderBy()
        {
            return string.Join(
                    ", ",
                    this.Select(OrderByAsString));
        }

        private static string OrderByAsString(OrderByColumn order)
        {
            return $"{order.Column} {(order.Ascending ? "ASC" : "DESC")}";
        }
    }
}
