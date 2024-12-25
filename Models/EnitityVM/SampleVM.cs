using System.ComponentModel.DataAnnotations;

namespace websitebenhvien.Models.EnitityVM
{
    public class SampleVM
    {
        public SampleVM()
        {
            ButtonSamples = new List<ButtonsampleVM>();
        }
        public int ?Id_SampleMessager { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập câu hỏi")]

        public string Question { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập câu trả lời")]

        public string Reply { get; set; }

        public string ?Status { get; set; }

        public string ?SamplesButton { get; set; }    
        public List<ButtonsampleVM> ButtonSamples { get; set; }

    }
}
