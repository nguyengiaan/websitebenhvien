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

        public string? link_alias { get; set; }
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

    // ViewModel cho trang hiển thị bài viết theo danh mục
    public class ActivityCategoryPageVM
    {
        public ActivitycategoryVM Category { get; set; } = new();
        public List<PostactivityVM> Posts { get; set; } = new List<PostactivityVM>();
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public int PageSize { get; set; } = 12;
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
    }

    // ViewModel cho trang tổng hợp hoạt động
    public class ActivityIndexPageVM
    {
        public List<PostactivityVM> Posts { get; set; } = new List<PostactivityVM>();
        public List<ActivitycategoryVM> Categories { get; set; } = new List<ActivitycategoryVM>();
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public int PageSize { get; set; } = 12;
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
        public string? SearchTerm { get; set; }
    }
}
