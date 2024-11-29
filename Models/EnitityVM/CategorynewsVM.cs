using System.ComponentModel.DataAnnotations;

namespace websitebenhvien.Models.EnitityVM
{
    public class CategorynewsVM
    {
        public string ?Id_Categorynews { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tiêu đề")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập alias")]
        public string Alias_url { get; set; }
        public string? Description { get; set; }

        public DateTime ? Createat { get; set; }
    }
}
