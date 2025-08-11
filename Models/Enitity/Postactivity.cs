namespace websitebenhvien.Models.Enitity
{
    public class Postactivity
    {
        public int Id_Postactivity { get; set; }
        public string Title { get; set; }

        public string? Description { get; set; }


        public string Alias_url { get; set; }

        public string? Keyword { get; set; }

        public string? Descriptionshort { get; set; }

        public int Id_Categoryactivity { get; set; }

        public bool Status { get; set; }

        public string? SchemaMakup { get; set; }
        
        public string? Thumbnail { get; set; }
        
        public DateTime Createat { get; set; }= DateTime.Now;

        public Activitycategory Activitycategory { get; set; }
    }
}
