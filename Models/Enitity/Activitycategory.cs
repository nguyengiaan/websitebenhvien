namespace websitebenhvien.Models.Enitity
{
    public class Activitycategory
    {
        public Activitycategory( ) 
        {
            Postactivities = new List<Postactivity>();

        }
        public int Id_activitycategory { get; set; }
        public string Title { get; set; }
        public string ?Description { get; set; }

        public string link_alias { get; set; }
      
        public List<Postactivity> Postactivities { get; set; } 

    }
}
