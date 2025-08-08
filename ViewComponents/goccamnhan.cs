using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.ViewComponents
{
    public class goccamnhan : ViewComponent
    {
        private readonly IReceivingAngle _gcn;
        private readonly IMemoryCache _memoryCache;

        // Constructor to inject the IReceivingAngle service


        public goccamnhan(IReceivingAngle gcn, IMemoryCache memoryCache)
        {
            _gcn = gcn;
            _memoryCache = memoryCache;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                // Check if the data is already cached
                if (_memoryCache.TryGetValue("ReceivingAngleCacheKey", out List<ReceivingAngleVM> cachedReceivingAngles))
                {
                    // If cached, return the cached data
                    return View(cachedReceivingAngles);
                }
                var model = await _gcn.GetAllReceivingAnglesAsync();
                var model2 = model.Where(x => x.status == true).ToList();

                // Cache the result
                _memoryCache.Set("ReceivingAngleCacheKey", model2);
    
                return View(model2);
            }
            catch (Exception)
            {
                // Handle the exception as needed, e.g., log it or return an empty view
                return View(new List<ReceivingAngleVM>());

            }
            
          
        }

    }
}