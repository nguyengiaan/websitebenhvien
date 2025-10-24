namespace websitebenhvien.Models.EnitityVM
{
    public class CatogeryguiderVM
    {
        public int  ? Id_Catogeryguider { get; set; }
        public string Title { get; set; }

        public string Alias_url { get; set; }
        public string? Description { get; set; }

        public DateTime Createat { get; set; } =DateTime.Now;
    }
}
