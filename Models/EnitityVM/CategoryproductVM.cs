using System.ComponentModel.DataAnnotations;

namespace websitebenhvien.Models.EnitityVM
{
    public class CategoryproductVM
    {
        public string ? Id_Categoryproduct { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tiêu đề")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập alias")]
        public string Alias_url { get; set; }

        public string ?url { get; set; }
        public string ?Description { get; set; }
        public DateTime? Createat { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn hình ảnh")]
        public IFormFile formFile { get; set; }
    }
}
