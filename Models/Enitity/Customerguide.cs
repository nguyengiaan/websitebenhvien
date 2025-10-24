using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using websitebenhvien.Models.Enitity;

namespace Websitebenhvien.Models.Enitity
{
  
    public class Customerguide
    {
        public int Id_Customerguide { get; set; }
        public string Title { get; set; }

        public string? Description { get; set; }

        public string Url { get; set; }
        public string Alias_url { get; set; }

        public string? Keyword { get; set; }

        public string? Descriptionshort { get; set; }

        public int Id_Catogeryguider { get; set; }

        public bool Status { get; set; }

        public string? SchemaMakup { get; set; }


        public DateTime Createat { get; set; }
    public Catogeryguider? Catogeryguider { get; set; }
    }
}