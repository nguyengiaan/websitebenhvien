using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace websitebenhvien.Models.EnitityVM
{
    public class MenuAdminVM
    {
        public int ?id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên menu")]

        public string Title { get; set; }

        public string? Icon { get; set; }

        public string? Url { get; set; }

        [NotMapped]
        public IFormFile ?formFile { get; set; }
    }
}
