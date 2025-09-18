using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.ViewComponents
{
    public class LoadNews : ViewComponent
    {


        private readonly IAllinone _allinone;
        private readonly IMemoryCache _memoryCache;

        // Constructor for the ListNewsViewComponent class
        public LoadNews(IAllinone allinone, IMemoryCache memoryCache)
        {
            _allinone = allinone;
            _memoryCache = memoryCache;
        }
        // This method is called when the view component is invoked
        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var cacheKey = "ListNewsCacheKey";
                if (!_memoryCache.TryGetValue(cacheKey, out List<NewsVM> listNews))
                {
                    // Nếu cache rỗng, lấy dữ liệu từ service
                    listNews = await _allinone.ListNews();
                    if (listNews == null)
                    {
                        return View("Error", "No data found.");
                    }

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
                    _memoryCache.Set(cacheKey, listNews, cacheEntryOptions);
                }

                // Sắp xếp theo ngày mới nhất (Createat giảm dần)
                var sortedList = listNews
                    .Where(n => n.Createat != null)
                    .OrderByDescending(n => n.Createat)
                    .ToList();

                return View(sortedList);
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }
    }
}

    
