using System.ComponentModel.DataAnnotations;

namespace websitebenhvien.Models.Enitity
{
    public class Sharecustomer
    {
        public string ?Id_share { get; set; }
        [Required(ErrorMessage ="Tiêu đề không được để trống")]
        public string title { get; set; }
        public bool? status { get; set; }
        [Required(ErrorMessage = "Mô tả không được để trống")]
        public string description { get; set; }
        [Required(ErrorMessage = "AliasUrt không được để trống")]
        public string aliasurl { get; set; }   

        public string ?image { get; set; }
        public DateTime ?Createat { get; set; }
    }
}
