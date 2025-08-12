using System.ComponentModel.DataAnnotations;
using websitebenhvien.Models.Enitity;

namespace websitebenhvien.Models.EnitityVM
{
    public class PostactivityVM
    {
        public int Id_Postactivity { get; set; }

        [Required(ErrorMessage = "Tiêu đề là bắt buộc")]
        [StringLength(255, ErrorMessage = "Tiêu đề không được vượt quá 255 ký tự")]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }


        [Required(ErrorMessage = "Alias URL là bắt buộc")]
        [StringLength(255, ErrorMessage = "Alias URL không được vượt quá 255 ký tự")]
        public string Alias_url { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "Từ khóa không được vượt quá 255 ký tự")]
        public string? Keyword { get; set; }

        [StringLength(500, ErrorMessage = "Mô tả ngắn không được vượt quá 500 ký tự")]
        public string? Descriptionshort { get; set; }

        [Required(ErrorMessage = "Danh mục hoạt động là bắt buộc")]
        public int Id_Categoryactivity { get; set; }

        public bool Status { get; set; }

        public string? SchemaMakup { get; set; }
        
        public string? Thumbnail { get; set; }
        
        public DateTime Createat { get; set; } = DateTime.Now;


        public IFormFile? ThumbnailFile { get; set; }

        // Navigation property
        public string? CategoryName { get; set; }
    }

    public class PostactivitySearchVM
    {
        public string? SearchTerm { get; set; }
        public int? CategoryId { get; set; }
        public bool? Status { get; set; }
        public string SortBy { get; set; } = "createat";
        public string SortDirection { get; set; } = "desc";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class PagedPostactivityResult
    {
        public List<PostactivityVM> Items { get; set; } = new List<PostactivityVM>();
        public int TotalItems { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? SearchTerm { get; set; }
        public int? CategoryId { get; set; }
        public bool? Status { get; set; }
        public string SortBy { get; set; } = "createat";
        public string SortDirection { get; set; } = "desc";
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    }
}
