using System.Collections.Generic;

namespace Shopsy_Project.Common
{
    public class PaginationFilter
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public string? SortBy { get; set; }
        public bool IsAscending { get; set; } = true;
        public int Skip => (PageNumber - 1) * PageSize;
        public bool isWishListFilter { get; set; }
    }

    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
