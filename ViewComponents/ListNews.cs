using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.ViewComponents
{
    public class ListNewsViewComponent : ViewComponent
    {
        private readonly IAllinone _allinone;
        private readonly IMemoryCache _memoryCache;

        // Constructor for the ListNewsViewComponent class
        public ListNewsViewComponent(IAllinone allinone,IMemoryCache memoryCache)
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
                if (!_memoryCache.TryGetValue(cacheKey, out var listNews))
                {
                    // If the cache is empty, fetch data from the service
                    listNews = await _allinone.ListNews();
                    if (listNews == null)
                    {
                        return View("Error", "No data found.");
                    }

                    // Set cache options
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
                    // Store data in cache
                    _memoryCache.Set(cacheKey, listNews, cacheEntryOptions);
                }
                return View( listNews);

            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log it)
                return View("Error", ex.Message);
            }
          
        }

     
    }
}