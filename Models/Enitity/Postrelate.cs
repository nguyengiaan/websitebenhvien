using System;
using System.ComponentModel.DataAnnotations;

namespace websitebenhvien.Models.Enitity
{
    public class Postrelate
    {
        public int Id_Postrelate { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string Url { get; set; }
        public string Alias_url { get; set; }
        public bool Status { get; set; }
        public DateTime Createat { get; set; }

        public int Id_Specialty { get; set; }   
        public Specialty Specialty { get; set; }

    }
}
