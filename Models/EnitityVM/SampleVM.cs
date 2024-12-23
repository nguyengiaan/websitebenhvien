namespace websitebenhvien.Models.EnitityVM
{
    public class SampleVM
    {
        public int ?Id_SampleMessager { get; set; }

        public string Question { get; set; }

        public string Reply { get; set; }

        public string Status { get; set; }

        public List<ButtonsampleVM> ButtonSamples { get; set; }

    }
}
