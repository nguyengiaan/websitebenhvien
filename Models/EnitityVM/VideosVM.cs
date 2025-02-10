using System.ComponentModel.DataAnnotations;

namespace websitebenhvien.Models.EnitityVM
{
    public class VideosVM
    {
        public int Id_video { get; set; }
        [Required(ErrorMessage = "Tiêu đề video không được để trống")]
        public string? Title_video { get; set; }
        public string? Link_video { get; set; }
        public string? Description_video { get; set; }
        public bool Status_video { get; set; }
        public DateTime ?Created_at_video { get; set; }
        public IFormFile ? formFile { get; set; }
    }
}
