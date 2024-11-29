namespace websitebenhvien.Models.EnitityVM
{
    public class HeaderVM
    {
        public string ?logo { get; set; }
        public string telephone { get; set; }
        public List<TitleheaderVM> titleheader { get; set; }
        public IFormFile ? formFile { get; set; }
        public string ? titlelist { get; set; }

    }
}
