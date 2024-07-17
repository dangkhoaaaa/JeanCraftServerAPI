namespace JeanCraftServerAPI.Util
{
    public class PageResult<T>
    {
        public IList<T> Items { get; set; }
        public int TotalRecords { get; set; }
        public double TotalAmount { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }
}
