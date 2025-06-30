using System;
using System.Collections.Generic;

namespace websitebenhvien.Models.EnitityVM
{
    public class Breadcrumb
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; }

        public Breadcrumb() { }

        public Breadcrumb(string title, string url, bool isActive = false)
        {
            Title = title;
            Url = url;
            IsActive = isActive;
        }
    }
}