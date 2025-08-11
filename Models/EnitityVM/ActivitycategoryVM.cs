using System.ComponentModel.DataAnnotations;

namespace websitebenhvien.Models.EnitityVM
{
    public class ActivitycategoryVM
    {
        public int Id_activitycategory { get; set; }

        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        [StringLength(200, ErrorMessage = "Tiêu đề không được vượt quá 200 ký tự")]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Mô tả không được vượt quá 1000 ký tự")]
        public string? Description { get; set; }
    

    }

    public class ActivityCategorySearchVM
    {
        public string? SearchTerm { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = "Title";
        public string SortDirection { get; set; } = "asc";
    }

    public class PagedActivityCategoryResult
    {
        public List<ActivitycategoryVM> Items { get; set; } = new List<ActivitycategoryVM>();
        public int TotalItems { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public bool HasPreviousPage => Page > 1;
        public bool HasNextPage => Page < TotalPages;
        public string? SearchTerm { get; set; }
        public string SortBy { get; set; } = "Title";
        public string SortDirection { get; set; } = "asc";
    }
}
