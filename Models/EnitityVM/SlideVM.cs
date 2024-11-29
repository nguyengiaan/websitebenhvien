using System.ComponentModel.DataAnnotations;

namespace websitebenhvien.Models.EnitityVM
{
    public class SlideVM
    {
        public string? id_slidepage { get; set; }
        public string ?link { get; set; }
        [Required(ErrorMessage = "Sắp xếp không được để trống")]
        public int sort { get; set; }
        [Required(ErrorMessage ="Tiêu đề không được để trống")]
        public string title { get; set; }
        [Required(ErrorMessage = "Hình ảnh là bắt buộc")]
        public IFormFile formFile { get; set; }
    }
}
