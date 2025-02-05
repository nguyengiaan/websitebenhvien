namespace websitebenhvien.Models.Enitity
{
    public class Videos
    {
        public int Id_video { get; set; }

        public string Title_video { get; set; }

        public string Link_video { get; set; }

        public string ? Description_video { get; set; }

        public bool Status_video { get; set; }

        public DateTime Created_at_video { get; set; }
    }
}
