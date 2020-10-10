using System.Collections.Generic;

namespace viewmodels
{
    public class PagedResult<T>
    {
        public int TotalItems { get; set; }
        public IEnumerable<T> Results { get; set; }
    }
}