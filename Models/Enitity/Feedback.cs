using System;
using System.ComponentModel.DataAnnotations;

namespace websitebenhvien.Models.Enitity
{
    // Phản hồi từ người dùng (Feedback)
    public class Feedback
    {

        public int Id { get; set; }
        public string Title_Name { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public bool status { get; set; } = true;

        public string Content { get; set; }

        public string Thumnail { get; set; }




        

    }
}