namespace websitebenhvien.Models.Enitity
{
    public class ButtonSample
    {
        public int Id_ButtonSample { get; set; }
        public string Title { get; set; }

        public string Link { get; set; }

        public int Id_SampleMessage { get; set; }

        public SampleMessage SampleMessage { get; set; }
    }
}
