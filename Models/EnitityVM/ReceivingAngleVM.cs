using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace websitebenhvien.Models.EnitityVM
{
    public class ReceivingAngleVM
    {
        public int? id { get; set; }
        [Required(ErrorMessage = "Không được để trống tiêu đề")]

        public string title { get; set; }

        public string? description { get; set; }

        public string? image { get; set; }

        public string? link { get; set; }
        public bool status { get; set; }
        public DateTime created_at { get; set; } = DateTime.Now;

        public int? order { get; set; }

        [NotMapped]
        public IFormFile ? formFile { get; set; }
    }
}