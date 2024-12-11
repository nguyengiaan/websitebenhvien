using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using websitebenhvien.Data;
using websitebenhvien.Models.Enitity;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.Service.Reponser
{
    public class PostReponser : IPost
    {
        private readonly MyDbcontext _context;
        public PostReponser(MyDbcontext context)
        {
            _context = context;
        }
        public async Task<News> GetNewsById(string alias_url)
        {
            try
            {
                var news = await _context.News.Where(x => x.Alias_url == alias_url).FirstOrDefaultAsync();
                return news;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
