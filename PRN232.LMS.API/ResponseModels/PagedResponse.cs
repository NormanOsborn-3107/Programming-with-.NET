using PRN232.LMS.Repositories.Common;

namespace PRN232.LMS.API.ResponseModels
{
    public class PagedResponse<T>
    {
        public IEnumerable<T> Data { get; set; }
        public PaginationMetadata Pagination { get; set; }
        
        public PagedResponse(IEnumerable<T> data, PaginationMetadata pagination)
        {
            Data = data;
            Pagination = pagination;
        }
    }
}
