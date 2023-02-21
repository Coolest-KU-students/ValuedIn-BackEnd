namespace ValuedInBE.DataControls.Ordering
{
    public class OrderByColumn
    {
        public string Column { get; set; }
        public bool Ascending { get; set; }
        //public string ExpressInString() => $"{Column} {(Ascending ? "ASC" : "DESC")}";

        public OrderByColumn(string column, bool ascending)
        {
            Column = column;
            Ascending = ascending;
        }
    }
}
