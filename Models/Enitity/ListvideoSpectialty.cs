
using System;
using System.ComponentModel.DataAnnotations;
namespace websitebenhvien.Models.Enitity
{
    public class ListvideoSpectialty
    {
        public int Id_ListvideoSpectialty { get; set; }

        public string Title { get; set; }

        public string Category { get; set; }

        public string Url { get; set; }

        public int Id_Specialty { get; set; }

        public Specialty Specialty { get; set; }


    }
}
