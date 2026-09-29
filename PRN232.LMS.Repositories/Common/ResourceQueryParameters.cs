namespace PRN232.LMS.Repositories.Common
{
    public class ResourceQueryParameters
    {
        const int maxPageSize = 50;
        public int Page { get; set; } = 1;
        
        private int _pageSize = 10;
        public int Size 
        { 
            get => _pageSize; 
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }

        public string? Search { get; set; }
        public string? Sort { get; set; }
        public string? Fields { get; set; }
        public string? Expand { get; set; }
    }
}
