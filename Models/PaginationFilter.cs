using System.Collections.Generic;

namespace Shopsy_Project.Common
{
    public class PaginationFilter
    {
        public int PageNumber { get; set; } = 1; // Default to page 1
        public int PageSize { get; set; } = 10;  // Default page size
        public string? SearchTerm { get; set; }  // Optional search term for filtering
        public string? SortBy { get; set; }      // Column to sort by
        public bool IsAscending { get; set; } = true; // Sort order

        public int Skip => (PageNumber - 1) * PageSize; // Calculate number of items to skip
    }

    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; } // Total number of records
        public int PageNumber { get; set; } // Current page number
        public int PageSize { get; set; }   // Page size
    }
}
