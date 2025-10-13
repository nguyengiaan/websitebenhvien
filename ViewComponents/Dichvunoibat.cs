using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using websitebenhvien.Service.Interface;
using websitebenhvien.Models.EnitityVM;
using Microsoft.Extensions.Caching.Memory;

namespace websitebenhvien.ViewComponents
{
    // ViewComponent hiển thị dịch vụ nổi bật trên trang chủ
    public class DichvunoibatViewComponent : ViewComponent
    {
          private readonly IAllinone _allinone;
        private readonly IMemoryCache _memoryCache;

        // Constructor for the ListNewsViewComponent class
        public DichvunoibatViewComponent(IAllinone allinone,IMemoryCache memoryCache)
        {
            _allinone = allinone;
            _memoryCache = memoryCache;
        }
        // This method is called when the view component is invoked
        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var cacheKey = "ListServicesCacheKey";
                if (!_memoryCache.TryGetValue(cacheKey, out var listServices))
                {
                    // If the cache is empty, fetch data from the service
                    listServices = await _allinone.ListService();
                    if (listServices == null)
                    {
                        return View("Error", "No data found.");
                    }

                    // Set cache options
                    var cacheEntryOptions = new MemoryCacheEntryOptions()   
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
                    // Store data in cache
                    _memoryCache.Set(cacheKey, listServices, cacheEntryOptions);
                }
                return View(listServices);

            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log it)
                return View("Error", ex.Message);
            }
          
        }





    }
}