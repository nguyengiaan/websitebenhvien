namespace websitebenhvien.Models.EnitityVM
{
    public class TitlemenuVM
    {
        public int? Id_titlemenu { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Url_icon { get; set; }

        public IFormFile? formFile { get; set; }
    }

    public class TitlemenuSearchVM
    {
        public string? SearchTerm { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = "Name";
        public string SortDirection { get; set; } = "asc";
    }

    public class PagedTitlemenuResult
    {
        public List<TitlemenuVM> Items { get; set; } = new List<TitlemenuVM>();
        public int TotalItems { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public bool HasPreviousPage => Page > 1;
        public bool HasNextPage => Page < TotalPages;
        public string? SearchTerm { get; set; }
        public string SortBy { get; set; } = "Name";
        public string SortDirection { get; set; } = "asc";
    }
}
