using System.ComponentModel.DataAnnotations;

namespace websitebenhvien.Models.EnitityVM
{
    public class CategoryActivityVM
    {
        public string Id_Categoryactivity { get; set; } = "";
        
        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        [StringLength(200, ErrorMessage = "Tiêu đề không được vượt quá 200 ký tự")]
        public string Title { get; set; } = "";
        
        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
        public string? Description { get; set; }
        
        [Required(ErrorMessage = "Link alias không được để trống")]
        [StringLength(200, ErrorMessage = "Link alias không được vượt quá 200 ký tự")]
        public string Link_alias { get; set; } = "";
        
        public int Sort { get; set; } = 0;
        
        public bool Status { get; set; } = true;
        
        public DateTime Createat { get; set; } = DateTime.Now;
        
        public DateTime? Updateat { get; set; }
        
        // Thuộc tính bổ sung cho hiển thị
        public int PostCount { get; set; } = 0; // Số lượng bài viết trong danh mục
        
        public string DisplayName => !string.IsNullOrEmpty(Title) ? Title : "Chưa có tên";
        
        public string StatusText => Status ? "Hoạt động" : "Ngừng hoạt động";
        
        public string CreateDateFormatted => Createat.ToString("dd/MM/yyyy HH:mm");
    }
}