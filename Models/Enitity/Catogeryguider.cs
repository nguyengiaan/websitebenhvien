using Websitebenhvien.Models.Enitity;

namespace websitebenhvien.Models.Enitity
{
    public class Catogeryguider
    {
        public Catogeryguider()
        {
            Customerguide = new List<Customerguide>();
        }
        public int Id_Catogeryguider { get; set; }
        public string Title { get; set; }

        public string Alias_url { get; set; }
        public string? Description { get; set; }

        public DateTime Createat { get; set; }

        public List<Customerguide> Customerguide { get; set; }
    }
}
