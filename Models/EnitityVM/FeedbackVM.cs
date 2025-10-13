using System.ComponentModel.DataAnnotations;

namespace websitebenhvien.Models.EnitityVM
{
    public class FeedbackVM
    {
        public int ? Id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tiêu đề phản hồi")]
        public string Title_Name { get; set; }

        public DateTime ? CreatedDate { get; set; } = DateTime.Now;

        public bool ? status { get; set; } = true;

        [Required(ErrorMessage = "Vui lòng nhập nội dung phản hồi")]

        public string Content { get; set; }

        public string? Thumnail { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn tệp hình ảnh")]

        public IFormFile formFile { get; set; } 
    }
}
