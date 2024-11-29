namespace websitebenhvien.Models.Enitity
{
    public class Categorynews
    {
        public Categorynews()
        {
            News = new List<News>();
        }
        public string Id_Categorynews { get; set; }
        public string Title { get; set; }

        public string Alias_url { get; set; }
        public string? Description { get; set; }

        public DateTime Createat { get; set; }



        public List<News> News { get; set; }


    }
}
