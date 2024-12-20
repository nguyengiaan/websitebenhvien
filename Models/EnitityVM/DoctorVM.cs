using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace websitebenhvien.Models.EnitityVM
{
    public class DoctorVM
    {
        public int? Id_doctor { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên bác sĩ")]
        public string Name { get; set; }
        public string ?Thumnail { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập giới thiệu bác sĩ")]
        public string Introduction { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tổ chức bác sĩ")]
        public string Organization { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập giải thưởng bác sĩ")]
        public string Award { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập nghiên cứu bác sĩ")]
        public string Research { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập đào tạo bác sĩ")]
        public string Training { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập kinh nghiệm làm việc bác sĩ")]
        public string Experiencework { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập chuyên khoa bác sĩ")]
        public int Id_specialty { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập alias url bác sĩ")]
        public string Alias_url { get; set; }
        public IFormFile ?ImageFile { get; set; }

        public string ? nameCategory { get; set; }
    }
}

