namespace websitebenhvien.Models.Enitity
{
    public class Slidepage
    {
        public string Id_slidepage { get; set; }

        public string title { get; set; }
        public string slide { get; set; }

        public bool status { get; set; }    

        public int sort { get; set; }

        public string link { get; set; }

        public DateTime Time { get; set; }
    }
}
