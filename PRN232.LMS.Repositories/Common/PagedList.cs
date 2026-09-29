using Microsoft.EntityFrameworkCore;

namespace PRN232.LMS.Repositories.Common
{
    public class PagedList<T> : List<T>
    {
        public PaginationMetadata Pagination { get; set; }
        
        
        public PagedList(IEnumerable<T> items, PaginationMetadata pagination)
        {
            Pagination = pagination;
            AddRange(items);
        }

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            Pagination = new PaginationMetadata
            {
                TotalItems = count,
                PageSize = pageSize,
                Page = pageNumber,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };
            AddRange(items);
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }

        public static PagedList<T> Create(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}

