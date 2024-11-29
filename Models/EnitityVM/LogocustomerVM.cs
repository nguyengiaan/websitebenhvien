using System.ComponentModel.DataAnnotations;

namespace websitebenhvien.Models.EnitityVM
{
    public class LogocustomerVM
    {
        public string? Id_logo { get; set; }
        [Required(ErrorMessage ="Tên khách hàng không được để trống")]
        public string CustomerName { get; set; }

        public IFormFile ?formFile { get; set; }
    }
}
