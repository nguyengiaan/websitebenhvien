namespace websitebenhvien.Models.Enitity
{
    public class Pricelist
    {
        public int Id_pricelist { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public DateTime Created_At { get; set; }

    }
}
