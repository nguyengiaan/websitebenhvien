using System.ComponentModel.DataAnnotations;

namespace websitebenhvien.Models.EnitityVM
{
    public class NewsVM
    {
        public string ?Id_News { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tiêu đề")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập chi tiết")]
        public string Description { get; set; }

        public string ?Url { get; set; }
        public string ?Alias_url { get; set; }

        public string ?Id_Categorynews { get; set; }

        public DateTime ?Createat { get; set; }

        public IFormFile ? formFile { get; set; }
    }
}
