using System.Collections.Generic;

namespace DB.Data.Queries
{
    public class ListWithTotals<T> where T: class 
    {
        public ICollection<T> List { get; set; }
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int SkippedItems { get; set; }
    }
}