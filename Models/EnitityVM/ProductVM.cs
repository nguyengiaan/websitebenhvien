using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using websitebenhvien.Models.Enitity;

namespace websitebenhvien.Models.EnitityVM
{
    public class ProductVM
    {
        [Required(ErrorMessage = "Vui lòng nhập mã sản phẩm")]
        public string Product_Id { get; set; }
        public string ?Id_product { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]

        public string Title { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Alias")]

        public string Alias_url { get; set; }


        public string ?url { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]

        public string Description { get; set; }


        public DateTime ?Createat { get; set; }

        public bool? status { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn danh mục")]
        public string ?Id_Categoryproduct { get; set; }

        public List<IFormFile>? Images { get; set; }   
        [Required(ErrorMessage = "Vui lòng chọn hình ảnh ảnh đại diện")]

        public IFormFile ImageThumnail { get; set; }

        public List<Proimages>? Proimages { get; set; }

        public List<string>? ImagesDelete { get; set; }
    }
}
