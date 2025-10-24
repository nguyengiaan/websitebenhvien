using System.ComponentModel.DataAnnotations;

namespace websitebenhvien.Models.EnitityVM
{
    public class PricelistVM 
    {
        public int? Id_pricelist { get; set; }
        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Mô tả không được để trống")]

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public DateTime Created_At { get; set; }
    }
}
