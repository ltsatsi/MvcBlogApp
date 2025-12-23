namespace MyBlogApplication.Infrastructure
{
    // Generic class
    // Constrained to be a generic List<T>
    public class PaginatedList<T> : List<T>
    {
        // Track current and total pages
        public int PageIndex { get; set; }
        public int TotalPages { get; private set; }
        public int TotalCount { get; private set; }

        // Non-default constructor
        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            TotalCount = count;
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count/(double)pageSize);

            this.AddRange(items);
        }


        // Check if data has previous and next
        // data contents
        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }

        // Main Logic Pagination
        public static PaginatedList<T> Create(IEnumerable<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
