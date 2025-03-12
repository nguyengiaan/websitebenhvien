namespace websitebenhvien.Models.EnitityVM
{
    public class ForbusinessVM
    {
        public int Id_Forbusiness { get; set; }


        public string Name_Forbusiness { get; set; }


        public string Content_Forbusiness { get; set; }

        public bool Status_Forbusiness { get; set; }

        public DateTime Create_at { get; set; }

        public string? Icon_Forbusiness { get; set; }

        public string? link_Forbusiness { get; set; }

        public string? link_Forbusiness_1 { get; set; }

        public IFormFile formFile { get; set; }
    }
}
