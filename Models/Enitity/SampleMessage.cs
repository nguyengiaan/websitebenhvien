namespace websitebenhvien.Models.Enitity
{
    public class SampleMessage
    {
        public SampleMessage()
        {
            ButtonSamples = new List<ButtonSample>();
        }
        public int Id_SampleMessager { get; set; }

        public string Question { get; set; }

        public string Reply { get; set; }

        public string Status { get; set; }

        public List<ButtonSample> ButtonSamples { get; set; }







    }
}
